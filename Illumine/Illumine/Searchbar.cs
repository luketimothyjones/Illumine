using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Illumine
{
    public partial class Searchbar : Form
    {
        public CancellationTokenSource resultListUpdateCancelHandler = null;

        // ====
        private readonly IntPtr mainWindow;
        private SearchResults searchResults = null;

        private GlobalHotkeys.GlobalHotkeys showHotkey;  // We need this to be defined for proper disposal
        private Dictionary<string, int> keybind;
        private KeybindSetter keybindSetter;

        private readonly SearchEngine searchEngine;
        private static readonly List<Keys> searchInputIgnoreKeys = new() { Keys.Left, Keys.Right, Keys.Home, Keys.End };

        // ========
        public Searchbar()
        {
            InitializeComponent();

            // Set the monitor to be displayed on
            ShowOnScreen(Properties.Settings.Default.DefaultMonitor);

            // Round the corners
            Region = Region.FromHrgn(WinDisplayFuncs.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Glue the searchbar window to the desktop
            mainWindow = Handle;
            WinDisplayFuncs.SetParent(mainWindow, WinDisplayFuncs.GetDesktopWindow());

            SearchInput.SelectionAlignment = HorizontalAlignment.Center;
            AddSearchInputContextMenu();

            // Capture ESC regardless of control focus
            KeyPreview = true;

            keybind = new Dictionary<string, int>()
            {
                { "keys", Properties.Settings.Default.KeybindKeys },  // (int)Keys.OemPeriod
                { "mods", Properties.Settings.Default.KeybindMods }   // (int)(GlobalHotkeys.Modifiers.Ctrl | GlobalHotkeys.Modifiers.Win)
            };

            try
            {
                showHotkey = new GlobalHotkeys.GlobalHotkeys((GlobalHotkeys.Modifiers)keybind["mods"], (Keys)keybind["keys"], this, true);
            }
            catch (GlobalHotkeys.GlobalHotkeysException)
            {
                keybind["keys"] = (int)Keys.OemPeriod;
                keybind["mods"] = (int)(GlobalHotkeys.Modifiers.Ctrl | GlobalHotkeys.Modifiers.Win);

                Properties.Settings.Default.KeybindKeys = keybind["keys"];
                Properties.Settings.Default.KeybindMods = keybind["mods"];
                Properties.Settings.Default.Save();

                showHotkey = new GlobalHotkeys.GlobalHotkeys((GlobalHotkeys.Modifiers)keybind["mods"], (Keys)keybind["keys"], this, true);

                MessageBox.Show("Keybind in user settings was invalid, it has been reset to CTRL + WIN + Period (full-stop)", "Invalid keybind", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            searchEngine = new SearchEngine();
        }

        private void ExitCleanup()
        {
            showHotkey.Unregister();
            showHotkey.Dispose();
            searchEngine.ClearSearch();
            Environment.Exit(0);
        }

        public void AddSearchInputContextMenu()
        {
            if (SearchInput.ContextMenuStrip == null)
            {
                ContextMenuStrip contextMenu = new()
                {
                    ShowImageMargin = false
                };

                ToolStripMenuItem tsmiCut = new("Cut");
                tsmiCut.Click += (sender, e) => SearchInput.Cut();
                contextMenu.Items.Add(tsmiCut);

                ToolStripMenuItem tsmiCopy = new("Copy");
                tsmiCopy.Click += (sender, e) => SearchInput.Copy();
                contextMenu.Items.Add(tsmiCopy);

                ToolStripMenuItem tsmiPaste = new("Paste");
                tsmiPaste.Click += (sender, e) => SearchInput.Paste();
                contextMenu.Items.Add(tsmiPaste);

                ToolStripMenuItem tsmiSelectAll = new("Select All");
                tsmiSelectAll.Click += (sender, e) => SearchInput.SelectAll();
                contextMenu.Items.Add(tsmiSelectAll);

                contextMenu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem tsmiSetKeybind = new("Set Keybind");
                tsmiSetKeybind.Click += (sender, e) =>
                {
                    keybindSetter = new KeybindSetter();
                    keybindSetter.RegisterCallback(HotkeySetCallback);
                    keybindSetter.Show();
                };
                contextMenu.Items.Add(tsmiSetKeybind);

                ToolStripMenuItem tsmiSetMonitor = new("Set Monitor");
                tsmiSetMonitor.Click += (sender, e) =>
                {
                    MonitorSetter monitorSetter = new();
                    monitorSetter.callbacks += (int screen) => { Properties.Settings.Default.DefaultMonitor = screen; Properties.Settings.Default.Save(); };
                    monitorSetter.callbacks += ShowOnScreen;
                    monitorSetter.callbacks += (int screen) => { if (searchResults != null) searchResults.ShowOnScreen(screen); };

                    monitorSetter.Show();
                };
                contextMenu.Items.Add(tsmiSetMonitor);

                contextMenu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem tsmiExit = new("Exit");
                tsmiExit.Click += (sender, e) => ExitCleanup();
                contextMenu.Items.Add(tsmiExit);

                contextMenu.Opening += (sender, e) =>
                {
                    tsmiCut.Enabled = SearchInput.SelectionLength > 0;
                    tsmiCopy.Enabled = SearchInput.SelectionLength > 0;
                    tsmiPaste.Enabled = Clipboard.ContainsText();
                    tsmiSelectAll.Enabled = SearchInput.TextLength > 0 && SearchInput.SelectionLength < SearchInput.TextLength;
                    tsmiSetMonitor.Enabled = Screen.AllScreens.Length > 1;
                };

                SearchInput.ContextMenuStrip = contextMenu;
            }
        }

        #region Window position and focus handling

        protected override CreateParams CreateParams
        {
            // Hide window from ALT+TAB
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public void TakeFocus()
        {
            WinDisplayFuncs.SetWindowPos(mainWindow, WinDisplayFuncs.HWND_TOP, 0, 0, 0, 0,
                                         (uint)(WinDisplayFuncs.WindowPosAttr.NOMOVE | WinDisplayFuncs.WindowPosAttr.NOSIZE));
            ActiveControl = SearchInput;
            SearchInput.Text = "";
            WinDisplayFuncs.SetForegroundWindow(mainWindow);
        }

        public void LoseFocus()
        {
            ActiveControl = null;
            SearchInput.Text = "";
            TopMost = false;
            SendWindowToBack();
        }

        void ShowOnScreen(int screenIndex)
        {
            Screen screen = Screen.AllScreens[Math.Min(Screen.AllScreens.Length - 1, screenIndex)];
            Location = new(screen.Bounds.Location.X + (screen.Bounds.Width / 2) - (Width / 2), screen.Bounds.Location.Y + 20);
        }

        private void SendWindowToBack()
        {
            // Pin searchbar to lowest z-index
            WinDisplayFuncs.SetWindowPos(mainWindow, WinDisplayFuncs.HWND_BOTTOM, 0, 0, 0, 0,
                                         (uint)(WinDisplayFuncs.WindowPosAttr.NOACTIVATE | WinDisplayFuncs.WindowPosAttr.NOMOVE | WinDisplayFuncs.WindowPosAttr.NOSIZE));
        }

        private void Searchbar_VisibleChanged(object sender, EventArgs e)
        {
            // Send to back of window stack on load
            WindowState = FormWindowState.Minimized;
            SendWindowToBack();
        }

        private void Searchbar_SizeChanged(object sender, EventArgs e)
        {
            // Reset to normal size on size change (minimize/maximize)
            if (WindowState != FormWindowState.Normal)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void Searchbar_Load(object sender, EventArgs e)
        {
            // Hide caret by focusing hidden label
            Top = 20;
            ActiveControl = FocusThiefLabel;
            SendWindowToBack();
        }

        private void Searchbar_Deactivate(object sender, EventArgs e)
        {
            if (!ContainsFocus && (searchResults == null || !searchResults.ContainsFocus))
            {
                LoseFocus();
            }
        }

        private void Searchbar_Click(object sender, EventArgs e)
        {
            TakeFocus();
        }

        public void HandleResultsClose(object sender, EventArgs e)
        {
            HandleEscapeHotkey();
        }

        private void HandleEscapeHotkey()
        {
            if (searchResults is not null)
            {
                searchResults.FormClosed -= HandleResultsClose;
                searchResults.Close();
                searchResults.Dispose();
                searchResults = null;
            }

            LoseFocus();
        }

        #endregion

        private bool HotkeySetCallback(in HashSet<Keys> hotkey)
        {
            Dictionary<string, int> newKeybind = new() { { "keys", 0 }, { "mods", 0 } };

            foreach (Keys k in hotkey)
            {
                // Is a modifier, needs to be converted
                if (k == Keys.ShiftKey || k == Keys.ControlKey || k == Keys.Menu || k == Keys.LWin || k == Keys.RWin)
                {
                    newKeybind["mods"] |= GlobalHotkeys.ModifierKeysToGlobalHotkeys.Convert(k);
                }
                else
                {
                    newKeybind["keys"] |= (int)k;
                }
            }

            if (newKeybind["keys"] == 0)
            {
                // No keys were pressed
                return false;
            }

            showHotkey.Unregister();
            showHotkey.Dispose();

            try
            {
                showHotkey = new GlobalHotkeys.GlobalHotkeys((GlobalHotkeys.Modifiers)newKeybind["mods"], (Keys)newKeybind["keys"], this, true);
            }
            catch (GlobalHotkeys.GlobalHotkeysException)
            {
                showHotkey = new GlobalHotkeys.GlobalHotkeys((GlobalHotkeys.Modifiers)keybind["mods"], (Keys)keybind["keys"], this, true);
                return false;
            }

            Properties.Settings.Default.KeybindKeys = newKeybind["keys"];
            Properties.Settings.Default.KeybindMods = newKeybind["mods"];
            Properties.Settings.Default.Save();

            keybind = newKeybind;

            return true;
        }

        protected override void WndProc(ref Message m)
        {
            // This gets called when any global hotkey is pressed, so we need to filter the messages to ensure we're the handler
            if (m.Msg == GlobalHotkeys.Constants.WM_HOTKEY_MSG_ID)
            {
                // Bitmask magic to get key codes
                int keyCode = ((int)m.LParam >> 16) & 0xFFFF;
                int modifierKeys = (int)m.LParam & 0xFFFF;

                if (keyCode == keybind["keys"] && modifierKeys == keybind["mods"])  // LWin and RWin are treated as the same
                {
                    if (TopMost)
                    {
                        LoseFocus();
                    }
                    else
                    {
                        TakeFocus();
                    }
                }
            }

            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                HandleEscapeHotkey();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SearchInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HandleEscapeHotkey();
                return;
            }

            // Users can use * rather than . if they really want to...
            if (searchInputIgnoreKeys.Contains(e.KeyCode) || SearchInput.Text == ".")
            {
                return;
            }

            if (SearchInput.Text != "")
            {
                if (searchResults is null || searchResults.IsDisposed)
                {
                    searchResults = new SearchResults();
                    searchResults.FormClosed += HandleResultsClose;
                    searchResults.Show();

                    // Receive keypresses from results file list
                    searchResults.KeypressPassthrough += new SearchResults.PassthroughKeypressEvent((KeyEventArgs ptKE) =>
                    {
                        if (ptKE.KeyCode == Keys.Escape)
                        {
                            HandleEscapeHotkey();
                        }
                    });

                    TopMost = true;
                    Activate();
                }

                if (resultListUpdateCancelHandler is not null)
                {
                    resultListUpdateCancelHandler.Cancel();
                }

                Task pauseForCancel = Task.Run(() => Task.Delay(20));
                pauseForCancel.Wait();

                resultListUpdateCancelHandler = new CancellationTokenSource();
                searchEngine.cancelationHandler = resultListUpdateCancelHandler;
                searchEngine.DoSearch(SearchInput.Text, SearchEngineCallback);
            }

            else
            {
                if (searchResults is not null && searchResults.HasResults)
                {
                    searchEngine.ClearSearch();
                }
            }
        }

        public void SearchEngineCallback(ref SearchResult[] results)
        {
            searchResults.DoUpdate(ref results);
            searchResults.currentSearchQuery = SearchInput.Text;

            Console.WriteLine("Updated results successfully");
        }
    }
}

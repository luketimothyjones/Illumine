using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace Illumine
{
    public partial class SearchResults : Form
    {
        public string currentSearchQuery;
        public bool HasResults
        {
            get { return ResultsFileList.Items.Count > 0; }
        }

        public delegate void PassthroughKeypressEvent(KeyEventArgs e);
        public event PassthroughKeypressEvent KeypressPassthrough;

        private Backdrop backdrop = null;

        public SearchResults()
        {
            InitializeComponent();

            ShowOnScreen(Properties.Settings.Default.DefaultMonitor);

            ResultsFileList.Columns.Add(new ColumnHeader() { Text = "File name", Name = "FileName" });
            ResultsFileList.Columns.Add(new ColumnHeader() { Text = "File path", Name = "FilePath" });
        }

        public void ShowOnScreen(int screenIndex)
        {
            Screen displayScreen = Screen.AllScreens[Math.Min(Screen.AllScreens.Length - 1, screenIndex)];

            Location = new Point(displayScreen.Bounds.Location.X, displayScreen.Bounds.Location.Y);
            Width = displayScreen.Bounds.Width;
            Height = displayScreen.Bounds.Height;
            WindowState = FormWindowState.Maximized;

            ResultsFileList.Left = (displayScreen.Bounds.Width / 2) - (ResultsFileList.Width / 2);
            ResultsFileList.Top = 450;

            // Make sure ResultsFileList does not overrun bottom of screen
            if (ResultsFileList.Top + ResultsFileList.Height > displayScreen.Bounds.Height)
            {
                ResultsFileList.Height -= (ResultsFileList.Top + ResultsFileList.Height) - displayScreen.Bounds.Height + 10;
            }
        }

        public void DoUpdate(ref SearchResult[] results)
        {
            ResultsFileList.BeginUpdate();

            ResultsFileList.Items.Clear();
            foreach (SearchResult result in results)
            {
                if (result is null)
                {
                    break;
                }

                ResultsFileList.Items.Add(new ListViewItem(new string[] { result.fileName, result.filePath }));
            }

            // We're done with the results array, so it can be cleared
            Array.Clear(results, 0, results.Length);

            // Autosize the first column, limit it to 400px, then fill the rest of the view with the second column
            ResultsFileList.Columns[0].Width = -2;
            ResultsFileList.Columns[0].Width = Math.Min(ResultsFileList.Columns[0].Width + 15, 400);
            ResultsFileList.Columns[1].Width = ResultsFileList.Width - ResultsFileList.Columns[0].Width - SystemInformation.VerticalScrollBarWidth;

            ResultsFileList.EndUpdate();
        }

        private void ResultsFileList_KeyDown(object sender, KeyEventArgs e)
        {
            // The file list eats escape keypresses when it has focus; this passses the event through
            KeypressPassthrough?.Invoke(e);
            base.OnKeyDown(e);
        }

        private void SearchResults_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                backdrop = new Backdrop();
                backdrop.Show();
            }
        }

        private void SearchResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backdrop != null)
            {
                backdrop.Close();
                backdrop = null;
            }

            ResultsFileList.Items.Clear();
        }

        private void ResultsFileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ResultsFileList.SelectedIndices.Count == 0)
            {
                return;
            }

            // Join the path and filename together
            ListViewSubItemCollection item = ResultsFileList.SelectedItems[0].SubItems;
            Utils.ShellUtils.OpenPathWithDefaultApp(item[1].Text.TrimEnd('\\') + "\\" + item[0].Text);

            Close();
        }
    }
}

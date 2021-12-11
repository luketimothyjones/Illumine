using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Illumine
{
    public partial class SearchResults : Form
    {
        public List<SearchResult> results;
        public string currentSearchQuery;

        public delegate void PassthroughKeypressEvent(KeyEventArgs e);
        public event PassthroughKeypressEvent KeypressPassthrough;

        private Backdrop backdrop = null;

        public SearchResults()
        {
            InitializeComponent();

            ShowOnScreen(Properties.Settings.Default.DefaultMonitor);

            results = new List<SearchResult>();

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

        public void DoUpdate()
        {
            ResultsFileList.BeginUpdate();

            ResultsFileList.Items.Clear();
            foreach (SearchResult result in results)
            {
                ResultsFileList.Items.Add(new ListViewItem(new string[] { result.fileName, result.filePath }));
            }

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

        private void SearchResults_VisibleChanged(object sender, System.EventArgs e)
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
        }

        private void ResultsFileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ResultsFileList.SelectedIndices.Count == 0)
            {
                return;
            }

            ListViewItem parts = ResultsFileList.SelectedItems[0];
            string fname = parts.SubItems[0].Text;
            string path = parts.SubItems[1].Text.TrimEnd('\\');
            string fullPath = path + '\\' + fname;

            Utils.ShellUtils.OpenPathWithDefaultApp(fullPath);

            Close();
        }
    }
}

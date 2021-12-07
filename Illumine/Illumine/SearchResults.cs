using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Illumine
{
    public partial class SearchResults : Form
    {
        public BindingList<string> results;
        public string currentSearchQuery;

        public delegate void PassthroughKeypressEvent(KeyEventArgs e);
        public event PassthroughKeypressEvent KeypressPassthrough;

        private Backdrop backdrop = null;

        public SearchResults()
        {
            InitializeComponent();

            results = new BindingList<string>();
            ResultsFileList.DataSource = results;

            // TODO :: Calculate center here
            ResultsFileList.Top = 400;
            ResultsFileList.Left = 300;
        }

        public void PauseResultsListUpdates() {
            ResultsFileList.BeginUpdate();
        }

        public void ResumeResultsListUpdates() {
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

        private void SearchResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Handle drawing of items in the results list, including highlighting of search query

            // https://i.imgur.com/O72M3Mu.png ???

            ListBox resultsListBox = (ListBox)sender;
            this.Invoke((MethodInvoker)delegate
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                if (e.Index < 0)
                {
                    // e.Index returns -1 if an item is removed from the list
                    return;
                }

                string resultLineString = resultsListBox.GetItemText(resultsListBox.Items[e.Index]);

                if (!string.IsNullOrEmpty(currentSearchQuery))
                {
                    string queryMatch = Regex.Match(resultLineString, Regex.Escape(currentSearchQuery), RegexOptions.IgnoreCase).Value;
                    string[] resultParts = Regex.Split(resultLineString, Regex.Escape(queryMatch));

                    Rectangle rect = e.Bounds;
                    int width, width2;

                    for (int i = 0; i < resultParts.Length; i++)
                    {
                        string part = resultParts[i];
                        if (part != "")
                        {
                            width = (int)e.Graphics.MeasureString(part, e.Font, e.Bounds.Width, StringFormat.GenericTypographic).Width;
                            rect.Width = width;
                            TextRenderer.DrawText(e.Graphics, part, e.Font, new Point(rect.X, rect.Y), resultsListBox.ForeColor);
                            rect.X += width;
                        }

                        if (i < resultParts.Length - 1)
                        {
                            width2 = (int)e.Graphics.MeasureString(queryMatch, e.Font, e.Bounds.Width, StringFormat.GenericTypographic).Width;
                            rect.Width = width2;
                            TextRenderer.DrawText(e.Graphics, queryMatch, e.Font, new Point(rect.X, rect.Y), Color.Black, Color.White);
                            rect.X += width2;
                        }
                    }
                }
                else
                {
                    TextRenderer.DrawText(e.Graphics, resultLineString, e.Font, new Point(e.Bounds.X, e.Bounds.Y), resultsListBox.ForeColor);
                }
            });
        }

        private void ResultsFileList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ResultsFileList.SelectedIndex < 0)
            {
                return;
            }

            string[] parts = ResultsFileList.SelectedItem.ToString().Split("  ||  ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string fname = parts[0];
            string path = parts[1].TrimEnd('\\');
            string fullPath = path + '\\' + fname;

            Utils.ShellUtils.OpenPathWithDefaultApp(fullPath);

            Close();
        }
    }
}

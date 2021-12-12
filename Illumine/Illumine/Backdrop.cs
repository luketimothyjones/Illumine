using System;
using System.Drawing;
using System.Windows.Forms;

namespace Illumine
{
    public partial class Backdrop : Form
    {
        public Backdrop()
        {
            InitializeComponent();

            ShowOnScreen(Properties.Settings.Default.DefaultMonitor);
        }

        public void ShowOnScreen(int screenIndex)
        {
            Screen displayScreen = Screen.AllScreens[Math.Min(Screen.AllScreens.Length - 1, screenIndex)];

            Width = displayScreen.Bounds.Width;
            Height = displayScreen.Bounds.Height;
            Location = new Point(displayScreen.Bounds.Location.X, displayScreen.Bounds.Location.Y);
            WindowState = FormWindowState.Maximized;
        }

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

        protected override void WndProc(ref Message m)
        {
            // Prevent backdrop from taking focus on click
            if (m.Msg == (int)0x84)
            {
                m.Result = (IntPtr)(-1);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace Illumine
{
    public partial class Backdrop : Form
    {
        public Backdrop()
        {
            InitializeComponent();
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
    }
}

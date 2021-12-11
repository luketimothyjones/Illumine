using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Illumine
{
    public partial class MonitorSetter : Form
    {
        public delegate void MonitorSetterCallback(int screenIndex);
        public MonitorSetterCallback callbacks;

        public MonitorSetter()
        {
            InitializeComponent();
        }

        private void MonitorSetter_Shown(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void SetMonitorButton_Click(object sender, EventArgs e)
        {
            Screen currentScreen = Screen.FromControl(this);
            int screenIndex = 0;
            foreach (Screen s in Screen.AllScreens)
            {
                if (s.Bounds == currentScreen.Bounds)
                {
                    break;
                }
                screenIndex++;
            }

            if (screenIndex <= Screen.AllScreens.Length - 1)
            {
                callbacks.Invoke(screenIndex);
            }

            Close();
        }

        private void CancelSetButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

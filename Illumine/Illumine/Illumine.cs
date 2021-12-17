using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Illumine
{
    static class Illumine
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName("Illumine").Length > 1)
            {
                // Exit if there is already an Illumine instance running
                Environment.Exit(-1);
                return;
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Searchbar());
        }
    }
}

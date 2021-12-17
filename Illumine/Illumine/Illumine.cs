using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

            // Run updater -- this will kill the process if the user chooses to update
            UpdateManager.UpdateManager updateManager = new();
            Task<bool> updateTask = Task.Run(updateManager.DoUpdate);
            updateTask.Wait();

            if (!updateTask.Result)
            {
                // If there isn't an update, run as normal
                Application.Run(new Searchbar());
            }
        }
    }
}

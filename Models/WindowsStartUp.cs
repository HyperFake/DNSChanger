using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DNS_changer.Models
{
    class WindowsStartUp
    {
        bool StartAtStartUp = false;

        // The path to the key where Windows looks for startup applications
        private readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public WindowsStartUp()
        {

        }

        /// <summary>
        /// Checks registry for DNSChanger value to see if it's enabled
        /// </summary>
        public void CheckRegistryState()
        {
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("DNSChanger") != null)
                StartAtStartUp = true;
            else
                StartAtStartUp = false;
        }

        /// <summary>
        /// Adds DNSChanger program to windows program start up
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void AddStartUpToRegistry(object sender, EventArgs e)
        {
            if (StartAtStartUp)
                rkApp.SetValue("DNSChanger", Application.ExecutablePath);
            else
                rkApp.DeleteValue("DNSChanger", false);
        }
    }
}

using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DNS_changer.Helper
{
    class WindowsStartUpHelper
    {
        // The path to the key where Windows looks for startup applications
        private readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public WindowsStartUpHelper()
        {

        }

        /// <summary>
        /// Checks registry for DNSChanger value to see if it's enabled
        /// </summary>
        public bool CheckRegistryState()
        {
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("DNSChanger") != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Adds DNSChanger program to windows program start up
        /// </summary>

        public void AddStartUpToRegistry()
        {
            rkApp.SetValue("DNSChanger", Application.ExecutablePath);

        }

        public void RemoveStartUpFromRegistry()
        {
            rkApp.DeleteValue("DNSChanger", false);
        }
    }
}

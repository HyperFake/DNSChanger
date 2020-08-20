using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DNS_changer.Helper
{
    public static class WindowsStartUpHelper
    {
        // The path to the key where Windows looks for startup applications
        private static readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


        /// <summary>
        /// Checks registry for DNSChanger value to see if it's enabled
        /// </summary>
        public static bool CheckRegistryState()
        {
            // Check to see the current state (running at startup or not)
            return rkApp.GetValue("DNSChanger") != null;
        }

        /// <summary>
        /// Adds DNSChanger program to windows program start up
        /// </summary>

        public static void AddStartUpToRegistry()
        {
            rkApp.SetValue("DNSChanger", Application.ExecutablePath);

        }

        public static void RemoveStartUpFromRegistry()
        {
            rkApp.DeleteValue("DNSChanger", false);
        }
    }
}

using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DNS_changer.Helper
{
    public static class WindowsStartUpHelper
    {
        // The path to the key where Windows looks for startup applications
        private static readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Checks registry for DNSChanger value to see if it's enabled
        /// </summary>
        public static bool CheckRegistryState()
        {
            bool regState = false;
            try
            {
                regState = rkApp.GetValue("DNSChanger") != null;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to check registry for DNS changer");
            }

            return regState;
        }

        /// <summary>
        /// Adds DNSChanger to start up registry list
        /// </summary>

        public static void AddStartUpToRegistry()
        {
            try
            {
                rkApp.SetValue("DNSChanger", Application.ExecutablePath);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to add DNS changer to registry");
            }
        }

        /// <summary>
        /// Removes DNS changer from start up registry list
        /// </summary>
        public static void RemoveStartUpFromRegistry()
        {
            try
            {
                rkApp.DeleteValue("DNSChanger", false);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to delete DNS changer from registry");
            }
        }
    }
}

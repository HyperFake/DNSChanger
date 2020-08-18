using Caliburn.Micro;
using DNS_changer.Helper;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using System;

namespace DNS_changer.ViewModels.Main
{
    public class MainViewModel : Screen
    {
        // System tray
        private TrayManager trayManager;

        // Logging
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MainViewModel(TrayManager Manager)
        {
            trayManager = Manager;
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Sets local DNS to Google DNS
        /// </summary>
        public void GoogleDNS()
        {
            SetDNS(DNSInformation.GoogleDNS);
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Sets local DNS to Cloudflare DNS
        /// </summary>
        public void CloudflareDNS()
        {
            SetDNS(DNSInformation.CloudflareDNS);
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Resets local DNS
        /// </summary>
        public void ResetDNS()
        {
            ResetAllDNS();
            CheckIfDNSChanged();
        }


        /// <summary>
        /// Set DNS
        /// </summary>
        /// <param name="DNSInfo">DNS string. Main and alternative</param>
        private static void SetDNS(string[] DNSInfo)
        {
            try
            {
                NetworkInterface CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
                if (CurrentInterface == null) return;

                ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection objMOC = objMC.GetInstances();
                foreach (ManagementObject objMO in objMOC)
                {
                    if ((bool)objMO["IPEnabled"])
                    {
                        if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                        {
                            ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                            if (objdns != null)
                            {
                                objdns["DNSServerSearchOrder"] = DNSInfo;
                                objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to set DNS");
            }
        }

        /// <summary>
        /// Unsets Main and Alternative local DNS
        /// </summary>
        private static void ResetAllDNS()
        {
            try
            {
                NetworkInterface CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
                if (CurrentInterface == null) return;

                ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection objMOC = objMC.GetInstances();
                foreach (ManagementObject objMO in objMOC)
                {
                    if ((bool)objMO["IPEnabled"])
                    {
                        if (objMO["Description"].ToString().Equals(CurrentInterface.Description))
                        {
                            ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                            if (objdns != null)
                            {
                                objdns["DNSServerSearchOrder"] = null;
                                objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to reset DNS");
            }

        }

        /// <summary>
        /// Gets Current DNS, Main and Alternative addresses
        /// </summary>
        public string GetCurrentDNS()
        {
            try
            {
                NetworkInterface CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
                if (CurrentInterface == null) return "";

                IPInterfaceProperties IPProperties = CurrentInterface.GetIPProperties();
                IPAddressCollection IPCollection = IPProperties.DnsAddresses;

                StringBuilder DNSstring = new StringBuilder();
                foreach (IPAddress info in IPCollection)
                {
                    DNSstring.Append($"{info } ");
                }
                CurrentDNS = DNSstring.ToString();


                return IPCollection[0].ToString();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to get current DNS");
            }

            return "";
        }

        /// <summary>
        /// Checks if DNS is changed and change System tray icon accordingly
        /// </summary>
        private void CheckIfDNSChanged()
        {
            try
            {
                string currentDNS = GetCurrentDNS();

                if (currentDNS == DNSInformation.GoogleDNS[0] || currentDNS == DNSInformation.CloudflareDNS[0])
                    trayManager.ActivateTray();
                else
                    trayManager.DeactivateTray();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to check if DNS is changed");
            }
        }

        /// <summary>
        /// Gets active Network
        /// </summary>
        /// <returns>Active Network</returns>
        private static NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
        {
            try
            {
                NetworkInterface currentNetwork = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                a => a.OperationalStatus == OperationalStatus.Up &&
                (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                a.GetIPProperties().GatewayAddresses.Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

                return currentNetwork;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to get current active network");
            }
            return null;
        }

        private string _currentDNS;
        public string CurrentDNS
        {
            get
            {
                return _currentDNS;
            }
            set
            {
                _currentDNS = value;
                NotifyOfPropertyChange(() => CurrentDNS);
            }
        }
    }
}

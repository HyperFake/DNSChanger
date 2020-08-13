using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DNS_changer.ViewModels
{
    public class MainViewModel : Screen
    {

        public MainViewModel()
        {
        }

        /// <summary>
        /// Sets local DNS to Google DNS
        /// </summary>
        public void GoogleDNS()
        {
            SetDNS(DNSInformation.GoogleDNS);
        }

        /// <summary>
        /// Sets local DNS to Cloudflare DNS
        /// </summary>
        public void CloudflareDNS()
        {
            SetDNS(DNSInformation.CloudflareDNS);
        }

        /// <summary>
        /// Resets local DNS
        /// </summary>
        public void ResetDNS()
        {
            UnsetDNS();
        }


        /// <summary>
        /// Set DNS
        /// </summary>
        /// <param name="DNSInfo">DNS string. Main and alternative</param>
        private static void SetDNS(string[] DNSInfo)
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

        /// <summary>
        /// Unsets Main and Alternative local DNS
        /// </summary>
        private static void UnsetDNS()
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

        /// <summary>
        /// Gets Current DNS, Main and Alternative addresses
        /// </summary>
        public void GetCurrentDNS()
        {
            NetworkInterface CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
            if (CurrentInterface == null) return;

            IPInterfaceProperties IPProperties = CurrentInterface.GetIPProperties();
            IPAddressCollection IPCollection = IPProperties.DnsAddresses;
            StringBuilder DNSstring = new StringBuilder();
            foreach(IPAddress info in IPCollection)
            {
                DNSstring.Append($"{info } ");
            }
            

            CurrentDNS = DNSstring.ToString();
        }

        /// <summary>
        /// Gets active Network
        /// </summary>
        /// <returns>Active Network</returns>
        private static NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
        {
            NetworkInterface Nic = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                a => a.OperationalStatus == OperationalStatus.Up &&
                (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                a.GetIPProperties().GatewayAddresses.Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

            return Nic;
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

using Caliburn.Micro;
using DNS_changer.Helper;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Main
{
    public class MainViewModel : Screen
    {
        // System tray
        private readonly TrayManager trayManager;

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MainViewModel(TrayManager Manager)
        {
            trayManager = Manager;
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Sets local DNS to Google DNS
        /// </summary>
        public async void GoogleDNS()
        {
            await SetDNSAsync(DNSInformation.GoogleDNS);
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Sets local DNS to Cloudflare DNS
        /// </summary>
        public async void CloudflareDNS()
        {
            await SetDNSAsync(DNSInformation.CloudflareDNS);
            CheckIfDNSChanged();
        }

        /// <summary>
        /// Resets local DNS
        /// </summary>
        public async void ResetDNS()
        {
            await ResetAllDNSAsync();
            CheckIfDNSChanged();
        }


        /// <summary>
        /// Set DNS
        /// </summary>
        /// <param name="DNSInfo">DNS string. Main and alternative</param>
        private async Task SetDNSAsync(string[] DNSInfo)
        {
            await Task.Run(() =>
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
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to set DNS");
                }
            });

        }

        /// <summary>
        /// Unsets Main and Alternative local DNS
        /// </summary>
        private async Task ResetAllDNSAsync()
        {
            await Task.Run(() =>
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
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to reset DNS");
                }
            });

        }

        /// <summary>
        /// Gets Current DNS, Main and Alternative addresses
        /// </summary>
        public async Task<string> GetCurrentDNSAsync()
        {
            string returnString = "";
            await Task.Run(() =>
            {
                try
                {
                    NetworkInterface CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
                    if (CurrentInterface == null) return returnString;

                    IPInterfaceProperties IPProperties = CurrentInterface.GetIPProperties();
                    IPAddressCollection IPCollection = IPProperties.DnsAddresses;

                    StringBuilder DNSstring = new StringBuilder();
                    foreach (IPAddress info in IPCollection)
                    {
                        DNSstring.Append($"{info } ");
                    }
                    CurrentDNS = DNSstring.ToString();


                    returnString = IPCollection[0].ToString();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to get current DNS");
                }
                return returnString;
            });
            return returnString;
        }

        /// <summary>
        /// Checks if DNS is changed and change System tray icon accordingly
        /// </summary>
        private async void CheckIfDNSChanged()
        {
            try
            {
                string currentDNS = await GetCurrentDNSAsync();

                if (currentDNS == DNSInformation.GoogleDNS[0] || currentDNS == DNSInformation.CloudflareDNS[0])
                {
                    CurrentImageString = ImageStrings.DNSEnabledPC;
                    trayManager.ActivateTray();
                    SuggestionColor = Brushes.Green;
                    SuggestionText = LanguageHelper.SavedValue("DNSChangedText");
                }
                else
                {
                    CurrentImageString = ImageStrings.DNSDisabledPC;
                    trayManager.DeactivateTray();
                    SuggestionColor = Brushes.Red;
                    SuggestionText = LanguageHelper.SavedValue("DNSNotChangedText");
                }
            }
            catch (Exception ex)
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

        private string _currentImageString;

        public string CurrentImageString
        {
            get { return _currentImageString; }
            set
            {
                _currentImageString = value;
                NotifyOfPropertyChange(() => CurrentImageString);
            }
        }

        private string _suggestionText;
        public string SuggestionText
        {
            get { return _suggestionText; }
            set
            {
                _suggestionText = value;
                NotifyOfPropertyChange(() => SuggestionText);
            }
        }

        private Brush _suggestionColor;
        public Brush SuggestionColor
        {
            get { return _suggestionColor; }
            set
            {
                _suggestionColor = value;
                NotifyOfPropertyChange(()=> SuggestionColor);
            }
        }
    }
}

using DNS_changer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DNS_changer.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        TrayManager trayManager = new TrayManager();
        public ShellView()
        {
            InitializeComponent();
            AttachEventOnDoubleClickIcon();
        }

        /// <summary>
        /// Hides Window on closing instead of exiting
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// Attaches ShowMainWindow function to icon doubleclick
        /// </summary>
        public void AttachEventOnDoubleClickIcon()
        {
            NotifyIcon icon = trayManager.NotifyIcon;
            icon.DoubleClick += ShowMainWindow;
        }

        /// <summary>
        /// Makes Main Window visible
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ShowMainWindow(object sender, EventArgs e)
        {
            Visibility = Visibility.Visible;
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
            foreach (IPAddress info in IPCollection)
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

        private string _currentDNS = "";

        
    }
}

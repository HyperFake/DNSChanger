using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using DNS_changer.Properties;
using System.ComponentModel;
using DNS_changer.Models;

namespace DNS_changer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static Settings defaultInstance = ((Settings)(SettingsBase.Synchronized(new Settings())));

        public App()
        {
        }
    }
}

using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNS_changer.ViewModels
{
    public class SettingsViewModel : Caliburn.Micro.Screen
    {
        // System tray 
        private TrayManager trayManager;

        public SettingsViewModel(TrayManager Manager)
        {
            trayManager = Manager;
        }

      
    }
}

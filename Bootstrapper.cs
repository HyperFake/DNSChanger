using Caliburn.Micro;
using DNS_changer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DNS_changer
{
    public class Bootstrapper : BootstrapperBase
    {
        
        public Bootstrapper()
        {
            // Gets current process name
            String DNSChangerName = Process.GetCurrentProcess().ProcessName;

            // Prevent opening more than 1 window
            if (Process.GetProcesses().Count(p => p.ProcessName == DNSChangerName) > 1)
                return;

            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}

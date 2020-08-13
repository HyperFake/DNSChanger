using Caliburn.Micro;
using DNS_changer.Models;
using DNS_changer.ViewModels;
using DNS_changer.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

            // Prevents opening more than 1 application
            if (Process.GetProcesses().Count(p => p.ProcessName == DNSChangerName) > 1)
                return;

            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //    DisplayRootViewFor<ShellViewModel>();

            IWindowManager windowManager = IoC.Get<IWindowManager>();

            LoginViewModel loginViewModel = IoC.Get<LoginViewModel>();
            windowManager.ShowDialog(loginViewModel, null, null);


        }
        private void ChangeView(object sender, EventArgs e)
        {
            
        }
    }
}

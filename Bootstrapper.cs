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
            IWindowManager windowManager = IoC.Get<IWindowManager>();

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.Password))
            {
                RegisterViewModel registerViewModel = IoC.Get<RegisterViewModel>();
                registerViewModel.OnRegisterEvent += (o, s) =>
                {
                    RegisterSuccess(registerViewModel);
                };
                // Show the login view
                windowManager.ShowDialog(registerViewModel, null, null);
            }
            else
            {
                LoginViewModel loginViewModel = IoC.Get<LoginViewModel>();
                // Subscribe new event to change view on successful login
                loginViewModel.OnLoginEvent += (o, s) =>
                {
                    LoginSuccess(loginViewModel);
                };
                // Show the login view
                windowManager.ShowDialog(loginViewModel, null, null);
            }
        }

        // Terminates login screen and shows Shell
        private void LoginSuccess(LoginViewModel loginViewModel)
        {
            IWindowManager windowManager = IoC.Get<IWindowManager>();
            ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();

            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            loginViewModel.TryClose();

            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            windowManager.ShowDialog(shellViewModel, null, null);
        }
        private void RegisterSuccess(RegisterViewModel registerViewModel)
        {
            IWindowManager windowManager = IoC.Get<IWindowManager>();
            ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();

            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            registerViewModel.TryClose();

            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            windowManager.ShowDialog(shellViewModel, null, null);
        }

    }
}

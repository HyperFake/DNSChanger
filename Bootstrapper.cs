using Caliburn.Micro;
using DNS_changer.Helper;
using DNS_changer.ViewModels.Login;
using DNS_changer.ViewModels.Register;
using DNS_changer.ViewModels.Shell;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace DNS_changer
{
    public class Bootstrapper : BootstrapperBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Bootstrapper()
        {
            // Gets current process name
            string DNSChangerName = Process.GetCurrentProcess().ProcessName;

            // Prevents opening more than 1 application
            if (Process.GetProcesses().Count(p => p.ProcessName == DNSChangerName) > 1)
                return;

            // Sets default app language
            SetDefaultLanguage();

            // Sets logging config
            Logging.SetConfig();

            Initialize();
        }

        /// <summary>
        /// Sets default language if there isn't one, otherwise sets the saved language
        /// </summary>
        private void SetDefaultLanguage()
        {

            if (string.IsNullOrWhiteSpace(LanguageHelper.GetLanguage()))
            {
                LanguageHelper.SetLanguage("en-US");
                return;
            }

            LanguageHelper.SetLanguage(LanguageHelper.GetLanguage());
        }

        /// <summary>
        /// Overrides OnStartup event. Activates Login or Register windows accordingly
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">StartupEventArgs</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            IWindowManager windowManager = IoC.Get<IWindowManager>();

            // If password is null, activate Register window
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.Password))
            {
                RegisterViewModel registerViewModel = IoC.Get<RegisterViewModel>();
                // Attach event, which will change the window
                registerViewModel.OnRegisterEvent += (o, s) =>
                {
                    RegisterSuccess(registerViewModel);
                };
                // Show the register view
                windowManager.ShowDialog(registerViewModel, null, null);
            }
            else
            {
                LoginViewModel loginViewModel = IoC.Get<LoginViewModel>();
                loginViewModel.OnLoginEvent += (o, s) =>
                {
                    LoginSuccess(loginViewModel);
                };
                // Show the login view
                windowManager.ShowDialog(loginViewModel, null, null);
            }
        }

        /// <summary>
        /// Login success method, which closes Login window and opens Shell window
        /// </summary>
        /// <param name="loginViewModel">LoginViewModel</param>
        private void LoginSuccess(LoginViewModel loginViewModel)
        {
            try
            {
                IWindowManager windowManager = IoC.Get<IWindowManager>();
                ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();

                Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                loginViewModel.TryClose();

                Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

                windowManager.ShowDialog(shellViewModel, null, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to close Login window");
            }

        }
        /// <summary>
        /// Register success method, which closes Register window and opens Shell window
        /// </summary>
        /// <param name="registerViewModel">RegisterViewModel</param>
        private void RegisterSuccess(RegisterViewModel registerViewModel)
        {
            try
            {
                IWindowManager windowManager = IoC.Get<IWindowManager>();
                ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();

                Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                registerViewModel.TryClose();

                Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

                windowManager.ShowDialog(shellViewModel, null, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to close Register window");
            }

        }

    }
}

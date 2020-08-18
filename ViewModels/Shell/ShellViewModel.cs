using Caliburn.Micro;
using DNS_changer.Helper;
using DNS_changer.ViewModels.Main;
using DNS_changer.ViewModels.Settings;
using System;
using System.ComponentModel;
using System.Windows;

namespace DNS_changer.ViewModels.Shell
{
    public class ShellViewModel : Conductor<object>
    {
        // System tray
        static TrayManager trayManager = new TrayManager();

        // Language Helper for multi languages support
        LanguageHelper lgHelper = new LanguageHelper();

        // Views
        MainViewModel mainView = new MainViewModel(trayManager);
        SettingsViewModel settingsView = new SettingsViewModel(trayManager);

        // Logging
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ShellViewModel()
        {
            // On startup load main view
            ActivateItem(mainView);

            // System Tray functions
            AttachShowWindow();
            AddMainWindowButton();
            settingsView.AddWindowsStartButton();
            settingsView.AddLanguageButton();
            AddExitAppButton();

        }

        /// <summary>
        /// Loads MainView into Content control
        /// </summary>
        public void LoadMainView()
        {
            ActivateItem(mainView);
        }

        /// <summary>
        /// Loads SettingsView into Content control
        /// </summary>
        public void LoadSettingsView()
        {
            ActivateItem(settingsView);
        }

        /// <summary>
        /// Exits Application
        /// </summary>
        public void ExitApp()
        {
            try
            {
                trayManager.Dispose();
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to exit the app");
            }
            return;
        }

        /// <summary>
        /// Exit app function for system tray
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void ExitAppSystemTray(object sender, EventArgs e)
        {
            ExitApp();
        }

        /// <summary>
        /// Overrides OnClose to just hide
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        public void OnClose(CancelEventArgs e)
        {
            MainWindowVisibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void AddExitAppButton()
        {
            trayManager.AddItemToContextStripMenu(lgHelper.SavedValue("ExitButton"), null, ExitAppSystemTray);
        }

        /// <summary>
        /// Adds Main window button to ContextStripMenu
        /// </summary>
        private void AddMainWindowButton()
        {
            trayManager.AddItemToContextStripMenu(lgHelper.SavedValue("MainButton"), null, ShowMainWindow);
        }

        /// <summary>
        /// Attaches function to show Main window on doubleclick
        /// </summary>
        private void AttachShowWindow()
        {
                trayManager.NotifyIcon.DoubleClick += ShowMainWindow;
        }

        /// <summary>
        /// Makes Main Window visible
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void ShowMainWindow(object sender, EventArgs e)
        {
            MainWindowVisibility = Visibility.Visible;
        }


        private Visibility _mainWindowVisibility;
        public Visibility MainWindowVisibility
        {
            get { return _mainWindowVisibility; }
            set
            {
                _mainWindowVisibility = value;
                NotifyOfPropertyChange(() => MainWindowVisibility);
            }
        }
    }
}

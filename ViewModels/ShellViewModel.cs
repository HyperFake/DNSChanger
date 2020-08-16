using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace DNS_changer.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        // System tray
        static TrayManager trayManager = new TrayManager();

        // Views
        MainViewModel mainView = new MainViewModel(trayManager);
        SettingsViewModel settingsView = new SettingsViewModel(trayManager);

        public ShellViewModel()
        {
            // On startup load main view
            ActivateItem(mainView);
            AttachEventOnDoubleClickIcon();
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
                Console.WriteLine($"{ex}");
            }
            return;
        }

        /// <summary>
        /// Overrides OnClose to just hide
        /// </summary>
        /// <param name="e">e</param>
        public void OnClose(CancelEventArgs e)
        {
            MainWindowVisibility = Visibility.Hidden;
            e.Cancel = true;
        }

        /// <summary>
        /// Attaches function to show Main window on doubleclick
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

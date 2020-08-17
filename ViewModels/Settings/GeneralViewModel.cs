using DNS_changer.Helper;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace DNS_changer.ViewModels.Settings
{
    public class GeneralViewModel : Caliburn.Micro.Screen
    {
        // Start up helper
        WindowsStartUpHelper startUpHelper = new WindowsStartUpHelper();
        // Language helper
        LanguageHelper lgHelper = new LanguageHelper();

        // Start Up item
        ToolStripMenuItem WindowsStartUp = new ToolStripMenuItem();

        /// <summary>
        /// GeneralViewModel Constructor, checks if DNS changer is enabled at start
        /// </summary>
        public GeneralViewModel()
        {
            StartUpEnabled = startUpHelper.CheckRegistryState();
        }

        public void AddChangeLanguageToTray()
        {
            ToolStripMenuItem ChangeLanguage = new ToolStripMenuItem();
            ChangeLanguage.DropDownItems.Add(lgHelper.ReturnValue("LanguageEnglishButton"), null, AddEnglishSystemTray);
            ChangeLanguage.DropDownItems.Add(lgHelper.ReturnValue("LanguageLithuanianButton"), null, AddLithuanianSystemTray);

            ChangeLanguage.Text = lgHelper.ReturnValue("ChangeLanguage");

            TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(ChangeLanguage);
        }

        private void AddEnglishSystemTray(object sender, EventArgs e)
        {
            lgHelper.SetLanguage("en-US");
        }

        private void AddLithuanianSystemTray(object sender, EventArgs e)
        {
            lgHelper.SetLanguage("lt-LT");
        }

        /// <summary>
        /// Adds Windows Start Up button to System tray
        /// </summary>
        public void AddToggleWindowsStartToTray()
        {
            // Setting up System tray item
            WindowsStartUp.Text = lgHelper.ReturnValue("WindowsButton");
            WindowsStartUp.MouseDown += ToggleStartUpSystemTray;

            if (StartUpEnabled)
                WindowsStartUp.Checked = true;
            else
                WindowsStartUp.Checked = false;

            // Adding it
            TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(WindowsStartUp);
        }


        /// <summary>
        /// Checks if we need to add or remove DNS changer from registry, sets status accordingly
        /// </summary>
        private void ToggleStartUp()
        {
            if(StartUpEnabled)
            {
                startUpHelper.RemoveStartUpFromRegistry();
                StartUpEnabled = false;
                WindowsStartUp.Checked = false;
            }
            else
            {
                startUpHelper.AddStartUpToRegistry();
                StartUpEnabled = true;
                WindowsStartUp.Checked = true;
            }
        }

        /// <summary>
        /// Event for System tray
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ToggleStartUpSystemTray(object sender, EventArgs e)
        {
            ToggleStartUp();
        }


        // Tray Manager, for handling system tray functions, additions
        private TrayManager _trayManager;
        public TrayManager TrayManager
        {
            get { return _trayManager; }
            set
            {
                _trayManager = value;
                NotifyOfPropertyChange(() => TrayManager);
            }
        }

        private bool _startUpEnabled;
        public bool StartUpEnabled
        {
            get { return _startUpEnabled; }
            set
            {
                _startUpEnabled = value;
                NotifyOfPropertyChange(() => StartUpEnabled);
            }
        }

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                NotifyOfPropertyChange(() => CurrentLanguage);
            }
        }

    }
}

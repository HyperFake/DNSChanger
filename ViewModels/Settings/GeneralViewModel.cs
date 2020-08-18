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

        // Logging
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// GeneralViewModel Constructor, checks if DNS changer is enabled at start
        /// </summary>
        public GeneralViewModel()
        {
            StartUpEnabled = startUpHelper.CheckRegistryState();
        }

        /// <summary>
        /// Adds language button to system tray
        /// </summary>
        public void AddChangeLanguageToTray()
        {
            try
            {
                ToolStripMenuItem languageStripMenu = new ToolStripMenuItem();
                languageStripMenu.DropDownItems.Add(lgHelper.SavedValue("LanguageEnglishButton"), null, AddEnglishSystemTray);
                languageStripMenu.DropDownItems.Add(lgHelper.SavedValue("LanguageLithuanianButton"), null, AddLithuanianSystemTray);

                languageStripMenu.Text = lgHelper.SavedValue("ChangeLanguage");

                TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(languageStripMenu);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to add language button to system tray.");
            }

        }

        /// <summary>
        /// Sets the language to en-US
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void AddEnglishSystemTray(object sender, EventArgs e)
        {
            lgHelper.SetLanguage("en-US");
        }

        /// <summary>
        /// Sets the languae to lt-LT
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void AddLithuanianSystemTray(object sender, EventArgs e)
        {
            lgHelper.SetLanguage("lt-LT");
        }

        /// <summary>
        /// Adds Windows Start Up button to System tray
        /// </summary>
        public void AddToggleWindowsStartToTray()
        {
            try
            {
                // Setting up System tray item
                WindowsStartUp.Text = lgHelper.SavedValue("WindowsButton");
                WindowsStartUp.MouseDown += ToggleStartUpSystemTray;

                if (StartUpEnabled)
                    WindowsStartUp.Checked = true;
                else
                    WindowsStartUp.Checked = false;

                // Adding it
                TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(WindowsStartUp);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to add windows start up toggle to system tray");
            }

        }


        /// <summary>
        /// Checks if we need to add or remove DNS changer from registry, sets status accordingly
        /// </summary>
        private void ToggleStartUp()
        {
            try
            {
                if (StartUpEnabled)
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
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to toggle windows start up");
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

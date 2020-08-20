using DNS_changer.Helper;
using System;
using System.Windows.Forms;

namespace DNS_changer.ViewModels.Settings
{
    public class GeneralViewModel : Caliburn.Micro.Screen
    {

        // Start Up item
        readonly ToolStripMenuItem WindowsStartUp = new ToolStripMenuItem();

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// GeneralViewModel Constructor, checks if DNS changer is enabled at start
        /// </summary>
        public GeneralViewModel()
        {
            StartUpEnabled = WindowsStartUpHelper.CheckRegistryState();
        }

        /// <summary>
        /// Adds language button to system tray
        /// </summary>
        public void AddChangeLanguageToTray()
        {
            try
            {
                ToolStripMenuItem languageStripMenu = new ToolStripMenuItem();
                languageStripMenu.DropDownItems.Add(LanguageHelper.SavedValue("LanguageEnglishButton"), null, AddEnglishSystemTray);
                languageStripMenu.DropDownItems.Add(LanguageHelper.SavedValue("LanguageLithuanianButton"), null, AddLithuanianSystemTray);

                languageStripMenu.Text = LanguageHelper.SavedValue("ChangeLanguage");

                TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(languageStripMenu);
            }
            catch (Exception ex)
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
            LanguageHelper.SetLanguage("en-US");
        }

        /// <summary>
        /// Sets the languae to lt-LT
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void AddLithuanianSystemTray(object sender, EventArgs e)
        {
            LanguageHelper.SetLanguage("lt-LT");
        }

        /// <summary>
        /// Adds Windows Start Up button to System tray
        /// </summary>
        public void AddToggleWindowsStartToTray()
        {
            try
            {
                // Setting up System tray item
                WindowsStartUp.Text = LanguageHelper.SavedValue("WindowsButton");
                WindowsStartUp.MouseDown += ToggleStartUpSystemTray;

                if (StartUpEnabled)
                    WindowsStartUp.Checked = true;
                else
                    WindowsStartUp.Checked = false;

                // Adding it
                TrayManager.NotifyIcon.ContextMenuStrip.Items.Add(WindowsStartUp);
            }
            catch (Exception ex)
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
                    WindowsStartUpHelper.RemoveStartUpFromRegistry();
                    StartUpEnabled = false;
                    WindowsStartUp.Checked = false;
                }
                else
                {
                    WindowsStartUpHelper.AddStartUpToRegistry();
                    StartUpEnabled = true;
                    WindowsStartUp.Checked = true;
                }
            }
            catch (Exception ex)
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

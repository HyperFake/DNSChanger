using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Settings
{
    public class GeneralViewModel : Caliburn.Micro.Screen
    {

        // Start Up item
        readonly ToolStripMenuItem WindowsStartUp = new ToolStripMenuItem();

        // Languages

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// GeneralViewModel Constructor, checks if DNS changer is enabled at start
        /// </summary>
        public GeneralViewModel()
        {
            StartUpEnabled = WindowsStartUpHelper.CheckRegistryState();
            SetButtonLook();
            CreateLanguageListOnStart();
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
            LanguageHelper.SetLanguage(LanguageStrings.ShortEnglishUS);
        }

        /// <summary>
        /// Sets the language to lt-LT
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void AddLithuanianSystemTray(object sender, EventArgs e)
        {
            LanguageHelper.SetLanguage(LanguageStrings.ShortLithuania);
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
        public void ToggleStartUp()
        {
            try
            {
                if (StartUpEnabled)
                {
                    WindowsStartUpHelper.RemoveStartUpFromRegistry();
                    StartUpEnabled = false;
                    WindowsStartUp.Checked = false;
                    SetButtonLook();
                }
                else
                {
                    WindowsStartUpHelper.AddStartUpToRegistry();
                    StartUpEnabled = true;
                    WindowsStartUp.Checked = true;
                    SetButtonLook();
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
        /// <param name="e">EventArgs</param>
        private void ToggleStartUpSystemTray(object sender, EventArgs e)
        {
            ToggleStartUp();
        }

        private void CreateLanguageListOnStart()
        {
            BindableCollection<string> newList = new BindableCollection<string>();
            newList.Add(LanguageStrings.LongEnglishUS);
            newList.Add(LanguageStrings.LongLithuania);

            LanguageList = newList;

            SetCurrentLanguage();
        }

        public void OnSelectionChanged()
        {
            if (SelectedLanguageList == LanguageStrings.LongEnglishUS)
            {
                LanguageHelper.SetLanguage(LanguageStrings.ShortEnglishUS);
                SelectedLanguageList = LanguageStrings.LongEnglishUS;
            }
            else if (SelectedLanguageList == LanguageStrings.LongLithuania)
            {
                LanguageHelper.SetLanguage(LanguageStrings.ShortLithuania);
                SelectedLanguageList = LanguageStrings.LongLithuania;
            }

            UpdatePageLanguage();
        }

        private void SetCurrentLanguage()
        {
            string currentLanguage = LanguageHelper.GetLanguage();

            if (currentLanguage == LanguageStrings.ShortEnglishUS)
                SelectedLanguageList = LanguageStrings.LongEnglishUS;
            else if (currentLanguage == LanguageStrings.ShortLithuania)
                SelectedLanguageList = LanguageStrings.LongLithuania;
        }


        private void UpdatePageLanguage()
        {
            if(StartUpEnabled)
            {
                WindowsButtonText = LanguageHelper.SavedValue("ButtonStateOn");

            }
            else
            {
                WindowsButtonText = LanguageHelper.SavedValue("ButtonStateOff");

            }
        }
        private void SetButtonLook()
        {
            BrushConverter brushCV = new BrushConverter();
            if(StartUpEnabled)
            {
                WindowsButtonText = LanguageHelper.SavedValue("ButtonStateOn");
                WindowsButtonColor = (Brush)brushCV.ConvertFromString("#1a8cff"); // blue
                WindowButtonColorHover = (Brush)brushCV.ConvertFromString("#0073e6"); // +10% strength blue
                WindowButtonColorPressed = (Brush)brushCV.ConvertFromString("#0059b3"); // +20% strength blue
            }
            else
            {
                WindowsButtonText = LanguageHelper.SavedValue("ButtonStateOff");
                WindowsButtonColor = Brushes.Red;
                WindowsButtonColor = (Brush)brushCV.ConvertFromString("#808080"); // grey
                WindowButtonColorHover = (Brush)brushCV.ConvertFromString("#666666"); // +10% strength grey
                WindowButtonColorPressed = (Brush)brushCV.ConvertFromString("#4d4d4d"); // +20% strength grey
            }
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

        private string _windowsButtonText;
        public string WindowsButtonText
        {
            get { return _windowsButtonText; }
            set
            {
                _windowsButtonText = value;
                NotifyOfPropertyChange(() => WindowsButtonText);
            }
        }

        private Brush _windowsButtonColor;
        public Brush WindowsButtonColor
        {
            get { return _windowsButtonColor; }
            set
            {
                _windowsButtonColor = value;
                NotifyOfPropertyChange(() => WindowsButtonColor);
            }
        }

        private BindableCollection<string> _languageList;
        public BindableCollection<string> LanguageList
        {
            get { return _languageList; }
            set
            {
                _languageList = value;
                NotifyOfPropertyChange(() => LanguageList);
            }
        }

        private string _selectedLanguageList;
        public string SelectedLanguageList
        {
            get { return _selectedLanguageList; }
            set
            {
                _selectedLanguageList = value;
                NotifyOfPropertyChange(() => SelectedLanguageList);
            }
        }

        private Brush _windowButtonColorHover;
        public Brush WindowButtonColorHover
        {
            get { return _windowButtonColorHover; }
            set
            {
                _windowButtonColorHover = value;
                NotifyOfPropertyChange(() => WindowButtonColorHover);
            }
        }

        private Brush _windowButtonColorPressed;
        public Brush WindowButtonColorPressed
        {
            get { return _windowButtonColorPressed; }
            set
            {
                _windowButtonColorPressed = value;
                NotifyOfPropertyChange(() => WindowButtonColorPressed);
            }
        }
    }
}

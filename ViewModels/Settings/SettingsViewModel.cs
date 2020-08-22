using Caliburn.Micro;
using DNS_changer.Helper;

namespace DNS_changer.ViewModels.Settings
{
    public class SettingsViewModel : Conductor<object>
    {
        // Setting view models
        readonly GeneralViewModel generalView = new GeneralViewModel();
        readonly PasswordViewModel passwordView = new PasswordViewModel();

        public SettingsViewModel(TrayManager Manager)
        {
            // Send tray system to subview
            generalView.TrayManager = Manager;

            // Activate subview
            GeneralSettings();
        }

        /// <summary>
        /// Adds language selection button to system tray
        /// </summary>
        public void AddLanguageButton()
        {
            generalView.AddChangeLanguageToTray();
        }

        /// <summary>
        /// Adds Windows start up toggle button to system tray
        /// </summary>
        public void AddWindowsStartButton()
        {
            generalView.AddToggleWindowsStartToTray();
        }

        /// <summary>
        /// Activates GeneralView screen
        /// </summary>
        public void GeneralSettings()
        {
            ActivateItem(generalView);
        }

        /// <summary>
        /// Activates PasswordView screen
        /// </summary>
        public void PasswordSettings()
        {
            ActivateItem(passwordView);
        }
    }
}

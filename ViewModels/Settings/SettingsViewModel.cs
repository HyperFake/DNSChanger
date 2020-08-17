using Caliburn.Micro;
using DNS_changer.Helper;

namespace DNS_changer.ViewModels.Settings
{
    public class SettingsViewModel : Conductor<object>
    {
        // Setting view models
        GeneralViewModel generalView = new GeneralViewModel();
        PasswordViewModel passwordView = new PasswordViewModel();

        public SettingsViewModel(TrayManager Manager)
        {
            // Send tray system to subview
            generalView.TrayManager = Manager;

            // Activate subview
            GeneralSettings();
        }

        public void AddLanguageButton()
        {
            generalView.AddChangeLanguageToTray();
        }

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

using Caliburn.Micro;
using DNS_changer.Helper;

namespace DNS_changer.ViewModels.Settings
{
    public class SettingsViewModel : Conductor<object>
    {
        // System tray 
        private TrayManager trayManager;

        // Setting view models
        GeneralViewModel generalView = new GeneralViewModel();
        PasswordViewModel passwordView = new PasswordViewModel();

        public SettingsViewModel(TrayManager Manager)
        {
            trayManager = Manager;
            GeneralSettings();
        }

        public void GeneralSettings()
        {
            ActivateItem(generalView);
        }

        public void PasswordSettings()
        {
            ActivateItem(passwordView);
        }

    }
}

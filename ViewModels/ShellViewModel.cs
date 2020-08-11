using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_changer.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {

        // Views
        MainViewModel mainView = new MainViewModel();
        SettingsViewModel settingsView = new SettingsViewModel();

        public ShellViewModel()
        {
            // On startup load main view
            ActivateItem(mainView);
        }

        public void LoadMainView()
        {
            ActivateItem(mainView);
        }

        public void LoadSettingsView()
        {
            ActivateItem(settingsView);
        }
    }
}

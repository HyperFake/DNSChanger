using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
            return;
        }
    }
}

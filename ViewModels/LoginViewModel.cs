using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Windows;

namespace DNS_changer.ViewModels
{
    class LoginViewModel : Screen, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler events;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Login()
        {
            
        }

        public LoginViewModel()
        {

        }

        public bool Success()
        {
            return true;
        }

        public void Window()
        {
            MainWindowVisibility = Visibility.Collapsed;
        }

        private Visibility _mainWindowVisibility;

        public Visibility MainWindowVisibility
        {
            get { return _mainWindowVisibility; }
            set
            {
                _mainWindowVisibility = value;
                NotifyPropertyChanged();
            }
        }
    }
}

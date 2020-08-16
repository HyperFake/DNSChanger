using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DNS_changer.ViewModels
{
    class LoginViewModel : Screen, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void LoginEventHandler(object sender, EventArgs e);
        public event LoginEventHandler OnLoginEvent;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoginButton()
        {
            PasswordHashing hashing = new PasswordHashing();
            string hashedPassword = hashing.HashPassword(PasswordInput);

            if(hashing.ComparePasswords(hashedPassword, Properties.Settings.Default.Password))
            {
                if (OnLoginEvent == null) return;

                EventArgs args = new EventArgs();
                OnLoginEvent(this, args);
            }
            else
            {
                LoginErrorText = "Password is incorrect. Try again";
                PasswordInput = "";
            }
        }

        public LoginViewModel()
        {

        }

        public void OnPasswordChanged(PasswordBox source)
        {
            PasswordInput = source.Password;
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

        private string _passwordInput;

        public string PasswordInput
        {
            get { return _passwordInput; }
            set
            {
                _passwordInput = value;
                NotifyOfPropertyChange(() => PasswordInput);
            }
        }

        private string _loginErrorText;

        public string LoginErrorText
        {
            get { return _loginErrorText; }
            set
            {
                _loginErrorText = value;
                NotifyPropertyChanged();
            }
        }
    }
}

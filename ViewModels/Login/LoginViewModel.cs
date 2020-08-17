﻿using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Windows.Controls;

namespace DNS_changer.ViewModels.Login
{
    class LoginViewModel : Screen
    {

        // For closing screen upon succesful login
        public delegate void LoginEventHandler(object sender, EventArgs e);
        public event LoginEventHandler OnLoginEvent;

        /// <summary>
        /// Login button function, compares passwords and activates login event if success
        /// </summary>
        public void LoginButton()
        {
            PasswordHelper passwordHelper = new PasswordHelper();

            if (passwordHelper.ComparePasswordToStored(PasswordInput))
            {
                if (OnLoginEvent == null) return;

                EventArgs args = new EventArgs();
                OnLoginEvent(this, args);
            }
            else
            {
                LoginErrorText = "Password is incorrect. Try again";
            }
        }

        /// <summary>
        /// On password input changed, update PasswordInput string
        /// </summary>
        /// <param name="source">PasswordBox</param>
        public void OnPasswordChanged(PasswordBox source)
        {
            PasswordInput = source.Password;
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
                NotifyOfPropertyChange(() => LoginErrorText);
            }
        }
    }
}

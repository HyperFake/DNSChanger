using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DNS_changer.ViewModels.Login
{
    class LoginViewModel : Screen
    {
        // For closing screen upon succesful login
        public delegate void LoginEventHandler(object sender, EventArgs e);
        public event LoginEventHandler OnLoginEvent;

        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Login button function, compares passwords and activates login event if success
        /// </summary>
        public async void LoginButton()
        {
            // Reset login text 
            LoginErrorText = "";

            bool result = await LoginAsync();

            try
            {
                if (result)
                {
                    if (OnLoginEvent == null) return;
                    OnLoginEvent(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Login function failed");
            }
        }

        /// <summary>
        /// Login async function. Checks if password input is not empty and is equal to saved
        /// </summary>
        /// <returns>true if it's saved password, false otherwise</returns>
        private async Task<bool> LoginAsync()
        {

            bool tempResult = false;
            await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(PasswordInput))
                    {
                        LoginErrorText = LanguageHelper.SavedValue("LoginRegisterEmpty");
                        tempResult = false;
                    }

                    if (PasswordHelper.ComparePasswordToStored(PasswordInput))
                    {
                        tempResult = true;
                    }
                    else
                    {
                        LoginErrorText = LanguageHelper.SavedValue("LoginErrorText");
                        tempResult = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "LoginAsync function failed");
                    tempResult = false;
                }
            });

            return tempResult;
        }

        /// <summary>
        /// Captures enter key and presses login button
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        public void EnterButtonLogin(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton();
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

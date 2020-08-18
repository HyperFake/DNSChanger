using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Settings
{
    public class PasswordViewModel : Screen
    {
        // For password comparing & hashing
        PasswordHelper passwordHelper = new PasswordHelper();

        // Language support
        LanguageHelper lgHelper = new LanguageHelper();

        // Logging
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Changes password if parameters meet
        /// </summary>
        public void ChangePassword()
        {
            try
            {
                // Check if password meets the requirements and isn't used as current password
                if (ParsePasswordInput(NewPassword) && PasswordsComparing())
                {
                    Properties.Settings.Default.Password = passwordHelper.HashPassword(NewPassword);
                    Properties.Settings.Default.Save();
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to change the password");
            }

        }

        /// <summary>
        /// Parses password input and represents it's strength accordingly
        /// </summary>
        /// <param name="password">Password user gave</param>
        /// <returns>true if user's given password is good. False otherwise</returns>
        private bool ParsePasswordInput(string password)
        {
            try
            {
                // If empty set default look
                if (string.IsNullOrWhiteSpace(password))
                {
                    DefaultLook();
                    ButtonEnabled = false;
                    return false;
                }

                // This is password security number that we use for progressbar
                int passwordValue = password.Length * 5;

                // Password must be atleast 3 symbols
                if (password.Length <= 3)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextShort"), passwordValue, Brushes.Red, Brushes.Black);
                    ButtonEnabled = false;
                    return false;
                }
                // Password must be less than 18 symbols
                else if (password.Length >= 20)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextLong"), passwordValue, Brushes.Red, Brushes.Black);
                    ButtonEnabled = false;
                    return false;
                }

                // Enable register button as it meets standarts
                ButtonEnabled = true;


                // Calculate additional password strength
                if (password.Any(char.IsDigit))
                    passwordValue += 10;
                if (password.Any(char.IsUpper))
                    passwordValue += 10;

                // Weak password
                if (passwordValue <= 30)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextWeak"), passwordValue, Brushes.Red, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 30 && passwordValue <= 60)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextMedium"), passwordValue, Brushes.Yellow, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 60 && passwordValue <= 85)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextGood"), passwordValue, Brushes.LightGreen, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 85)
                {
                    ChangeLook(lgHelper.SavedValue("BarTextVGood"), passwordValue, Brushes.Green, Brushes.Black);
                    return true;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to parse the password input (Password Change View)");
            }

            return false;
        }

        /// <summary>
        /// Default look for password strength bar
        /// </summary>
        private void DefaultLook()
        {
            ChangeLook(lgHelper.SavedValue("BarDefaultText"), 5, Brushes.LightGray, Brushes.Gray);
        }

        /// <summary>
        /// Changes password strength bar look
        /// </summary>
        /// <param name="HeaderText">Bar text</param>
        /// <param name="barValue">Bar value</param>
        /// <param name="barColor">Bar color</param>
        /// <param name="barTextColor">Bar text color</param>
        private void ChangeLook(string HeaderText, int barValue, Brush barColor, Brush barTextColor)
        {
            try
            {
                BarText = HeaderText;
                BarValue = barValue;
                BarColor = barColor;
                BarTextColor = barTextColor;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to change look (Password change view)");
            }

        }

        /// <summary>
        /// Compares Old password input to Stored password, New password to Repeat password and New password to Stored password
        /// </summary>
        /// <returns></returns>
        private bool PasswordsComparing()
        {
            try
            {
                if (!passwordHelper.ComparePasswordToStored(OldPassword))
                {
                    ErrorText = lgHelper.SavedValue("OldPasswordIncorrect");
                    return false;
                }
                else if (!NewPassword.Equals(RepeatPassword))
                {
                    ErrorText = lgHelper.SavedValue("NewPasswordNoMatch");
                    return false;
                }
                else if (passwordHelper.ComparePasswordToStored(NewPassword))
                {
                    ErrorText = lgHelper.SavedValue("NewPasswordOldValue");
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Failed to compare passwords (Password change view)");
            }

            ErrorText = "";
            return true;
        }

        /// <summary>
        /// On old password input changed, updates OldPassword string
        /// </summary>
        /// <param name="source">PasswordBox</param>
        public void OnOldPasswordChanged(PasswordBox source)
        {
            OldPassword = source.Password;
        }

        /// <summary>
        /// On new password input changed, updates NewPassword string
        /// </summary>
        /// <param name="source">PasswordBox</param>
        public void OnNewPasswordChanged(PasswordBox source)
        {
            NewPassword = source.Password;
        }

        /// <summary>
        /// On repeat password input changed, updates RepeatPassword string
        /// </summary>
        /// <param name="source">PasswordBox</param>
        public void OnRepeatPasswordChanged(PasswordBox source)
        {
            RepeatPassword = source.Password;
        }

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                NotifyOfPropertyChange(() => OldPassword);
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                ParsePasswordInput(value);
                NotifyOfPropertyChange(() => NewPassword);
            }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;
                NotifyOfPropertyChange(() => RepeatPassword);
            }
        }

        private string _barText;
        public string BarText
        {
            get { return _barText; }
            set
            {
                _barText = value;
                NotifyOfPropertyChange(() => BarText);
            }
        }

        private Brush _barTextColor;
        public Brush BarTextColor
        {
            get { return _barTextColor; }
            set
            {
                _barTextColor = value;
                NotifyOfPropertyChange(() => BarTextColor);
            }
        }

        private int _barValue;
        public int BarValue
        {
            get { return _barValue; }
            set
            {
                _barValue = value;
                NotifyOfPropertyChange(() => BarValue);
            }
        }

        private Brush _barColor;
        public Brush BarColor
        {
            get { return _barColor; }
            set
            {
                _barColor = value;
                NotifyOfPropertyChange(() => BarColor);
            }
        }

        // For enabling or disabling button
        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get { return _buttonEnabled; }
            set
            {
                _buttonEnabled = value;
                NotifyOfPropertyChange(() => ButtonEnabled);
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                NotifyOfPropertyChange(() => ErrorText);
            }
        }
    }
}

using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Settings
{
    public class PasswordViewModel : Screen
    {
        // For password comparing & hashing
        PasswordHelper passwordHelper = new PasswordHelper();

        public PasswordViewModel()
        {

        }

        public void ChangePassword()
        {
            if(ParsePasswordInput(NewPassword) && PasswordsComparing())
            {
                Properties.Settings.Default.Password = passwordHelper.HashPassword(NewPassword);
                Properties.Settings.Default.Save();
            }
        }
        private bool ParsePasswordInput(string password)
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
                ChangeLook("Too short", passwordValue, Brushes.Red, Brushes.Black);
                ButtonEnabled = false;
                return false;
            }
            // Password must be less than 18 symbols
            else if (password.Length >= 20)
            {
                ChangeLook("Too long", passwordValue, Brushes.Red, Brushes.Black);
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
                ChangeLook("Weak", passwordValue, Brushes.Red, Brushes.Black);
                return true;
            }
            else if (passwordValue > 30 && passwordValue <= 60)
            {
                ChangeLook("Medium", passwordValue, Brushes.Yellow, Brushes.Black);
                return true;
            }
            else if (passwordValue > 60 && passwordValue <= 85)
            {
                ChangeLook("Good", passwordValue, Brushes.LightGreen, Brushes.Black);
                return true;
            }
            else if (passwordValue > 85)
            {
                ChangeLook("Very Good", passwordValue, Brushes.Green, Brushes.Black);
                return true;
            }

            return false;
        }

        private void DefaultLook()
        {
            ChangeLook("Input Password", 5, Brushes.LightGray, Brushes.Gray);
        }

        private void ChangeLook(string HeaderText, int barValue, Brush barColor, Brush barTextColor)
        {
            BarText = HeaderText;
            BarValue = barValue;
            BarColor = barColor;
            BarTextColor = barTextColor;
        }

        private bool PasswordsComparing()
        {
            if(!passwordHelper.ComparePasswordToStored(OldPassword))
            {
                ErrorText = "Old password is incorrect";
                return false;
            }
            else if(!NewPassword.Equals(RepeatPassword))
            {
                ErrorText = "New passwords doesn't match";
                return false;
            }
            else if(passwordHelper.ComparePasswordToStored(NewPassword))
            {
                ErrorText = "New password cannot not match old one";
                return false;
            }

            ErrorText = "";
            return true;
        }

        public void OnOldPasswordChanged(PasswordBox source)
        {
            OldPassword = source.Password;
        }

        public void OnNewPasswordChanged(PasswordBox source)
        {
            NewPassword = source.Password;
        }

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

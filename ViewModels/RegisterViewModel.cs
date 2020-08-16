using Caliburn.Micro;
using DNS_changer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DNS_changer.ViewModels
{
    class RegisterViewModel : Screen
    {
        public delegate void RegisterEventHandler(object sender, EventArgs e);
        public event RegisterEventHandler OnRegisterEvent;

        public RegisterViewModel()
        {
            // Set default parameters
            PasswordDifficultyDefaultLook();
        }

        public void RegisterButton()
        {
            if(ParsePasswordInput(PasswordInput))
            {
                string hashedPassword = new PasswordHashing().HashPassword(PasswordInput); 
                // Set the password
                Properties.Settings.Default.Password = hashedPassword;
                Properties.Settings.Default.Save();


                // Change the window
                if (OnRegisterEvent == null) return;
                EventArgs args = new EventArgs();
                OnRegisterEvent(this, args);
            }
        }

        private bool ParsePasswordInput(string password)
        {
            // If empty set default look
            if(string.IsNullOrWhiteSpace(password))
            {
                PasswordDifficultyDefaultLook();
                RegisterButtonEnabled = false;
                return false;
            }

            // This is password security number that we use for progressbar
            int passwordValue = password.Length * 5;

            // Password must be atleast 3 symbols
            if (password.Length <= 3)
            {
                SetPasswordLook("Too short", passwordValue, Brushes.Red, Brushes.Black);
                RegisterButtonEnabled = false;
                return false;
            }
            // Password must be less than 18 symbols
            if (password.Length >= 20)
            {
                SetPasswordLook("Too long", passwordValue, Brushes.Red, Brushes.Black);
                RegisterButtonEnabled = false;
                return false;
            }

            // Enable register button as it meets standarts
            RegisterButtonEnabled = true;


            // Calculate additional password strength
            if (password.Any(char.IsDigit))
                passwordValue += 10;
            if (password.Any(char.IsUpper))
                passwordValue += 10;

            // Weak password
            if(passwordValue <= 30)
            {
                SetPasswordLook("Weak", passwordValue, Brushes.Red, Brushes.Black);
                return true;
            }
            else if(passwordValue > 30 && passwordValue <= 60)
            {
                SetPasswordLook("Medium", passwordValue, Brushes.Yellow, Brushes.Black);
                return true;
            }
            else if(passwordValue > 60 && passwordValue <= 85)
            {
                SetPasswordLook("Good", passwordValue, Brushes.LightGreen, Brushes.Black);
                return true;
            }
            else if (passwordValue > 85)
            {
                SetPasswordLook("Very Good", passwordValue, Brushes.Green, Brushes.Black);
                return true;
            }

            return false;
        }


        private void SetPasswordLook(string HeaderText, int barValue, Brush barColor, Brush barTextColor)
        {
            PasswordStrengthText = HeaderText;
            PasswordStrengthValue = barValue;
            RegisterStrengthBarColor = barColor;
            RegisterStrengthBarTextColor = barTextColor;
        }

        public void OnPasswordChanged(PasswordBox source)
        {
            PasswordInput = source.Password;
        }

        private void PasswordDifficultyDefaultLook()
        {
            SetPasswordLook("Input Password", 5, Brushes.LightGray, Brushes.Gray);
        }

        private string _password;

        public string PasswordInput
        {
            get { return _password; }
            set
            {
                _password = value;
                ParsePasswordInput(value);
                NotifyOfPropertyChange(() => PasswordInput);
            }
        }

        private string _passwordStrengthText;

        public string PasswordStrengthText
        {
            get { return _passwordStrengthText; }
            set
            {
                _passwordStrengthText = value;
                NotifyOfPropertyChange(() => PasswordStrengthText);
            }
        }

        private int _passwordStrengthValue;

        public int PasswordStrengthValue
        {
            get { return _passwordStrengthValue; }
            set
            {
                _passwordStrengthValue = value;
                NotifyOfPropertyChange(() => PasswordStrengthValue);
            }
        }

        private Brush _registerStrengthBarColor;

        public Brush RegisterStrengthBarColor
        {
            get { return _registerStrengthBarColor; }
            set
            {
                _registerStrengthBarColor = value;
                NotifyOfPropertyChange(() => RegisterStrengthBarColor);
            }
        }

        private Brush _registerStrengthBarTextColor;

        public Brush RegisterStrengthBarTextColor
        {
            get { return _registerStrengthBarTextColor; }
            set
            {
                _registerStrengthBarTextColor = value;
                NotifyOfPropertyChange(() => RegisterStrengthBarTextColor);

            }
        }

        private bool _registerButtonEnabled;
        public bool RegisterButtonEnabled
        {
            get { return _registerButtonEnabled; }
            set
            {
                _registerButtonEnabled = value;
                NotifyOfPropertyChange(() => RegisterButtonEnabled);
            }
        }
    }
}

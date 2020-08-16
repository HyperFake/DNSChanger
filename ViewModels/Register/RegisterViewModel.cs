using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Register
{
    class RegisterViewModel : Screen
    {
        public delegate void RegisterEventHandler(object sender, EventArgs e);
        public event RegisterEventHandler OnRegisterEvent;

        public RegisterViewModel()
        {
            // Set default parameters
            DefaultLook();
        }

        public void RegisterButton()
        {
            if(ParsePasswordInput(PasswordInput))
            {
                string hashedPassword = new PasswordHelper().HashPassword(PasswordInput); 
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
            if (password.Length >= 20)
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
            if(passwordValue <= 30)
            {
                ChangeLook("Weak", passwordValue, Brushes.Red, Brushes.Black);
                return true;
            }
            else if(passwordValue > 30 && passwordValue <= 60)
            {
                ChangeLook("Medium", passwordValue, Brushes.Yellow, Brushes.Black);
                return true;
            }
            else if(passwordValue > 60 && passwordValue <= 85)
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


        private void ChangeLook(string HeaderText, int barValue, Brush barColor, Brush barTextColor)
        {
            BarText = HeaderText;
            BarValue = barValue;
            BarColor = barColor;
            BarTextColor = barTextColor;
        }

        public void OnPasswordChanged(PasswordBox source)
        {
            PasswordInput = source.Password;
        }

        private void DefaultLook()
        {
            ChangeLook("Input Password", 5, Brushes.LightGray, Brushes.Gray);
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
    }
}

using Caliburn.Micro;
using DNS_changer.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DNS_changer.ViewModels.Register
{
    class RegisterViewModel : Screen
    {
        // Event for changing screen upon successful register
        public delegate void RegisterEventHandler(object sender, EventArgs e);
        public event RegisterEventHandler OnRegisterEvent;


        // Logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public RegisterViewModel()
        {
            // Set default parameters
            DefaultLook();
        }

        /// <summary>
        /// Checks password input and stores it if met parameters
        /// </summary>
        public async void RegisterButton()
        {
            bool result = await RegisterAsync();

            try
            {
                if (result)
                {
                    // Change the window
                    if (OnRegisterEvent == null) return;
                    OnRegisterEvent(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Register function failed");
            }
        }

        /// <summary>
        /// Register Async function. Parses password input and saves password value to Properties
        /// </summary>
        /// <returns>true if password can be parsed, false otherwise</returns>
        public async Task<bool> RegisterAsync()
        {
            bool resultTemp = false;
            await Task.Run(() =>
            {
                try
                {
                    if (ParsePasswordInput(PasswordInput))
                    {
                        resultTemp = true;
                        string hashedPassword = PasswordHelper.HashPassword(PasswordInput);

                        // Set the password
                        PasswordHelper.SetPassword(hashedPassword);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "RegisterAsync failed");
                }
            });
            return resultTemp;
        }

        /// <summary>
        /// Parses user password input
        /// </summary>
        /// <param name="password">Password string</param>
        /// <returns>true if it meets parameters. False otherwise</returns>
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
                    ChangeLook(LanguageHelper.SavedValue("BarTextShort"), passwordValue, Brushes.Red, Brushes.Black);
                    ButtonEnabled = false;
                    return false;
                }
                // Password must be less than 18 symbols
                if (password.Length >= 20)
                {
                    ChangeLook(LanguageHelper.SavedValue("BarTextLong"), passwordValue, Brushes.Red, Brushes.Black);
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
                    ChangeLook(LanguageHelper.SavedValue("BarTextWeak"), passwordValue, Brushes.Red, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 30 && passwordValue <= 60)
                {
                    ChangeLook(LanguageHelper.SavedValue("BarTextMedium"), passwordValue, Brushes.Yellow, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 60 && passwordValue <= 85)
                {
                    ChangeLook(LanguageHelper.SavedValue("BarTextGood"), passwordValue, Brushes.LightGreen, Brushes.Black);
                    return true;
                }
                else if (passwordValue > 85)
                {
                    ChangeLook(LanguageHelper.SavedValue("BarTextVGood"), passwordValue, Brushes.Green, Brushes.Black);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to parse password input");
            }

            return false;
        }

        /// <summary>
        /// Enter key push activates register button
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        public void EnterButtonRegister(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisterButton();
            }
        }

        /// <summary>
        /// Changes bar look
        /// </summary>
        /// <param name="barText">Bar text</param>
        /// <param name="barValue">Bar value</param>
        /// <param name="barColor">Bar color</param>
        /// <param name="barTextColor">Bar text color</param>
        private void ChangeLook(string barText, int barValue, Brush barColor, Brush barTextColor)
        {
            BarText = barText;
            BarValue = barValue;
            BarColor = barColor;
            BarTextColor = barTextColor;
        }

        /// <summary>
        /// On password input changed, update PasswordInput string
        /// </summary>
        /// <param name="source">PasswordBox</param>
        public void OnPasswordChanged(PasswordBox source)
        {
            PasswordInput = source.Password;
        }

        /// <summary>
        /// Bar default look
        /// </summary>
        private void DefaultLook()
        {
            ChangeLook(LanguageHelper.SavedValue("BarDefaultText"), 0, Brushes.LightGray, Brushes.Gray);
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

        // Disable or enable button
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

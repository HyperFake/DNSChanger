using System.Windows;

namespace DNS_changer.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        // logging
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginView()
        {
            InitializeComponent();
        }

    }
}

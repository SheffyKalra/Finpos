using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using System;
using System.Windows;
using FinPos.Client.Views.UserControls;
using System.Windows.Controls;
using NLog;

namespace FinPos.Client.Views
{
    /// <summary>
    /// Interaction logic for ClientLogin.xaml
    /// </summary>
    public partial class ClientLogin : Window
    {
        private int _noOfErrorsOnScreen = 0;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ClientLogin()
        {
            InitializeComponent();
            CommonFunction.Common._containerWidth = SystemParameters.PrimaryScreenWidth;
            CommonFunction.Common._containerHeight = SystemParameters.PrimaryScreenHeight;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }

        private void LoginUser()
        {
            UserController user = new UserController();
            UserModel userData = user.GetUser(txtEmail.Text, txtPassword.Password);
            if (userData != null)
            {
                this.Hide();
                try
                {
                    Main form = new Main(userData);
                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
            else
            {
                errorGrid.Visibility = Visibility.Visible;
                errormessage.Text = (string)Application.Current.Resources["login_ErrorMsg"];
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmail_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void AddCustomer_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                LoginUser();
            }
        }
    }
}

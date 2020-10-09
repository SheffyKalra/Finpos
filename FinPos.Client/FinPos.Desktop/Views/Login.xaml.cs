using FinPos.Client.View;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using FinPOS.DomainContracts.Model;
using FinPosCrm.Utility.Enums;
using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace FinPos.Client.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        private int _noOfErrorsOnScreen = 0;
        private string header = "Login";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Login()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();

            bool IsServerLicenseExist = CommonFunctions.IsLicenseActivate(Convert.ToString(CommonEnums.Edition.Server), Convert.ToString(CommonEnum.IndustryTypes.RS));
            bool isExpired = CommonFunctions.IsKeyExpired(IsServerLicenseExist ? Convert.ToString(CommonEnums.Edition.Server) : Convert.ToString(CommonEnums.Edition.Client), Convert.ToString(CommonEnum.IndustryTypes.RS));
            bool IsActivatedTrue = CommonFunctions.IsActivatedTrue(IsServerLicenseExist ? Convert.ToString(CommonEnums.Edition.Server) : Convert.ToString(CommonEnums.Edition.Client), Convert.ToString(CommonEnum.IndustryTypes.RS));
            string confirmationMessage = string.Empty;
            bool isLogin = false;
            if (!isExpired)
            {
                if (IsActivatedTrue)
                {
                    bool IsLicenseExist = CommonFunctions.IsLicenseActivate(Convert.ToString(CommonEnums.Edition.Client), Convert.ToString(CommonEnum.IndustryTypes.RS));
                    bool isMontlyExpired = CommonFunctions.IsMonthlyTimeExpired(IsServerLicenseExist ? Convert.ToString(CommonEnums.Edition.Server) : Convert.ToString(CommonEnums.Edition.Client), Convert.ToString(CommonEnum.IndustryTypes.RS));


                    bool isClientLogin = false;

                    if (isExpired)
                    {
                        confirmationMessage = (string)Application.Current.Resources["expired_KeyMsg"];
                        isLogin = true;
                    }
                    else if (isMontlyExpired)
                    {
                        confirmationMessage = (string)Application.Current.Resources["planExpired_KeyMsg"];
                        isLogin = true;
                    }
                    else if (IsLicenseExist || IsServerLicenseExist)
                    {
                        isClientLogin = true;
                        isLogin = true;
                    }
                    else
                    {
                        Uri uri = new Uri("/ResourceFiles/en.xaml", UriKind.RelativeOrAbsolute);
                        var resDict = Application.LoadComponent(uri) as ResourceDictionary;
                        this.Resources.MergedDictionaries.Add(resDict);
                    }
                    if (!isClientLogin && isLogin)
                    {
                        ConfirmationPopup form = new ConfirmationPopup(confirmationMessage, header, false);
                        form.ShowDialog();

                        this.Close();
                    }
                    else if (isClientLogin && isLogin)
                    {
                        ClientLogin form = new ClientLogin();
                        form.Show();
                        this.Close();
                    }
                    }
                else
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["incorrect_SystemDateMsg"], header, false);
                    form.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                confirmationMessage = (string)Application.Current.Resources["expired_KeyMsg"];
                isLogin = true;
                ConfirmationPopup form = new ConfirmationPopup(confirmationMessage, header, false);
                form.ShowDialog();
                this.Close();
            }
            // this.pageFrame.Content = page;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length == 0 || txtPassword.Password.Length == 0)
            {
                errorGrid.Visibility = Visibility.Visible;
                errormessage.Text = (string)Application.Current.Resources["credential_ErrorMsg"];
            }

            else
            {
                try
                {
                    LoginModel login = new LoginModel(txtEmail.Text, txtPassword.Password, false);
                    HttpClient client = new HttpClient();
                    StringContent queryString = new StringContent(login.ToString());
                    client.BaseAddress = new Uri(_baseUrl);

                    HttpResponseMessage response = client.PostAsJsonAsync("Account/customerValidate", login).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        var accessToken = response.Content.ReadAsStringAsync().Result;
                        dynamic token = JsonConvert.DeserializeObject(accessToken);
                        accessToken = token["access_token"];
                        var macadress = CommonFunctions.GetMacAddress();
                        this.Hide();
                        LicenseModel license = new LicenseModel(string.Empty, Convert.ToString(macadress), Convert.ToString(accessToken));
                        License form = new License(license);
                        form.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        errorGrid.Visibility = Visibility.Visible;
                        errormessage.Text = response.ReasonPhrase;
                    }
                }
                catch (Exception ex)
                {
                    errorGrid.Visibility = Visibility.Visible;
                    errormessage.Text = ex.InnerException.InnerException.Message;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmail_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            //  _noOfErrorsOnScreen = 1;
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
    }
}

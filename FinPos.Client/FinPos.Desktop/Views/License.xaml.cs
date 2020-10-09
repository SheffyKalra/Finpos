using FinPos.Client.Views;
using FinPos.Utility.CommonMethods;
using FinPOS.DomainContracts.Model;
using FinPosCrm.Utility;
using FinPosCrm.Utility.Enums;
using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinPos.Client.View
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : Window
    {
        public LicenseModel _AccessToken;
        public string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        private int _noOfErrorsOnScreen = 0;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public License()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        public License(LicenseModel license)
        {
            InitializeComponent();
            this._AccessToken = license;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtlicenseKey.Text))
            {
                // errormessage.Text = "Enter an email.";
                txtlicenseKey.BorderThickness = new Thickness(2);
                txtlicenseKey.BorderBrush = Brushes.Red;
            }
            else
            {
                HttpClient client = new HttpClient();
                string[] licenseEdition = GetDecryptLicense(txtlicenseKey.Text);
                if (licenseEdition != null)
                {
                    if (licenseEdition[5] != Convert.ToString(CommonEnums.Edition.Client))
                    {
                        errorGrid.Visibility = Visibility.Visible;
                        errormessage.Text = (string)Application.Current.Resources["invalid_ClientKeyMsg"]; 
                    }
                    else
                    {
                        //try
                        //{
                        LicenseModel data = new LicenseModel(txtlicenseKey.Text, this._AccessToken.MacAddress, this._AccessToken.AccessToken);
                        client.BaseAddress = new Uri(_baseUrl);
                        //  CommonFunctions obj = new CommonFunctions();
                        //   obj.createRegistry();

                        HttpResponseMessage response = client.PostAsJsonAsync("Account/licenskey", data).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var license = response.Content.ReadAsStringAsync().Result;
                            dynamic token = JsonConvert.DeserializeObject(license);
                            var accessToken = token["AccessToken"];
                            var licenseKey = token["LicenseKey"];
                            //  var generatedLicenseKey = new LicenseModel();
                            string[] modifylicense = GetDecryptLicense(Convert.ToString(licenseKey));
                            string edition = modifylicense[5];
                            string plantype = modifylicense[1];
                            string industryType = modifylicense[3];
                            CommonFunctions obj = new CommonFunctions(licenseKey, accessToken, this._AccessToken.MacAddress);
                            var isSaved = obj.createRegistry(edition, Convert.ToInt32(plantype), Convert.ToInt32(industryType));
                            if (isSaved)
                            {
                                this.Close();
                                ClientLogin form = new ClientLogin();
                                form.ShowDialog();
                                //MyPopup.IsOpen = true;

                            }
                            else
                            {
                                errorGrid.Visibility = Visibility.Visible;
                                errormessage.Text = (string)Application.Current.Resources["key_ActivatedAllreadyErrorMsg"];
                            }
                        }
                        else
                        {
                            errorGrid.Visibility = Visibility.Visible;
                            errormessage.Text = response.ReasonPhrase + "! " + (string)Application.Current.Resources["key_ActivatedAllreadyErrorMsg"];
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    errorGrid.Visibility = Visibility.Visible;
                        //    errormessage.Text = ex.InnerException.InnerException.Message;
                        //}
                    }
                }
                else
                {
                    errorGrid.Visibility = Visibility.Visible;
                    errormessage.Text = (string)Application.Current.Resources["invalid_LicenseMsg"];
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.txtlicenseKey.Text = "";
            this.Close();

        }
        private string[] GetDecryptLicense(string licnese)
        {
            string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licnese), true);
            if (modifylicense != null)
                return modifylicense.Split('-');
            else
                return null;
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
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }
    }
}

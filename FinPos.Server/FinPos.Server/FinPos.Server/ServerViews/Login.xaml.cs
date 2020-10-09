using FinPos.Server.ServerViews;
using FinPos.Utility.CommonMethods;
using FinPOS.DomainContracts.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using FinPosCrm.Utility.Enums;
using FinPos.Utility.CommonEnums;
using System.Diagnostics;
using System.IO;
using NLog;

namespace FinPos.Server.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = "Login";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Login()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            bool isExpired = CommonFunctions.IsKeyExpired(Convert.ToString(CommonEnums.Edition.Server), Convert.ToString(CommonEnum.IndustryTypes.RS));
            bool IsLicenseExist = CommonFunctions.IsLicenseActivate(Convert.ToString(CommonEnums.Edition.Server), Convert.ToString(CommonEnum.IndustryTypes.RS));
            if (isExpired)
            {
                msg = "Your Key has been expired";
                ConfirmationPopup form = new ConfirmationPopup(msg, header);
                form.ShowDialog();
                this.Close();

            }
            else if (IsLicenseExist)
            {

                //string FinposBasePath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString()).FullName;

                string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                //string clientApplicationPath = FinposBasePath + @"\Finpos Client\Finpos.Client.exe";
                //string clientApplicationPath = FinposBasePath + @"\Finpos.Client.exe";
                //Process process = Process.Start(clientApplicationPath);
                string LocalPath = @"D:\Nishant\ProjectCode\FINPOS_Desktop_10_5_17\branches\FinPos.Desktop\FinPos.Client\FinPos.Desktop\bin\Debug\Finpos.Client.exe";
                Process process = Process.Start(LocalPath);
                this.Close();
            }
            else
            {
                Uri uri = new Uri("/ResourceFiles/uk.xaml", UriKind.RelativeOrAbsolute);
                var resDict = Application.LoadComponent(uri) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(resDict);
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }

        private void LoginUser()
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                errorGrid.Visibility = Visibility.Visible;
                errormessage.Text = "All Fields are required";
            }
            else
            {
                //try
                //{
                LoginModel login = new LoginModel(txtEmail.Text, txtPassword.Password, false);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_baseUrl);

                HttpResponseMessage response = client.PostAsJsonAsync("Account/customerValidate", login).Result;
                if (response.IsSuccessStatusCode)
                {
                    var accessToken = response.Content.ReadAsStringAsync().Result;
                    dynamic token = JsonConvert.DeserializeObject(accessToken);
                    accessToken = token["access_token"];
                    var macadress = CommonFunctions.GetMacAddress();
                    this.Hide();
                    LicenseModel license = new LicenseModel(String.Empty, Convert.ToString(macadress), Convert.ToString(accessToken));
                    License form = new License(license);
                    form.Show();
                    this.Close();
                }
                else
                {
                    errorGrid.Visibility = Visibility.Visible;
                    errormessage.Text = response.ReasonPhrase;
                }
                //}
                //catch (DBConcurrencyException ex)
                //{
                //    errorGrid.Visibility = Visibility.Visible;
                //    errormessage.Text = ex.InnerException.InnerException.Message;
                //    Common.CommonFunctions.SaveErrorLog(ex.InnerException.Message);
                //}
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
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

        //public static bool checkDB_Conn()
        //{
        //    var conn_info = "server=localhost;port=3306;database=finpos;uid='" + Common.CommonProperties.mySqlUser + "';password='" + Common.CommonProperties.mySqlPassword + "'";
        //    bool isConn = false;
        //    MySqlConnection conn = null;
        //    try
        //    {
        //        conn = new MySqlConnection(conn_info);
        //        conn.Open();
        //        isConn = true;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        isConn = false;
        //        switch (ex.Number)
        //        {
        //            //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
        //            case 1042: // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
        //                break;
        //            case 0: // Access denied (Check DB name,username,password)
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    return isConn;
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
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

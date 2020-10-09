using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.Server.ServerControllers;
using FinPos.Server.Views;
using FinPos.Utility.CommonMethods;
using FinPOS.DomainContracts.Model;
using FinPosCrm.Utility.Enums;
using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace FinPos.Server.ServerViews
{
    /// <summary>
    /// Interaction logic for License1.xaml
    /// </summary>
    public partial class License : Window
    {
        public LicenseModel _AccessToken;
        public string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = "License";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //public License()
        //{
        //    InitializeComponent();


        //}
        public License(LicenseModel license)
        {
            InitializeComponent();
            this._AccessToken = license;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RegesterUser();
        }

        private void RegesterUser()
        {
            string filepath = @"c:\Exception\error.txt";  //Text File Path

            if (!Directory.Exists(@"c:\Exception"))
            {
                Directory.CreateDirectory(@"c:\Exception");
            }
            //Text File Name
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            if (string.IsNullOrEmpty(txtlicenseKey.Text))
            {
                errorGrid.Visibility = Visibility.Visible;
                errormessage.Text = "Please enter license Key";
            }
            else
            {
                this.Hide();
                HttpClient client = new HttpClient();
                string[] licenseEdition = GetDecryptLicense(txtlicenseKey.Text);
                if (licenseEdition != null)
                {
                    if (licenseEdition[5] != Convert.ToString(CommonEnums.Edition.Server))
                    {
                        errorGrid.Visibility = Visibility.Visible;
                        errormessage.Text = "Invalid server key, enter valid key";
                    }
                    else
                    {

                        //try
                        //{
                        LicenseModel data = new LicenseModel(txtlicenseKey.Text, this._AccessToken.MacAddress, this._AccessToken.AccessToken);
                        client.BaseAddress = new Uri(_baseUrl);
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = client.PostAsJsonAsync("Account/licenskey", data).Result;
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            var license = response.Content.ReadAsStringAsync().Result;
                            dynamic token = JsonConvert.DeserializeObject(license);
                            var accessToken = token["AccessToken"];
                            var licenseKey = token["LicenseKey"];
                            var generatedLicenseKey = new LicenseModel();
                            string[] modifylicense = GetDecryptLicense(Convert.ToString(licenseKey));
                            string edition = modifylicense[5];
                            string plantype = modifylicense[1];
                            string industryType = modifylicense[3];
                            CommonFunctions obj = new CommonFunctions(licenseKey, accessToken, this._AccessToken.MacAddress);
                            var isSaved = obj.createRegistry(edition, Convert.ToInt32(plantype), Convert.ToInt32(industryType));
                            if (isSaved)
                            {
                                var encodedJwt = new JwtSecurityTokenHandler().ReadToken(Convert.ToString(accessToken)) as JwtSecurityToken;
                                var payloadData = encodedJwt.Payload;
                                string[] username = payloadData["UserName"].ToString().Split(' ');
                                string firstName = username[0];
                                string lastname = username[1];
                                string emailid = payloadData["UserEmail"].ToString();
                                string password = payloadData["Password"].ToString();
                                UserModel user = new UserModel(0, 1000, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), firstName, lastname, true, emailid, password, true, null, string.Empty, string.Empty, 1);
                                UserController userController = new UserController();
                                userController.SaveUpdateUser(user);

                                CompanyModel model = new CompanyModel(0, "Default Company", string.Empty, string.Empty, null, true, true, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), null, null, null);

                                CompanyController controller = new CompanyController();
                                int CompanyId = controller.SaveUpdateCompany(model);


                                BranchModel branchModel = new BranchModel(0, CompanyId, "Default Branch", string.Empty, string.Empty, true, true, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), null, null, null);

                                controller.SaveUpdateBranch(branchModel);
                                string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                                string clientApplicationPath = FinposBasePath + @"\Finpos.Client.exe";
                                Process process = Process.Start(clientApplicationPath);
                                //string LocalPath = @"D:\Nishant\ProjectCode\FINPOS_Desktop_10_5_17\branches\FinPos.Desktop\FinPos.Client\FinPos.Desktop\bin\Debug\Finpos.Client.exe";
                                //Process process = Process.Start(LocalPath);

                                this.Close();
                            }
                            else
                            {
                                errorGrid.Visibility = Visibility.Visible;
                                errormessage.Text = "Key Already activated";
                            }
                        }
                        else
                        {
                            errorGrid.Visibility = Visibility.Visible;
                            errormessage.Text = response.ReasonPhrase + "! " + "Key Already activated";
                        }

                        //}
                        //catch (Exception ex)
                        //{

                        //    LogError.LogErrorMessage("Server Licence.xaml.cs: client application path", ex.StackTrace);
                        //    errorGrid.Visibility = Visibility.Visible;
                        //    errormessage.Text = ex.InnerException.InnerException.Message;
                        //}

                    }
                }
                else
                {
                    errorGrid.Visibility = Visibility.Visible;
                    errormessage.Text = "Invalid License key";
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
            string modifylicense = FinPosCrm.Utility.ExtensionMethods.Decrypt(Convert.ToString(licnese), true);
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

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                RegesterUser();
            }
        }
    }
}

using FinPos.Client.Controllers;
using FinPos.Client.View;
using FinPos.Domain.DataContracts;
using FinPos.Utility.Constants;
using FinPOS.DomainContracts.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinPos.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private readonly ICompanyInvoker companyInvoker;
        public MainWindow()
        {
            InitializeComponent();
            //
            //  this.companyInvoker = new CompanyInvoker();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (txtEmail.Text.Length == 0)
            {
                flag = false;
               // errormessage.Text = "Enter an email.";
                txtEmail.BorderThickness = new Thickness(2);
                txtEmail.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                txtEmail.BorderThickness = new Thickness(0);
            }
            if (txtPassword.Password.Length == 0)
            {
                flag = false;
               // errormessage.Text = "Password not empty!";
                txtPassword.BorderThickness = new Thickness(2);
                txtPassword.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                txtPassword.BorderThickness = new Thickness(0);
            }
            if (!Regex.IsMatch(txtEmail.Text, CommonConstants._emailValidation))
            {
                flag = false;
              //  errormessage.Text = "Enter a valid email.";
                txtEmail.BorderThickness = new Thickness(2);
                txtEmail.BorderBrush = System.Windows.Media.Brushes.Red;
                txtEmail.Select(0, txtEmail.Text.Length);
            }
            else
            {
                txtEmail.BorderThickness = new Thickness(0);
            }
            if (flag)
            {
                LoginModel login = new LoginModel(txtEmail.Text, txtPassword.Password, false);
                HttpClient client = new HttpClient();
                StringContent queryString = new StringContent(login.ToString());
                client.BaseAddress = new Uri("http://localhost:11556/#!/");

                HttpResponseMessage response = client.PostAsJsonAsync("Account/customerValidate", login).Result;


                if (response.IsSuccessStatusCode)
                {
                    var accessToken = response.Content.ReadAsStringAsync().Result;
                    dynamic token = JsonConvert.DeserializeObject(accessToken);
                    accessToken = token["access_token"];
                    var encodedJwt = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
                    var payloadData = encodedJwt.Payload;

                    var macadress = GetMacAddress();
                    this.Hide();

                    LicenseModel license = new LicenseModel(null, macadress.ToString(), accessToken);
                    License form = new License(license);

                    form.ShowDialog();
                    this.Close();
                }
                else
                {
                   // errormessage.Text = response.ReasonPhrase;
                }
            }
        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

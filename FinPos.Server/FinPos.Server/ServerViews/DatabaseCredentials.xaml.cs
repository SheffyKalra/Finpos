using FinPOS.DomainContracts.Model;
using System.Windows;

namespace FinPos.Server.ServerViews
{
    /// <summary>
    /// Interaction logic for DatabaseCredentials.xaml
    /// </summary>
    public partial class DatabaseCredentials : Window
    {
        private LicenseModel _license;
        public DatabaseCredentials()
        {
            InitializeComponent();
        }
        public DatabaseCredentials(LicenseModel license)
        {
            InitializeComponent();
            this._license = license;
        }
        //public bool check_connection(string conn)
        //{
        //    bool result = false;
        //    MySqlConnection connection = new MySqlConnection(conn);
        //    try
        //    {
        //        connection.Open();
        //        result = true;
        //        connection.Close();
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    return result;

        //}

        private void saveDbCredentials_click(object sender, RoutedEventArgs e)
        {
            string connection = "SERVER=localhost;UID='" + txtDbUser.Text + "';password='" + txtPassword.Password + "'";
            MySqlConnection Connection = new MySqlConnection(connection);
            var isConnectionExist = check_connection(connection);
            if (isConnectionExist)
            {
                using (MySqlCommand Command = new MySqlCommand("CREATE USER IF NOT EXISTS '" + Common.CommonProperties.mySqlUser + "'@'localhost' IDENTIFIED BY '" + Common.CommonProperties.mySqlPassword + "';", Connection))
                {
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                using (MySqlCommand cmd = new MySqlCommand("GRANT ALL ON finpos.* TO '" + Common.CommonProperties.mySqlUser + "'@'localhost';", Connection))
                {
                    cmd.ExecuteNonQuery();
                    Connection.Close();
                }
                this.Hide();
                LicenseModel license = new LicenseModel(string.Empty, _license.MacAddress.ToString(), _license.AccessToken);
                License form = new License(license);

                form.ShowDialog();
                
            }
            else
            {
                errorGrid.Visibility = Visibility.Visible;
                errormessage.Text = "Invalid credential! please enter valid credentials...";
            }
          //  this.Close();

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

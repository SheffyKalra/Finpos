using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using Microsoft.Win32;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddCompany.xaml
    /// </summary>
    public partial class AddCompany : Page
    {
        #region Properties
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = (string)Application.Current.Resources["company_Title"];
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region Constructor
        public AddCompany()
        {
            InitializeComponent();
            ChangeHeightWidth();
            txtName.Text = string.Empty;
            txt_cmpDes.Text = string.Empty;
            txt_cmpPhone.Text = string.Empty;
            check_Status.IsChecked = true;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        #endregion
        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.AddCompanyPage.Height = HeightWidth.Height - 65;
            this.AddCompanyPage.Width = HeightWidth.width;

        }
        public void ClearFields()
        {
            txtName.Text = "";
            txt_cmpDes.Text = "";
            txt_cmpPhone.Text = "";
            check_IsDefault.IsChecked = false;
            CompanyDemoLogo.Visibility = Visibility.Visible;
            CompanyLogo.Visibility = Visibility.Hidden;
        }
        public void NavigateToBackPage()
        {
            Company company = new Company();
            NavigationService.Navigate(company);
        }
        #endregion
        #region CRUD Operation
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                Common.ErrorMessage((string)Application.Current.Resources["company_RequiredMsg"], header);
            else
            {
                try
                {
                    CompanyController controller = new CompanyController();
                    var IsCompnayExist = controller.GetCompanies().Any(x => x.Name.ToLower() == txtName.Text.ToLower());
                    if (IsCompnayExist)
                    {
                        msg = "Company'" + txtName.Text + "'already exist";
                        Common.ErrorMessage(msg, header);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txt_cmpPhone.Text) && txt_cmpPhone.Text.Length < 10)
                        {
                            Common.ErrorMessage((string)Application.Current.Resources["company_MobileErrorMsg"], header);
                            return;
                        }
                        if (CompanyLogo.Source != null)
                        {
                            SaveImageFile(CompanyLogo.Source.ToString());
                        }


                        CompanyModel model = new CompanyModel(0, txtName.Text, txt_cmpDes.Text, txt_cmpPhone.Text, System.IO.Path.GetFileName(CompanyLogo.Source.ToString()), check_IsDefault.IsChecked.Value, check_Status.IsChecked.Value, CommonFunctions.ParseDateToFinclaveString(DateTime.UtcNow.ToShortDateString()), null, "", "");
                        controller.SaveUpdateCompany(model);
                        Common.Notification((string)Application.Current.Resources["company_SaveSuccessMsg"], header, false);
                        ClearFields();
                        Window yourParentWindow = Window.GetWindow(this);
                        if (yourParentWindow.GetType().Name == "Main")
                        {
                            var page = yourParentWindow as Main;
                            page.BindCompanyCMBFiltered();
                        }
                        NavigateToBackPage();
                    }
                }
                catch (Exception ex)
                {
                    Common.ErrorMessage(ex.Message, header);
                }
            }
        }


        private void btn_uploadlogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = (string)Application.Current.Resources["company_imagePngType"]; 
            dlg.Filter = (string)Application.Current.Resources["company_CompareImageType"]; 

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string path2 = SaveImageFile(dlg.FileName);
                CompanyLogo.Source = new BitmapImage(new Uri(path2));
                //CompanyLogo.Source = b;
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }


        private static string SaveImageFile(string filePath)
        {
            string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
            string path = FinposBasePath + @"\FinPosImageDocument";
            string fileName = System.IO.Path.GetFileName(filePath);

            // Create directory temp if it doesn't exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path2 = System.IO.Path.Combine(path, fileName);
            if (fileName != (string)Application.Current.Resources["add_Company_Image_Name"])
            {
                if (!File.Exists(path2))
                {
                    File.Copy(filePath, path2);
                }
            }
            return path2;
        }


        private void txt_cmpPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            NavigateToBackPage();
        }

        private void check_IsDefault_Checked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked.Value)
            {
                tbMessage.Text = (check_Status.IsChecked.Value) ? Convert.ToString(Application.Current.Resources["company_ConformationMsgWithDefaultCompany"]): Convert.ToString(Application.Current.Resources["company_ConformationMsgWithStatusCompany"]);
                messagePopUp.IsOpen = true;
                btn_No.Visibility = Visibility.Visible;
                btn_Yes.Visibility = Visibility.Visible;
            }
        }

        private void btn_removelogo_Click(object sender, RoutedEventArgs e)
        {
            CompanyDemoLogo.Visibility = Visibility.Visible;
            CompanyLogo.Source = CompanyDemoLogo.Source;
            CompanyLogo.Visibility = Visibility.Hidden;
        }


        private void txt_cmpPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
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

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = false;
            messagePopUp.IsOpen = false;
            if (btn_Yes.Visibility != Visibility.Collapsed)
                setStatus();
        }

        private void btn_Yes_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = true;
            messagePopUp.IsOpen = false;
            setStatus();
        }

        private void btn_No_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = false;
            messagePopUp.IsOpen = false;
            setStatus();
        }
        public void setStatus()
        {
            check_Status.IsChecked = Common._isChecked;
            check_IsDefault.IsChecked = Common._isChecked;
        }

        private void check_Status_Unchecked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked == true)
            {
                btn_Yes.Visibility = Visibility.Collapsed;
                btn_No.Visibility = Visibility.Collapsed;
                tbMessage.Text = (string)Application.Current.Resources["company_ErrorActiveInactiveCompany"];
                messagePopUp.IsOpen = true;
                Common._isChecked = true;
            }
            else
                Common._isChecked = false;
            setStatus();
        }

        private void txt_cmpPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.Integer);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }
        #endregion
    }
}

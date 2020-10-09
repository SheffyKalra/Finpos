using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.Domain.DataContracts;
using FinPos.Utility.CommonMethods;
using Microsoft.Win32;
using NLog;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditCompany.xaml
    /// </summary>
    public partial class EditCompany : Page
    {
        #region Properties
        public int RowId;
        public bool IsDefault;
        public dynamic seletecRow;
        private int _noOfErrorsOnScreen = 0;
        private string header = "EditCompany";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CompanyController controller = new CompanyController();
        #endregion

        #region Constructor
        public EditCompany(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            SetText(row);
        }
        #endregion

        #region Common Methods
        public void setStatus()
        {
            check_Status.IsChecked = Common._isChecked;
            check_IsDefault.IsChecked = Common._isChecked;
        }
        private void UpdateCompanyCombo()
        {
            Window yourParentWindow = Window.GetWindow(this);
            if (yourParentWindow.GetType().Name == "Main")
            {
                var page = yourParentWindow as Main;
                page.BindCompanyCMBFiltered();
            }
        }
        private void ToggleBtnYesNo(Visibility btnNo, Visibility btnyes)
        {
            messagePopUp.IsOpen = true;
            btn_No.Visibility = btnNo;
            btn_Yes.Visibility = btnyes;
        }
        private void SetText(dynamic row)
        {
            seletecRow = row;
            txtName.Text = row.Name;
            txt_cmpPhone.Text = row.PhoneNo;
            check_Status.IsChecked = row.IsActive;
            txt_cmpDes.Text = row.Description;
            check_IsDefault.IsChecked = row.IsDefault;
            lblEditCompany.Content = "Edit (" + row.Name + ")";
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            SetCompanyLogo(row);
            RowId = row.Id;
            IsDefault = row.IsDefault;
        }

        private void SetCompanyLogo(dynamic row)
        {
            if (!string.IsNullOrWhiteSpace(row.Logo))
            {
                string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + @"\FinPosImageDocument";
                if (Path.GetFileName(row.Logo) != (string)Application.Current.Resources["add_Company_Image_Name"])
                {
                    if (Directory.Exists(path))
                    {
                        FileInfo file = new FileInfo(path + "\\" + row.Logo);
                        if (file.Exists)
                        {
                            BitmapImage logo = new BitmapImage();
                            logo.BeginInit();
                            logo.UriSource = new Uri(path + "\\" + row.Logo);
                            logo.EndInit();
                            CompanyLogo.Source = logo;
                            CompanyLogo.Visibility = Visibility.Visible;
                            CompanyDemoLogo.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }
        private static string SaveImageFile(string filePath)
        {
            string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
            string path = FinposBasePath + @"\FinPosImageDocument";
            string fileName = System.IO.Path.GetFileName(filePath);
            if (!Directory.Exists(path + '\\' + fileName))
                Directory.CreateDirectory(path);

            string finalImagePath = System.IO.Path.Combine(path, fileName);
            if (fileName != (string)Application.Current.Resources["add_Company_Image_Name"])
            {
                if (!File.Exists(finalImagePath))
                {
                    File.Copy(filePath, finalImagePath);
                }
            }
            return finalImagePath;
        }
        private void GoToBackPage()
        {
            Company _Company = new Company();
            NavigationService.Navigate(_Company);
        }
        public void ChangeHeightWidth()
        {
            this.EditCompanyPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.EditCompanyPage.Width = HeightWidth.width;

        }
        #endregion

        #region Events
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txt_cmpPhone.Text) && txt_cmpPhone.Text.Length < 10)
            {
                Common.ShowConfirmationPopup((string)Application.Current.Resources["add_Company_Phone_ErrorMessage"], header, false);
                return;
            }
            if (CompanyLogo.Source != null)
                SaveImageFile(CompanyLogo.Source.ToString());
            CompanyModel model = new CompanyModel(RowId, txtName.Text, txt_cmpDes.Text, txt_cmpPhone.Text, System.IO.Path.GetFileName(CompanyLogo.Source.ToString()), check_IsDefault.IsChecked.Value, check_Status.IsChecked.Value, Convert.ToString(seletecRow.CreatedDate), CommonFunctions.ParseDateToFinclaveString(DateTime.UtcNow.ToShortDateString()), "", "");
            controller.SaveUpdateCompany(model);
            Common.Notification((string)Application.Current.Resources["company_SuccessMsg"], header, false);
            this.Visibility = Visibility.Visible;
            UpdateCompanyCombo();
            GoToBackPage();

        }
        private void btn_uploadlogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = (string)Application.Current.Resources["SupportedfileFormates"];

            if (dlg.ShowDialog() == true)
            {
                string finalImagePath = SaveImageFile(dlg.FileName);
                CompanyLogo.Source = new BitmapImage(new Uri(finalImagePath));
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }
        private void check_IsDefault_Checked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked.Value)
            {
                if (check_Status.IsChecked.Value)
                {
                    tbMessage.Text = (string)Application.Current.Resources["company_ConformationMsg"];
                    ToggleBtnYesNo(Visibility.Visible, Visibility.Visible);
                }
                else
                {
                    tbMessage.Text = (string)Application.Current.Resources["company_setDefaultMsg"];
                    ToggleBtnYesNo(Visibility.Visible, Visibility.Visible);
                }
            }
            else
            {
                ToggleBtnYesNo(Visibility.Collapsed, Visibility.Collapsed);
                tbMessage.Text = (string)Application.Current.Resources["company_setInactiveActiveMsg"];
                Common._isChecked = true;
                setStatus();
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = false;
            messagePopUp.IsOpen = false;
            if (btn_Yes.Visibility != Visibility.Collapsed)
            {
                setStatus();
            }
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
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            GoToBackPage();
        }
        private void btn_removelogo_Click(object sender, RoutedEventArgs e)
        {
            CompanyDemoLogo.Visibility = Visibility.Visible;
            CompanyLogo.Source = CompanyDemoLogo.Source;
            CompanyLogo.Visibility = Visibility.Hidden;
        }

        private void txt_cmpPhone_KeyUp(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = FinPos.Utility.CommonMethods.Validations.StringHasSpace(ke);
        }

        private void txt_cmpPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void txt_cmpPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = Utility.CommonMethods.Validations.StringHasSpace(ke);
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

        private void check_Status_Unchecked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked == true)
            {

                ToggleBtnYesNo(Visibility.Collapsed, Visibility.Collapsed);
                tbMessage.Text = (string)Application.Current.Resources["company_setInactiveActiveMsg"];
                Common._isChecked = true;
                setStatus();
            }
            else
            {
                Common._isChecked = false;
                setStatus();
            }
        }

        private void txt_cmpPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.Integer))
                textBox.Text = string.Empty;
        }
        #endregion
    }
}

using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditBranch.xaml
    /// </summary>
    /// 
    public partial class EditBranch : Page
    {
        #region Properties
        public int RowId;
        public int CompanyId;
        public bool IsDefault;
        public dynamic seletecRow;
        private int _noOfErrorsOnScreen = 0;
        private string header = "Branch";
        private string _companyName = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        public EditBranch(dynamic row, string companyName)
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            SetText(row, companyName);
        }
        #endregion

        #region Common Methods
        private void SetText(dynamic row, string companyName)
        {
            _companyName = companyName;
            seletecRow = row;
            txtName.Text = row.Name;
            txt_address.Text = row.Address;
            txt_Description.Text = row.Description;
            check_Status.IsChecked = row.IsActive;
            check_IsDefault.IsChecked = row.IsDefault;
            this.CompanyId = row.CompanyId;
            RowId = row.Id;
            lblBranchHeading.Content = "Edit (" + row.Name + ")";
        }
        public void ChangeHeightWidth()
        {
            this.EditBranchPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.EditBranchPage.Width = HeightWidth.width;
        }
        private void SaveBranch(CompanyController controller)
        {
            BranchModel model = new BranchModel(RowId, CompanyId, txtName.Text, txt_Description.Text, txt_address.Text, check_IsDefault.IsChecked.Value, check_Status.IsChecked.Value, seletecRow.CreatedDate, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), "", "");
            controller.SaveUpdateBranch(model);
            Common.Notification((string)Application.Current.Resources["branch_UpdateSuccess"], header, false);
            UpdateCompanyCombo();
            NavigateToParent();
        }

        private void NavigateToParent()
        {
            ViewBranch viewBranch = new ViewBranch(CompanyId, _companyName);
            NavigationService.Navigate(viewBranch);
        }

        private void UpdateCompanyCombo()
        {
            Window yourParentWindow = Window.GetWindow(this);
            if (yourParentWindow.GetType().Name == "Main")
            {
                var page = yourParentWindow as Main;
                page.BindBranchCMBFiltered(CompanyId.ToString(), txtName.Text);
            }
        }
        public void setStatus()
        {
            check_Status.IsChecked = Common._isChecked;
            check_IsDefault.IsChecked = Common._isChecked;
        }
        #endregion

        #region Events
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyController controller = new CompanyController();
                ResponseVm response = controller.GetCompanyBranches(CompanyId);//.Where(x => x.Id != RowId).Any(x => x.Name.ToLower() == txtName.Text.ToLower());
                if (response.FaultData == null)
                {
                    if (response.Response.Where(x => x.Id != RowId).Any(x => x.Name.ToLower() == txtName.Text.ToLower()))
                        Common.ErrorMessage("Branch'" + txtName.Text + "' already exist", header);
                    else
                        SaveBranch(controller);
                }
                else
                    Common.ErrorMessage(response.FaultData.Detail.ErrorDetails, header);

            }
            catch (Exception ex)
            {
                Common.ErrorMessage(ex.Message, header);
            }
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ViewBranch back = new ViewBranch(this.CompanyId, _companyName);
            NavigationService.Navigate(back);
        }

        private void check_IsDefault_Checked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked.Value)
            {
                tbMessage.Text = check_Status.IsChecked.Value ? (string)Application.Current.Resources["branch_DefaultBranchMessage"] : (string)Application.Current.Resources["branch_DefaultBranchStatusActiveMessage"];
                messagePopUp.IsOpen = true;
                btn_Yes.Visibility = Visibility.Visible;
                btn_No.Visibility = Visibility.Visible;
            }
            else
            {
                tbMessage.Text = (string)Application.Current.Resources["branch_DefaultBranchErrorMessage"];
                btn_Yes.Visibility = Visibility.Collapsed;
                btn_No.Visibility = Visibility.Collapsed;
            }
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
            {
                setStatus();
            }
        }

        private void btn_No_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = false;
            messagePopUp.IsOpen = false;
            setStatus();
        }

        private void btn_Yes_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = true;
            messagePopUp.IsOpen = false;
            setStatus();
        }
        private void check_Status_Unchecked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked == true)
            {
                btn_Yes.Visibility = Visibility.Collapsed;
                btn_No.Visibility = Visibility.Collapsed;
                tbMessage.Text = (string)Application.Current.Resources["branch_InActiveBracnhMessage"];
                messagePopUp.IsOpen = true;
                Common._isChecked = true;
            }
            else
                Common._isChecked = false;
            setStatus();
        }
        #endregion
    }
}

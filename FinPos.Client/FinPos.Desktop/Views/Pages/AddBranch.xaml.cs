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
    /// Interaction logic for AddBranch.xaml
    /// </summary>
    public partial class AddBranch : Page
    {
        #region Properties
        int companyId;
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = (string)Application.Current.Resources["_branchHeader"];
        private string _companyName = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region Constructor
        public AddBranch(dynamic id, string companyName)
        {
            companyId = id;
            InitializeComponent();
            ChangeHeightWidth();
            ClearFields();
            _companyName = companyName;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        #endregion
        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.AddBranchPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddBranchPage.Width = HeightWidth.width;
        }
        private void ClearFields()
        {
            txtName.Text = "";
            txt_Description.Text = "";
            txt_address.Text = "";
            check_IsDefault.IsChecked = false;

        }
        public void NaviagteTOBackPage()
        {
            ViewBranch viewBranch = new ViewBranch(companyId, _companyName);
            NavigationService.Navigate(viewBranch);
        }
        public void setStatus()
        {
            check_Status.IsChecked = Common._isChecked;
            check_IsDefault.IsChecked = Common._isChecked;
        }

        #endregion
        #region CDUD Opertion
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            NaviagteTOBackPage();
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txt_address.Text))
            {
                Common.ErrorMessage((string)Application.Current.Resources["branchRequiredFields"], header);
            }
            else
            {
                try
                {
                    CompanyController controller = new CompanyController();
                    ResponseVm response = controller.GetCompanyBranches(companyId);//.Any(x => x.Name.ToLower() == txtName.Text.ToLower());
                    if (response.FaultData == null)
                    {
                        var IsBranchExist = response.Response.Any(x => x.Name.ToLower() == txtName.Text.ToLower());
                        if (IsBranchExist)
                        {
                            msg = "Branch'" + txtName.Text + "' already exist";
                            Common.ErrorMessage(msg, header);
                        }
                        else
                        {
                            BranchModel model = new BranchModel(0, companyId, txtName.Text, txt_Description.Text, txt_address.Text, check_IsDefault.IsChecked.Value, true, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), null, "", "");
                            controller.SaveUpdateBranch(model);
                            Window yourParentWindow = Window.GetWindow(this);
                            if (yourParentWindow.GetType().Name == "Main")
                            {
                                var page = yourParentWindow as Main;
                                page.BindBranchCMBFiltered(companyId.ToString(), txtName.Text);
                            }
                            Common.Notification((string)Application.Current.Resources["branch_AddedSuccess"], header, false);
                            ClearFields();
                            NaviagteTOBackPage();
                        }
                    }
                    else
                        Common.ErrorMessage(response.FaultData.Detail.ErrorDetails, header);
                }
                catch (Exception ex)
                {
                    Common.ErrorMessage(ex.Message, header);
                }
            }
        }

        private void check_IsDefault_Checked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked.Value)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["branch_ConfirmationMsg"], header, true);
                form.ShowDialog();
                check_IsDefault.IsChecked = Common._isChecked;
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

        private void check_Status_Unchecked(object sender, RoutedEventArgs e)
        {
            if (check_IsDefault.IsChecked == true)
            {
                btn_Yes.Visibility = Visibility.Collapsed;
                btn_No.Visibility = Visibility.Collapsed;
                tbMessage.Text = (string)Application.Current.Resources["branch_ConfirmationMsgForActiveInactive"];
                messagePopUp.IsOpen = true;
                Common._isChecked = true;
            }
            else
                Common._isChecked = false;
            setStatus();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Common._isChecked = false;
            messagePopUp.IsOpen = false;
            if (btn_Yes.Visibility != Visibility.Collapsed)
                setStatus();
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
        #endregion

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Page
    {
        #region Properties
        SupplierController controller = new SupplierController();
        private string msg = string.Empty;
        private string header =(string)Application.Current.Resources["purchase_suplierHeader"];
        private int _noOfErrorsOnScreen = 0;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private bool supplireNameExists = false;
        #endregion
        #region Constructor
        public AddSupplier()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ContentPanel.Height = CommonFunction.Common._containerHeight - 275;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        #endregion
        #region Common Method
        public void ChangeHeightWidth()
        {
            this.AddSupplierPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddSupplierPage.Width = HeightWidth.width;

        }
        private void NavigateBackPage()
        {
            ClearData();
            Supplier form = new Supplier();
            NavigationService.Navigate(form);
        }
        private void ClearData()
        {
            supplier_name.Text = string.Empty;
            sort_name.Text = string.Empty;
            supplier_address.Text = string.Empty;
            supplier_contact_name.Text = string.Empty;
            supplier_telephone.Text = string.Empty;
            supplier_mobile.Text = string.Empty;
            supplier_fax.Text = string.Empty;
            supplier_websiteUrl.Text = string.Empty;
            supplier_email.Text = string.Empty;
            supplier_note.Text = string.Empty;
            supplier_discount.Text = string.Empty;
        }
        #endregion

        #region CRUD Operation
        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (!supplireNameExists)
            {
                if (string.IsNullOrEmpty(supplier_name.Text) || string.IsNullOrEmpty(supplier_address.Text) || string.IsNullOrEmpty(supplier_contact_name.Text) || string.IsNullOrEmpty(supplier_mobile.Text) || string.IsNullOrEmpty(supplier_email.Text))
                    Common.ErrorMessage((string)Application.Current.Resources["commonFieldsError_Msg"], header);
                else
                {
                    decimal? nullval = null;
                    SupplierModel model = new SupplierModel(supplier_name.Text, sort_name.Text, supplier_address.Text, supplier_contact_name.Text, supplier_telephone.Text, supplier_mobile.Text, supplier_fax.Text, supplier_websiteUrl.Text, supplier_email.Text, supplier_note.Text, string.IsNullOrEmpty(supplier_discount.Text) ? nullval : Convert.ToDecimal(supplier_discount.Text), UserModelVm.CompanyId, UserModelVm.BranchId);
                    controller.SaveUpdateSupplier(model);
                    Common.Notification((string)Application.Current.Resources["supplier_SavedSuccessMsg"], header, false);
                    NavigateBackPage();
                }
            }
            else
                Common.ErrorMessage((string)Application.Current.Resources["supplier_exists"], header);
        }
        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            NavigateBackPage();
        }
       

        private void txt_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void AddSupplier_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        private void txt_cmpPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void txt_cmp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.DecimalValueChecker(sender, e);
            if (Regex.IsMatch(supplier_discount.Text, @"\.\d\d"))
            {
                e.Handled = true;
            }
        }

        private void txt_cmpPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void supplier_discount_KeyUp(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
            else if (supplier_discount.Text == ".")
                supplier_discount.Text = string.Empty;
            else if (!string.IsNullOrEmpty(supplier_discount.Text) && Convert.ToDecimal(supplier_discount.Text) > 100)
            {
                Common.ErrorMessage((string)Application.Current.Resources["invalid_DiscountSupplier"],header);
                supplier_discount.Text = string.Empty;
            }
        }

        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (supplier_name.Text != string.Empty)
            {
                var supplierId = controller.GetSupplierIdByName(supplier_name.Text);
                if (supplierId != null)
                {
                    supplier_name.Text = string.Empty;
                    supplireNameExists = true;
                    Common.ErrorMessage((string)Application.Current.Resources["supplier_exists"], header);
                }
                else
                    supplireNameExists = false;
            }
        }
        private void check_IsNumeric(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.Integer);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }
        #endregion
    }
}

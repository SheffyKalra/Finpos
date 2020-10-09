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
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditSupplier.xaml
    /// </summary>
    public partial class EditSupplier : Page
    {
        SupplierController controller = new SupplierController();
        private string msg = string.Empty;
        private string header = "Supplier";
        private int _noOfErrorsOnScreen = 0;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public int RowId;
        private bool supplireNameExists = false;
        public EditSupplier(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            supplier_name.Text = row.SupplierName;
            sort_name.Text = row.ShortName;
            supplier_address.Text = row.Address;
            supplier_contact_name.Text = row.ContactName;
            supplier_telephone.Text = row.Telephone;
            supplier_mobile.Text = row.Mobile;
            supplier_fax.Text = row.Fax;
            supplier_websiteUrl.Text = row.WebsiteUrl;
            supplier_email.Text = row.Email;
            supplier_note.Text = row.Notes;
            supplier_discount.Text = Convert.ToString(row.DiscountPercentage);
            RowId = row.Id;
            lblSupplierName.Content = "Edit (" + row.SupplierName + ")";
        }
        public void ChangeHeightWidth()
        {
            this.EditSupplierPage.Height = HeightWidth.Height - 65;
            this.EditSupplierPage.Width = HeightWidth.width;

        }
        private void ClearData()
        {
            supplier_name.Text = "";
            sort_name.Text = "";
            supplier_address.Text = "";
            supplier_contact_name.Text = "";
            supplier_telephone.Text = "";
            supplier_mobile.Text = "";
            supplier_fax.Text = "";
            supplier_websiteUrl.Text = "";
            supplier_email.Text = "";
            supplier_note.Text = "";
            supplier_discount.Text = "";
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

        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (!supplireNameExists)
            {
                if (string.IsNullOrEmpty(supplier_name.Text) || string.IsNullOrEmpty(supplier_address.Text) || string.IsNullOrEmpty(supplier_contact_name.Text) || string.IsNullOrEmpty(supplier_mobile.Text) || string.IsNullOrEmpty(supplier_email.Text))
                {
                    //   msg = "Please fill the required fields first";
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["error_message_Tax"], header, false);
                    form.ShowDialog();
                    // Common.ErrorNotification((string)Application.Current.Resources["error_message_Tax"],header,false);
                }
                else
                {
                    decimal? nullval = null;
                    SupplierModel model = new SupplierModel(RowId, supplier_name.Text, sort_name.Text, supplier_address.Text, supplier_contact_name.Text, supplier_telephone.Text, supplier_mobile.Text, supplier_fax.Text, supplier_websiteUrl.Text, supplier_email.Text, supplier_note.Text, string.IsNullOrEmpty(supplier_discount.Text) ? nullval : Convert.ToDecimal(supplier_discount.Text), UserModelVm.CompanyId, UserModelVm.BranchId);
                    controller.SaveUpdateSupplier(model);
                    Common.Notification((string)Application.Current.Resources["supplier_UpdatedSuccessMsg"], header, false);
                    navigatePage();
                }
            }
            else
            {
                Common.ErrorMessage((string)Application.Current.Resources["supplier_exists"], header);
            }
        }

        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            navigatePage();
        }
        private void navigatePage()
        {
            ClearData();
            Supplier form = new Supplier();
            NavigationService.Navigate(form);
        }


        private void supplier_discount_KeyUp(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
            else if (supplier_discount.Text == ".")
            {
                supplier_discount.Text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(supplier_discount.Text) && Convert.ToDecimal(supplier_discount.Text) > 100)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["invalid_DiscountSupplier"], header, false);
                form.ShowDialog();
                //  Common.ErrorNotification((string)Application.Current.Resources["invalid_DiscountSupplier"], header, false);
                supplier_discount.Text = string.Empty;
            }
        }

        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (supplier_name.Text != string.Empty)
            {
                var supplierId = controller.GetSupplierIdByName(supplier_name.Text);
                if (supplierId != null && supplierId != RowId)
                {
                    supplier_name.Text = string.Empty;
                    supplireNameExists = true;
                    Common.ErrorMessage((string)Application.Current.Resources["supplier_exists"], header);
                }
                else
                {
                    supplireNameExists = false;
                }
            }
        }

        private void check_IsNumeric(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.Integer);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }
    }
}

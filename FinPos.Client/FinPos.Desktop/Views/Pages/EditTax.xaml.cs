using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditTax.xaml
    /// </summary>
    public partial class EditTax : Page
    {
        TaxController controller = new TaxController();
        private string msg = string.Empty;
        private string header = "Tax";
        private int _noOfErrorsOnScreen = 0;
        private IList<TaxModel> _taxs;
        public int RowId;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public EditTax(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            ResponseVm responce = controller.GetTax();///.ToList();
            if (responce.FaultData == null)
            {
                _taxs = responce.Response.Cast<TaxModel>().ToList();
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, "Fault", false);
                form.ShowDialog();
            }
            tax_Detail.Text = row.TaxDetail;
            tax_Rate.Text = Convert.ToString(row.Rate);
            RowId = row.TaxCode;
        }
        public void ChangeHeightWidth()
        {
            this.EditTaxpage.Height = HeightWidth.Height - 65;
            this.EditTaxpage.Width = HeightWidth.width;
        }
        private void NavigateBackPage()
        {
            ClearData();
            Tax form = new Tax();
            NavigationService.Navigate(form);
        }
        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            Tax form = new Tax();
            NavigationService.Navigate(form);
        }
        private void ClearData()
        {
            tax_Detail.Text = "";
            tax_Rate.Text = "";
        }

        private void txt_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void AddTax_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void txt_decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.DecimalValueChecker(sender, e);
            if (Regex.IsMatch(tax_Rate.Text, @"\.\d\d"))
            {
                e.Handled = true;
            }
        }

        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tax_Detail.Text) || string.IsNullOrEmpty(tax_Rate.Text))
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["error_message_Tax"], header, false);
                form.ShowDialog();
            }
            else
            {
                decimal? nullval = null;
                TaxModel model = new TaxModel(RowId, tax_Detail.Text, Convert.ToDouble(tax_Rate.Text), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty, string.Empty, string.Empty);
                controller.SaveUpdateTax(model);
                Common.Notification((string)Application.Current.Resources["tax_UpdateSuccessMsg"], header, false);
                NavigateBackPage();
            }
        }
        private void tax_Rate_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
            else if (tax_Rate.Text == ".")
            {
                tax_Rate.Text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(tax_Rate.Text) && Convert.ToDouble(tax_Rate.Text) > 100)
            {
                tax_Rate.Text = string.Empty;
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["tax_RateErrorMsg"], header, false);
                form.ShowDialog();
               // Common.ErrorNotification((string)Application.Current.Resources["tax_RateErrorMsg"], header, false);
            }
        }
        private void tax_Detail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_taxs.Any(x => x.TaxDetail.ToLower() == tax_Detail.Text.ToLower()))
            {
                tax_Detail.Text = string.Empty;
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["tax_DetailErrorMsg"], header, false);
                form.ShowDialog();
               // Common.ErrorNotification((string)Application.Current.Resources["tax_DetailErrorMsg"], header, false);
            }
        }

        private void tax_Rate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tax_Detail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

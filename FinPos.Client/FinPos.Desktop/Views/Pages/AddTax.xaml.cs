using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddTax.xaml
    /// </summary>
    public partial class AddTax : Page
    {
        #region Properties
        TaxController controller = new TaxController();
        private IList<TaxModel> _taxes;
        private string msg = string.Empty;
        private string header = "Tax";
        private int _noOfErrorsOnScreen = 0;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion

        #region Constructor
        public AddTax()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ClearData();
            BindTaxes();
        }
        #endregion

        #region Common Methods
        private void BindTaxes()
        {
            ResponseVm responce = controller.GetTax();
            if (responce.FaultData == null)
                _taxes = responce.Response.Cast<TaxModel>().ToList();
            else
                Common.ErrorMessage(responce.FaultData.Detail.ErrorDetails, header);
        }

        public void ChangeHeightWidth()
        {
            this.AddTaxpage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddTaxpage.Width = HeightWidth.width;
        }


        private void NavigateBackPage()
        {
            ClearData();
            Tax form = new Tax();
            NavigationService.Navigate(form);
        }


        private void ClearData()
        {
            tax_Detail.Text = "";
            tax_Rate.Text = "";
        }
        #endregion

        #region Events
        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tax_Detail.Text) || string.IsNullOrEmpty(tax_Rate.Text))
            {
                Common.ErrorMessage((string)Application.Current.Resources["error_message_Tax"], header);
            }
            else
            {
                #region Save
                TaxModel model = new TaxModel(null, tax_Detail.Text, Convert.ToDouble(tax_Rate.Text), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty, string.Empty, string.Empty);
                controller.SaveUpdateTax(model);
                Common.Notification((string)Application.Current.Resources["tax_SaveSuccessMsg"], header, false);
                NavigateBackPage();
                #endregion
            }
        }
        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            Tax form = new Tax();
            NavigationService.Navigate(form);
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
            else if (!string.IsNullOrEmpty(tax_Rate.Text) && Convert.ToDouble(tax_Rate.Text) > Common.PercentMaxValue)
            {
                tax_Rate.Text = string.Empty;
                Common.ErrorMessage((string)Application.Current.Resources["tax_RateErrorMsg"], header);
            }
        }

        private void tax_Detail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_taxes.Any(x => x.TaxDetail.ToLower() == tax_Detail.Text.ToLower()))
            {
                tax_Detail.Text = string.Empty;
                Common.ErrorMessage((string)Application.Current.Resources["tax_DetailErrorMsg"], header);
            }
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion
    }
}

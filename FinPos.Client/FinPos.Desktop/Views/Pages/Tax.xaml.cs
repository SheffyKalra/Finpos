using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.Constants;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Tax.xaml
    /// </summary>
    public partial class Tax : Page
    {
        private IList<TaxModel> _taxs;
        private string msg = string.Empty;
        private string header = "Tax";
        private TaxController controller = new TaxController();
        BrushConverter color = new BrushConverter();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Tax()
        {
            InitializeComponent();

            ChangeHeightWidth();
            ResponseVm responce = controller.GetTax();///.ToList();
            if (responce.FaultData == null)
            {
                _taxs = responce.Response.Cast<TaxModel>().ToList();

                lvTaxs.ItemsSource = _taxs;
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, "Fault", false);
                form.ShowDialog();
            }
            edit_Tax.IsEnabled = false;
            edit_Tax.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
        }
        public void ChangeHeightWidth()
        {
            this.taxpage.Height = HeightWidth.Height - 65;
            this.taxpage.Width = HeightWidth.width;
           
        }
        private void lvTax_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = (sender as ListViewItem);

            if (item != null || item.IsSelected)
            {
                edit_Tax.IsEnabled = true;
                edit_Tax.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                btn_clear.IsEnabled = true;
                btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_Delete.IsEnabled = true;
                btn_Delete.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
            }
        }

        private void btn_Tax_Click(object sender, RoutedEventArgs e)
        {
            AddTax addTax = new AddTax();
            NavigationService.Navigate(addTax);
        }

        private void edit_Tax_Click(object sender, RoutedEventArgs e)
        {
            var row = lvTaxs.SelectedItem;
            EditTax editTax = new EditTax(row);
            NavigationService.Navigate(editTax);
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvTaxs.SelectedItem;
            ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["confirm_message_Tax"], header, true);
            form.ShowDialog();
            if (Common._isChecked)
            {
                controller.DeleteTax(row.TaxCode);
                ResponseVm responce = controller.GetTax();
                _taxs = responce.Response.Cast<TaxModel>().ToList();
                lvTaxs.ItemsSource = _taxs;
                Common.Notification((string)Application.Current.Resources["success_message_Tax"], header, false);
                DisableButtons();
                //ConfirmationPopup form1 = new ConfirmationPopup(msg, header, false);
                //form1.ShowDialog();
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
        }
        private void DisableButtons()
        {
            lvTaxs.SelectedItem = null;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            edit_Tax.IsEnabled = false;
            edit_Tax.Background = Brushes.Gray;
        }
        private void tax_search_GotFocus(object sender, RoutedEventArgs e)
        {
            tax_search.Text = string.Empty;
        }

        private void tax_search_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(tax_search.Text))
            {
                tax_search.Text = "Search";
            }
        }
        private void tax_search_KeyUp(object sender, KeyEventArgs e)
        {
            var text = tax_search.Text.ToLower();
            lvTaxs.ItemsSource = _taxs.Where(x => Convert.ToString(x.TaxCode.Value).Contains(text) || x.TaxDetail.ToLower().Contains(text) || Convert.ToString(x.Rate).Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvTaxs.ItemsSource).Refresh();
            edit_Tax.IsEnabled = false;
            edit_Tax.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            tax_search.Text = string.Empty;
            lvTaxs.ItemsSource = _taxs.Where(x => Convert.ToString(x.TaxCode.Value).Contains(tax_search.Text) || x.TaxDetail.ToLower().Contains(tax_search.Text) || Convert.ToString(x.Rate).Contains(tax_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvTaxs.ItemsSource).Refresh();
            SetTextOnSearch();
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

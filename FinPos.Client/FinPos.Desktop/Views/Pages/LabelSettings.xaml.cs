using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Documents;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for LabelSettings.xaml
    /// </summary>
    public partial class LabelSettings : Page
    {
        public int? Id;
        public int lebelSettingCode;
        public int ItemId;
        public ProductModel Item;
        private string msg = string.Empty;
        private string header = "Label Settings";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ProductController controller = new ProductController();
        public LabelSettings(dynamic row)
        {
            InitializeComponent();
            Item = controller.GetProductById(row.ItemId);
            bar_code_height.ItemsSource = Enum.GetValues(typeof(CommonFunction.Common.barcodeHeight));
            label_sheet_dd.ItemsSource = Enum.GetValues(typeof(CommonFunction.Common.sheetSizes));
           // bar_code_height.SelectedItem= (CommonFunction.Common.barcodeHeight)3;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            GridLengthConverter gridLengthConverter = new GridLengthConverter();
            Id = row.Id != null? row.Id : null ;
            lebelSettingCode = row.LabelSettingCode;
            ItemId = row.ItemId != null? row.ItemId:null ;
           // bar_code_height.Text = row.BarCodeHeight;
            pageLabel.Content =new Bold(new Run("Label Settings  ("+ Item.ItemName+")"));
            chk_print_barcode.IsChecked = row.PrintBarCode;
            chk_item_detail.IsChecked = row.PrintItemDetail;
            print_item_code.IsChecked = row.PrintItemCode;
            //print_unit_measure.Text = row.PrintUnitMeasure;
            lb_print_bc.Content = Item.BarCode;
            lb_print_Id.Content = Item.ItemName;
            lb_print_price.Content = Item.RetailPrice;
            print_item_price.IsChecked = row.PrintItemPrice;
            tb_no_of_prints.Text = row.TotalNoOfPrints;
            nud_start_column.Value = row.StartColumn==null?0: Convert.ToDouble(row.StartColumn);
            nud_start_row.Value = row.StartRow==null?0: Convert.ToDouble(row.StartRow);
        }
       

        private void update_label_data(object sender, RoutedEventArgs e)
        {
            ProductController controller = new ProductController();
            LabelSettingModel model = new LabelSettingModel(Id, lebelSettingCode, ItemId, print_item_code.IsChecked.Value,chk_item_detail.IsChecked.Value,"0", print_item_price.IsChecked.Value, chk_print_barcode.IsChecked.Value, bar_code_height.Text, label_sheet_dd.SelectedValue.ToString(),tb_no_of_prints.Text, nud_start_row.Value.ToString(),nud_start_column.Value.ToString());
            controller.SaveUpdateLabel(model);
            inventory product = new inventory();
            NavigationService.Navigate(product);
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            inventory product = new inventory();
            NavigationService.Navigate(product);
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void print_unit_measure_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void print_item_code_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_Ic.Content = ItemId;
            lb_print_Ic.Visibility = Visibility.Visible;
        }

        private void print_item_code_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_Ic.Visibility = Visibility.Collapsed;
        }

        private void chk_print_barcode_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_bc.Visibility = Visibility.Visible;
        }

        private void chk_print_barcode_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_bc.Visibility = Visibility.Collapsed;
        }

        private void print_item_price_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_price.Visibility = Visibility.Visible;
        }

        private void print_item_price_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_price.Visibility = Visibility.Collapsed;
        }

        private void chk_item_detail_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_Id.Visibility = Visibility.Visible;
        }

        private void chk_item_detail_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_Id.Visibility = Visibility.Collapsed;
        }

        private void label_sheet_dd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bar_code_height_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tb_no_of_prints_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void bar_code_height_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void label_sheet_dd_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

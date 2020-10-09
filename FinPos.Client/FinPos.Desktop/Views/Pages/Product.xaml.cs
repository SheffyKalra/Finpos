using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.Client.Views.UserControls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using FinPos.Utility.Constants;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for inventory.xaml
    /// </summary>
    public partial class inventory : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private ProductController controller = new ProductController();
        private IList<ProductModel> _products;
        private LabelSettingModel _labelSettings;
        private string msg = string.Empty;
        private string header = "Product";
        BrushConverter color = new BrushConverter();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public inventory()
        {
            InitializeComponent();
            ChangeHeightWidth();
            product_search.Text = "Search";
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ResponseVm responce = controller.GetProductsByCompanyAndBranch();///.ToList();

            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                if (_products != null && _products.Count > 0)
                {
                    foreach (ProductModel item in _products)
                    {

                        if (item.ImageText != null && item.ImageText != "")
                        {
                            string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                            string path = FinposBasePath + @"\FinPosImageDocument";
                            if (Path.GetFileName(item.ImageText) != (string)Application.Current.Resources["add_Company_Image_Name"])
                            {
                                if (Directory.Exists(path))
                                {
                                    FileInfo file = new FileInfo(path + "\\" + item.ImageText);
                                    if (file.Exists)
                                    {
                                        BitmapImage logo = new BitmapImage();
                                        logo.BeginInit();
                                        logo.UriSource = new Uri(path + "\\" + item.ImageText);
                                        logo.EndInit();
                                        item.ItemImage = Utility.CommonMethods.CommonFunctions.ImageToByteArray(logo);
                                    }
                                }
                            }
                        }
                    }
                }
                lvProducts.ItemsSource = _products;
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, "Fault", false);
                form.ShowDialog();
            }
            edit_Product.IsEnabled = false;
            edit_Product.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Label.IsEnabled = false;
            btn_Label.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
        }
        public void ChangeHeightWidth()
        {
            this.ProductPage.Height = HeightWidth.Height - 65;
            this.ProductPage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvProducts.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvProducts.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
        private void lvUsers_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);

            if (item != null || item.IsSelected)
            {
                edit_Product.IsEnabled = true;
                edit_Product.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                btn_clear.IsEnabled = true;
                btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_Delete.IsEnabled = true;
                btn_Delete.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_Label.IsEnabled = true;
                btn_Label.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.SelectedItem = null;
            btn_addProducts.IsEnabled = true;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            edit_Product.IsEnabled = false;
            edit_Product.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Label.IsEnabled = false;
            btn_Label.Background = Brushes.Gray;
        }
        private void delete_product_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvProducts.SelectedItem;
            msg = (string)Application.Current.Resources["product_delete_alert"];
            ConfirmationPopup form = new ConfirmationPopup(msg, header, true);
            form.ShowDialog();
            if (Common._isChecked)
            {
                var result = controller.DeleteProduct(row.Id);
                if (result.FaultData == null)
                {
                    ResponseVm response = controller.GetProductsByCompanyAndBranch();//.ToList();
                    if (response.FaultData == null)
                    {
                        _products = response.Response.Cast<ProductModel>().ToList();
                        lvProducts.ItemsSource = _products;
                        msg = (string)Application.Current.Resources["delete_success_alert"];
                        // ConfirmationPopup form1 = new ConfirmationPopup(msg, header, false);
                        //  form1.ShowDialog();
                        Common.Notification(msg, header, false);
                        edit_Product.IsEnabled = false;
                        edit_Product.Background = Brushes.Gray;
                        btn_clear.IsEnabled = false;
                        btn_clear.Background = Brushes.Gray;
                        btn_Label.IsEnabled = false;
                        btn_Label.Background = Brushes.Gray;
                        btn_Delete.IsEnabled = false;
                        btn_Delete.Background = Brushes.Gray;
                    }
                    else
                    {
                        ConfirmationPopup form2 = new ConfirmationPopup(response.FaultData.Detail.ErrorDetails, header, false);
                        form2.ShowDialog();
                    }
                }
                else
                {
                    msg = (string)Application.Current.Resources["product_exist_exeption"];
                    form = new ConfirmationPopup(msg, header, false);
                    form.ShowDialog();
                    // Common.ErrorNotification(msg, header, false);
                }
            }
        }
        private void btn_lebelSetting_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvProducts.SelectedItem;
            _labelSettings = controller.GetLebelDate(row.Id);
            LabelSettings page = new LabelSettings(_labelSettings);
            NavigationService.Navigate(page);
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void btn_addProducts_Click(object sender, RoutedEventArgs e)
        {
            AddProductHistory addProduct = new AddProductHistory();
            NavigationService.Navigate(addProduct);
        }

        private void edit_Product_Click(object sender, RoutedEventArgs e)
        {
            var row = lvProducts.SelectedItem;
            var product = (ProductModel)row;
            bool isExist = controller.IsProductExistInRepack(product.Id.Value);
            if (isExist)
            {
                Common.ErrorMessage((string)Application.Current.Resources["product_alreadyInUsedMsg"], header);
            }
            else
            {
                EditProductHistory editProduct = new EditProductHistory(row);
                // ProductsHistory editProduct = new ProductsHistory();
                NavigationService.Navigate(editProduct);
            }
        }

        private void cmpny_search_KeyUp(object sender, KeyEventArgs e)
        {
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            edit_Product.IsEnabled = false;
            edit_Product.Background = Brushes.Gray;
            btn_Label.IsEnabled = false;
            btn_Label.Background = Brushes.Gray;
            var text = product_search.Text.ToLower();

            lvProducts.ItemsSource = _products.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.ShortName.ToLower().Contains(text) || x.ItemName.ToLower().Contains(text) || x.BarCode.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvProducts.ItemsSource).Refresh();
        }

        private void product_search_GotFocus(object sender, RoutedEventArgs e)
        {
            product_search.Text = string.Empty;
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(product_search.Text))
            {
                product_search.Text = "Search";
            }
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            product_search.Text = string.Empty;
            lvProducts.ItemsSource = _products.Where(x => Convert.ToString(x.Id.Value).Contains(product_search.Text) || x.ShortName.ToLower().Contains(product_search.Text) || x.ItemName.ToLower().Contains(product_search.Text) || x.BarCode.ToLower().Contains(product_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvProducts.ItemsSource).Refresh();
            SetTextOnSearch();
        }
        private void product_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(product_search.Text))
            {
                product_search.Text = "Search";
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvProducts_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvProducts_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}

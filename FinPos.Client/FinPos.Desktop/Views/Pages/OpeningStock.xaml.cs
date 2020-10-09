using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for OpeningStock.xaml
    /// </summary>
    public partial class OpeningStock : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private ProductController controller = new ProductController();
        private IList<ProductModel> _products;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private ObservableCollection<OpeningStockModel> OpeningStocks;
        BrushConverter color = new BrushConverter();
        public OpeningStockModel _selectedStock;
        public int rowIndex = 0;
        private string header = (string)Application.Current.Resources["opening_stock"];
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public OpeningStock()
        {
            InitializeComponent();
            ChangeHeightWidth();
            btn_remove.Background = Brushes.Gray;
            ResponseVm responce = controller.GetProductsByCompanyAndBranch();
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                if (_products != null && _products.Count > 0)
                {
                    dgProducts.ItemsSource = _products;
                    dgProducts.Visibility = Visibility.Visible;
                }
                else
                {
                    dgProducts.Visibility = Visibility.Collapsed;
                    brd_exp.Visibility = Visibility.Visible;
                }
            }

            if (OpeningStocks == null)
            {
                additems();
            }
            lvOpeningStock.ItemsSource = OpeningStocks;




        }
        public void ChangeHeightWidth()
        {
            this.OpeningStockPage.Height = HeightWidth.Height - 65;
            this.OpeningStockPage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvOpeningStock.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvOpeningStock.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
        public void additems()
        {
            OpeningStocks = new ObservableCollection<OpeningStockModel>();
            OpeningStocks.Add(new OpeningStockModel() { ProductName = "" });
        }
        private void btn_addRow_Click(object sender, RoutedEventArgs e)
        {
            OpeningStocks.Add(new OpeningStockModel() { ProductName = "" });
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            List<OpeningStockModel> Stocks = lvOpeningStock.Items.Cast<OpeningStockModel>().Select(x => x).ToList();
            bool IsTrue = Stocks.Where(x => x.ProductCode == 0 || x.Quantity == 0).Any();
            if (IsTrue)
            {
                Common.ErrorMessage((string)Application.Current.Resources["stock_adjustmentRequiredFields"], header);
            }
            else
            {
                foreach (OpeningStockModel stock in Stocks)
                {
                    stock.CreatedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    stock.CompanyCode = UserModelVm.CompanyId;
                    stock.BranchCode = UserModelVm.BranchId;
                    stock.CreatedBy = UserModelVm.UserId;
                }
                OpeningStockController.SaveUpdateOpeningStocks(Stocks);
                Common.Notification((string)Application.Current.Resources["stock_addSuccessMsg"], header, false);
                additems();
                lvOpeningStock.ItemsSource = OpeningStocks;
            }
        }


        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;

            if (element.GetType() == type) return element;

            return GetAncestorByType(VisualTreeHelper.GetParent(element), type);

        }
        public void stock_AddItem(ProductModel itemToAdd)
        {

            if (OpeningStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_already"], "OpeningStock", true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    AddItemSource(itemToAdd);
                }
            }
            else
            {
                AddItemSource(itemToAdd);
            }
            //OpeningStockModel _openingStock = new OpeningStockModel();
            //_openingStock.ProductName = itemToAdd.ItemName;
            //_openingStock.productCode = Convert.ToInt64(itemToAdd.Id);
            //_openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(itemToAdd.Id));
            //OpeningStocks[rowIndex] = _openingStock;
            //lvOpeningStock.ItemsSource = OpeningStocks;
        }
        private void AddItemSource(ProductModel openingModel)
        {
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = openingModel.ItemName;
            _openingStock.productCode = Convert.ToInt64(openingModel.Id);
            _openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(openingModel.Id));
            OpeningStocks[rowIndex] = _openingStock;
            lvOpeningStock.ItemsSource = OpeningStocks;
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            this.IsEnabled = true;
        }


        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            if (productItem != null)
            {
                productPopUp.IsOpen = false;
                this.IsEnabled = true;
                btn_remove.IsEnabled = false;
                btn_remove.Background = Brushes.Gray;
                bool stockExists = OpeningStockController.CheckProductOpningStock(Convert.ToInt64(productItem.Id));
                if (!stockExists)
                {
                    stock_AddItem(productItem);
                }
                else
                {

                    ResourceDictionary myResourceDictionary = new ResourceDictionary();
                    myResourceDictionary.Source =
                    new Uri("/ResourceFiles/En.xaml",
                            UriKind.RelativeOrAbsolute);
                    string errorMessage = (string)myResourceDictionary["error_message_false_product_selection_Opening_Stock"];

                    ConfirmationPopup form = new ConfirmationPopup(errorMessage, header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification(errorMessage, header, false);
                }
            }
        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            //  this.ApplicationBar.IsVisible = false;
            //ProductListPopUp productList = new ProductListPopUp();
            //productList.ShowDialog();
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lvOpeningStock.SelectedIndex =
                    lvOpeningStock.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lvOpeningStock.SelectedIndex;
                _selectedStock = (OpeningStockModel)lvOpeningStock.SelectedItem;
            }
        }

        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgProducts.ItemsSource);
            if (cv == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(filterText))
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    ProductModel p = o as ProductModel;
                    return (p.ItemName.ToUpper().StartsWith(filterText.ToUpper()) || p.BarCode.ToUpper().StartsWith(filterText.ToUpper()) || p.Id.ToString().ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };
            }
            else
            {
                cv.Filter = null;
            }
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (productPopUp.IsOpen == true)
            {
                return;
            }
            OpeningStocks.Remove((OpeningStockModel)lvOpeningStock.SelectedItem);
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
        }

        private void lvOpeningStock_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = (sender as System.Windows.Controls.ListViewItem);
            if (item != null || item.IsSelected)
            {
                btn_remove.IsEnabled = true;
                btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
            }
        }

        // Use the DataObject.Pasting Handler 
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
        #region Search Box
        private void txt_search_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_search.Text = string.Empty;
        }
        private void txt_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            dgProducts.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
            //SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }

        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            dgProducts.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
        }
        #endregion

        private void lvOpeningStock_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as System.Windows.Controls.ListView);
        }

        private void lvOpeningStock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as System.Windows.Controls.ListView);
        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }

}

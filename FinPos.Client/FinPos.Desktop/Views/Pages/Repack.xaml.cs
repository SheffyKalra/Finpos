using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Repack.xaml
    /// </summary>
    public partial class Repack : Page
    {
        ProductController controller = new ProductController();
        private IList<ProductModel> _products;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private ObservableCollection<RepackStockModel> OpeningStocks;
        public RepackStockModel _selectedStock;
        ProductModel selectedBulkProduct = null;
        public int rowIndex = 0;
        public decimal? currentStockofBulk = 0;
        string header = "Repack";
        public ListViewItem item = null;
        BrushConverter color = new BrushConverter();
        public int? bulkCode = null;
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Repack()
        {
            InitializeComponent();
            ChangeHeightWidth();
            // ResponseVm responce = controller.GetProductsByCompanyAndBranch(UserModelVm.CompanyId, UserModelVm.BranchId);
            ResponseVm responce = controller.GetProductsByitemType(UserModelVm.CompanyId, UserModelVm.BranchId, (int)CommonEnum.ItemTypes.RePack);
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            repack_IssueDate.Text = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                if (_products != null && _products.Count > 0)
                {
                    dgProducts.ItemsSource = _products;
                    ///dgProductsBulk.ItemsSource = _products.Where(x => x.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.Bulk));
                   // dgProductsBulk.Visibility = Visibility.Visible;
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
           // btn_remove.Background = Brushes.Gray;
            lvProductDetails.ItemsSource = OpeningStocks;
        }
        public void ChangeHeightWidth()
        {
            this.RepackPage.Height = HeightWidth.Height - 65;
            this.RepackPage.Width = HeightWidth.width;

        }
        private void lvProductDetails_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
        }
        public void additems()
        {
            OpeningStocks = new ObservableCollection<RepackStockModel>();
            OpeningStocks.Add(new RepackStockModel() { ProductName = "" });
        }

        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // selectedBulkProduct = null;
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            bool stockExists = false;// OpeningStockController.CheckProductOpningStock(Convert.ToInt64(productItem.Id));
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

                ConfirmationPopup form = new ConfirmationPopup(errorMessage, "Repack", false);
                form.ShowDialog();
                //  Common.ErrorNotification(errorMessage, header, false);
            }
        }
        public void stock_AddItem(ProductModel itemToAdd)
        {
            if (itemToAdd != null)
            {
                if (OpeningStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
                {
                    ErroMessage((string)Application.Current.Resources["repack_already"], header);
                }
                else if (selectedBulkProduct != null && selectedBulkProduct.Id != itemToAdd.BulkCode)
                {
                    ErroMessage((string)Application.Current.Resources["repack_missMatch_Error"], header);
                }
                else
                {
                    AddItemSource(itemToAdd);
                }
            }
        }
        private CurrentRepackStockVm GetCurrentStock(IList<ProductModel> products, List<RepackModel> repacks)
        {
            List<CurrentRepackStockVm> currentStockData = new List<CurrentRepackStockVm>();
            List<decimal?> sumOfStocks = new List<decimal?>();
            repacks.ToList().ForEach(x =>
            {
                var currentStock = products.FirstOrDefault(z => z.Id == x.ProductCode);
                if (currentStock != null)
                {
                    sumOfStocks.Add(currentStock.Weight * x.Quantity);
                }
            });
            var totalData = GetBulkStockData(sumOfStocks.Sum(), currentStockofBulk, selectedBulkProduct.Weight);
            return totalData;
        }
        private CurrentRepackStockVm GetBulkStockData(decimal? sumOfRepackStock, decimal? currentStock, decimal? bulkWeight)
        {
            var stockData = ((bulkWeight * currentStock) - sumOfRepackStock) / bulkWeight;
            currentStockofBulk = stockData;// currentStock - stockData;
            return new CurrentRepackStockVm(stockData, currentStock - stockData, currentStockofBulk);
        }
        private void AddItemSource(ProductModel openingModel)
        {
            CurrentRepackStockVm pack = null;
            currentStockofBulk = OpeningStockController.GetCurrentStockByProductCode(openingModel.BulkCode.Value);
            selectedBulkProduct = controller.GetProductsById(UserModelVm.CompanyId, UserModelVm.BranchId, openingModel.BulkCode.Value);
            var SavedBulkData = controller.GetRepackByBulkId(0, null, openingModel.BulkCode.Value);
            if (SavedBulkData.Any())
            {
                pack = GetCurrentStock(_products, SavedBulkData);
            }
            if (currentStockofBulk > 0)
            {
                RepackStockModel _openingStock = new RepackStockModel();
                _openingStock.ProductName = openingModel.ItemName;
                _openingStock.productCode = Convert.ToInt64(openingModel.Id);
                //_openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(openingModel.Id));
                _openingStock.RetailPrice = openingModel.RetailPrice;
                _openingStock.Weight = openingModel.Weight;
                OpeningStocks[rowIndex] = _openingStock;
                lvProductDetails.ItemsSource = OpeningStocks;
                if (pack == null)
                    CalculateStock(null);
                else
                    AddIntoBulk(selectedBulkProduct, pack);
                txt_BulkName.Text = selectedBulkProduct.ItemName;
                txt_Weight.Text = Convert.ToString(selectedBulkProduct.Weight);
                txt_Cost.Text = Convert.ToString(selectedBulkProduct.TradePrice);
                txt_LatestSale.Text = Convert.ToString(selectedBulkProduct.RetailPrice);
                txt_CurrentStock.Text = pack != null ? Convert.ToString(pack.CurrentStock) : Convert.ToString(currentStockofBulk);
            }
            else
            {
                //  AddIntoBulk(selectedBulkProduct, pack);
                ErroMessage((string)Application.Current.Resources["repack_NoStockRemain_Error"], header);
                selectedBulkProduct = null;
                ClearFields();
                // ConfirmationPopup form = new ConfirmationPopup("There is no stock for this", "Repack", false);
                // form.ShowDialog();
            }

        }
        //private void ClearBulkData()
        //{
        //    txt_BulkName.Text = "";
        //    txt_Weight.Text = "";
        //    txt_Cost.Text = "";
        //    txt_LatestSale.Text = "";// Convert.ToString(selectedBulkProduct.RetailPrice);
        //    txt_CurrentStock.Text = "";// pack != null ? Convert.ToString(pack.CurrentStock) : Convert.ToString(currentStockofBulk);
        //    txt_AvailPack.Text = string.Empty;
        //    txt_LatestSale.Text = string.Empty;
        //}
        private void SetBulkData(ProductModel selectedBulkProduct)
        {

        }
        private void AddIntoBulk(ProductModel bulkProduct, CurrentRepackStockVm pack)
        {
            // txt_BulkCode.Text = Convert.ToString(bulkProduct.Id.Value);

            txt_Packed.Text = Convert.ToString(0);
            txt_AvailPack.Text = pack != null ? Convert.ToString(pack.AvailStock) : Convert.ToString(currentStockofBulk);
        }
        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            //  this.ApplicationBar.IsVisible = false;
            //ProductListPopUp productList = new ProductListPopUp();
            //productList.ShowDialog();
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lvProductDetails.SelectedIndex =
                    lvProductDetails.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lvProductDetails.SelectedIndex;
                _selectedStock = (RepackStockModel)lvProductDetails.SelectedItem;
            }
        }
        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;

            if (element.GetType() == type) return element;

            return GetAncestorByType(VisualTreeHelper.GetParent(element), type);

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //CalculateStock();
        }
        private void CalculateStock(RepackStockModel product)
        {
            var Stocks = lvProductDetails.Items.Cast<RepackStockModel>().Select(x => x).ToList();
            if (Stocks.Where(x => x.ProductCode > 0 && x.Quantity > 0).Any())
            {
                Stocks.ToList().ForEach(x =>
                {
                    x.ProductCode = x.productCode;
                    x.Id = x.Id;
                    x.ProductName = x.ProductName;
                    x.Quantity = x.Quantity;
                    x.Weight = x.Weight;
                    x.RetailPrice = x.RetailPrice;
                    x.BatchNo = x.BatchNo;
                    x.CompanyCode = x.CompanyCode;
                    x.TotalQuantityStock = x.Quantity * x.Weight;
                });
                decimal? TotalStockInUse = Stocks.Sum(x => x.TotalQuantityStock).Value;
                if (TotalStockInUse > selectedBulkProduct.Weight * currentStockofBulk)
                {
                    ErroMessage((string)Application.Current.Resources["repack_bulkweight_Error"], header);
                    txt_Packed.Text = Convert.ToString(0);
                    txt_AvailPack.Text = Convert.ToString(currentStockofBulk);
                }
                else
                {
                    if (product != null)
                    {
                        TotalStockInUse = TotalStockInUse - product.Quantity * product.Weight;
                    }
                    decimal? totalPacked = GetTotalPackedItems(TotalStockInUse);
                    txt_Packed.Text = Convert.ToString(totalPacked);
                    txt_AvailPack.Text = Convert.ToString(currentStockofBulk - totalPacked);
                }
            }
            if (Stocks.Count == 0)
            {
                selectedBulkProduct = null;
                ClearFields();
            }
        }
        private void ErroMessage(string msg, string title)
        {
            ConfirmationPopup form = new ConfirmationPopup(msg, title, false);
            form.ShowDialog();
        }
        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }
        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            dgProducts.ItemsSource = _products.Where(p => p.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.RePack) && (p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper()))).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
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
            dgProducts.ItemsSource = _products.Where(p => p.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.RePack) && (p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper()))).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
        }

        private void btn_addRow_Click(object sender, RoutedEventArgs e)
        {
            OpeningStocks.Add(new RepackStockModel() { ProductName = "" });
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            RepackStockModel productContent = (RepackStockModel)lvProductDetails.SelectedItem;
            OpeningStocks.Remove(productContent);
            lvProductDetails.ItemsSource = OpeningStocks;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            CalculateStock(null);
           
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            var stocks = lvProductDetails.Items.Cast<RepackStockModel>().Select(x => x).ToList();
            decimal? TotalStockInUse = stocks.Sum(x => x.TotalQuantityStock).Value;
            bool IsStocks = stocks.Where(x => x.ProductCode == 0 || x.Quantity == 0).Any();
            if (IsStocks)
            {
                ErroMessage((string)Application.Current.Resources["repack_Quantity_Error"], header);
            }
            else if (TotalStockInUse > selectedBulkProduct.Weight * currentStockofBulk)
            {
                ErroMessage((string)Application.Current.Resources["repack_bulkweight_Error"], header);
            }
            else if (currentStockofBulk > 0)
            {
                List<RepackModel> repack = stocks.Select(x => new RepackModel(0, selectedBulkProduct.Id.Value, Convert.ToInt32(x.ProductCode), x.Quantity, UserModelVm.UserId, "", null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()))).ToList();
                controller.SaveUpdateRepackitems(repack);
                Common.Notification((string)Application.Current.Resources["repack_Success_Msg"], header, false);
                OpeningStocks = null;
                selectedBulkProduct = null;
                additems();
                ClearFields();
                lvProductDetails.ItemsSource = OpeningStocks;
            }
            else
            {
                ErroMessage((string)Application.Current.Resources["repack_currentStock_Error"], header);
            }
        }
        private void ClearFields()
        {
            /// txt_BulkCode.Text = string.Empty;
            txt_BulkName.Text = string.Empty;
            txt_Weight.Text = string.Empty;
            txt_Cost.Text = string.Empty;
            txt_CurrentStock.Text = string.Empty;
            txt_Packed.Text = string.Empty;
            txt_AvailPack.Text = string.Empty;
            txt_LatestSale.Text = string.Empty;
        }

        private void txt_cmpPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            RepackStockModel product = null;
            string value = ((System.Windows.Controls.TextBox)e.Source).Text;
            if (string.IsNullOrWhiteSpace(value))
            {
                product = (RepackStockModel)((FrameworkElement)e.Source).DataContext;

            }
            CalculateStock(product);
        }
        private decimal? GetTotalPackedItems(decimal? totalStockInUse)
        {
            return currentStockofBulk - ((selectedBulkProduct.Weight * currentStockofBulk - totalStockInUse) / selectedBulkProduct.Weight);
        }

        private void RepackDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvProductDetails_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvProductDetails_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);

        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

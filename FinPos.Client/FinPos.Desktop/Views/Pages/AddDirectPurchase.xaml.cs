using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AddDirectPurchase.xaml
    /// </summary>
    public partial class AddDirectPurchase : Page
    {
        #region Properties
        SupplierController controller = new SupplierController();
        ProductController productController = new ProductController();
        PurchaseController purchaseController = new PurchaseController();
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private List<SupplierModel> _supliers;
        private IList<ProductModel> _products;
        public PurchaseStockModel _selectedStock;
        private ObservableCollection<PurchaseStockModel> purchaseStocks;
        public int rowIndex = 0;
        public ListViewItem item = null;
        private int _noOfErrorsOnScreen = 0;
        BrushConverter color = new BrushConverter();
        ResourceDictionary myResourceDictionary;
        public string header = (string)Application.Current.Resources["purchase_purchaseHeader"];
        private int? supplierCode = null;
        TaxController taxController = new TaxController();
        private IList<TaxModel> _taxdetails;
        private string _taxValue;
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion
        #region Constructor
        public AddDirectPurchase()
        {
            InitializeComponent();
            ChangeHeightWidth();
            myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/ResourceFiles/En.xaml",
                        UriKind.RelativeOrAbsolute);
            new ObservableCollection<PurchaseStockModel>().Clear();
            // purchase_deliveryDate.se =DateTime.Now;
            ResponseVm responseTax = taxController.GetTax();
            if (responseTax.FaultData == null)
            {
                TaxModel objTaxModel = new TaxModel(0, "Select", 0, "", "", "", "");
                _taxdetails = responseTax.Response.Cast<TaxModel>().ToList();
                _taxdetails.Insert(0, objTaxModel);
                this.cmbTax.ItemsSource = _taxdetails;
                this.cmbTax.DisplayMemberPath = "TaxDetail";
                this.cmbTax.SelectedValuePath = "TaxCode";
                this.cmbTax.SelectedIndex = 0;
            }
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                _products?.ToList().ForEach(x =>
                {
                    int currentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(x.Id));
                    x.CurrentStock = currentStock;
                });
                dgPurchases.ItemsSource = _products;
                dgPurchases.Visibility = Visibility.Visible;
            }
            if (purchaseStocks == null)
            {
                additems();
            }

            btn_AddRow.IsEnabled = true;
            btn_Save.IsEnabled = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            _supliers = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId, UserModelVm.BranchId).OrderBy(x => x.Id).ToList();
            lstPurchase.ItemsSource = purchaseStocks;
        }
        #endregion
        #region Common Method
        public void ChangeHeightWidth()
        {
            this.AddDirectPurchasePage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddDirectPurchasePage.Width = HeightWidth.width;

        }
        public void SetPropertiesOfSupplier(SupplierModel supplier)
        {
            supplierCode = supplier.Id.Value;
            supplier_name.Text = supplier.SupplierName;
            supplier_mobile.Text = supplier.Mobile;
            txt_suplierAddress.Text = supplier.Address;
        }
        public void ClearFileds()
        {
            supplier_name.Text = string.Empty;
            supplier_mobile.Text = string.Empty;
            txt_suplierAddress.Text = string.Empty;
        }
        public void additems()
        {
            purchaseStocks = new ObservableCollection<PurchaseStockModel>();
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }
        public void NavigateToBackPage()
        {
            DirectPurchase form = new DirectPurchase();
            NavigationService.Navigate(form);
        }
        private void ClearData()
        {
            supplier_name.Text = "";
            supplier_mobile.Text = "";
            txt_suplierAddress.Text = "";
            purchase_expiryDate.Text = "";
            purchase_deliveryDate.Text = "";
            purchase_cashdiscount.Text = "";
            purchase_cashdiscountDoller.Text = "";
            purchase_cashSubChargeAmo.Text = "";
            cmbTax.SelectedIndex = 0;
        }
        private void SetPurchaseGridValues()
        {
            dgPurchases.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
        }
        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            SetPurchaseGridValues();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
        }
        #endregion
        #region CRUD Operation
        private void lstPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        }

        private void btn_AddRow_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var data = _supliers;
            string query = (sender as TextBox).Text;
            if (query.Length == 0)
            {
                lbAutoPurchase.Items.Clear();
                lbAutoPurchase.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbAutoPurchase.IsEnabled = true;
                lbAutoPurchase.Visibility = Visibility.Visible;
            }

            // Clear the list 
            lbAutoPurchase.Items.Clear();

            // Add the result 
            foreach (var obj in data)
            {
                if (Convert.ToString(obj.Id).ToLower().StartsWith(query.ToLower()))
                {
                    addProductCode(obj);
                    found = true;
                }
            }

            if (!found)
            {
                lbAutoPurchase.Items.Add(new TextBlock() { Text = "No results found." });
                lbAutoPurchase.IsEnabled = false;
            }
        }
        private void addProductCode(SupplierModel suplier)
        {
            lbAutoPurchase.Items.Add(suplier.Id);
        }

        private void addProductName(SupplierModel suplier)
        {

            lbAutoPurchaseWithName.Items.Add(suplier.SupplierName);
        }


        private void lbAutoPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoPurchase.Visibility = Visibility.Collapsed;
            if (lbAutoPurchase.SelectedIndex != -1)
            {
                SupplierModel supplier = controller.GetSuppliersBySupplierCode(UserModelVm.CompanyId, UserModelVm.BranchId, Convert.ToInt32(lbAutoPurchase.SelectedItem));
                //  supplier = _supliers?.FirstOrDefault(x => Convert.ToString(x.Id.Value) == Convert.ToString(lbAutoPurchase.SelectedItem));
                SetPropertiesOfSupplier(supplier);
            }
        }

        private void supplier_name_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("supplier_name_KeyUp");
        }

        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierModel suplier = _supliers?.FirstOrDefault(x => x.SupplierName.ToLower() == supplier_name.Text.ToLower());
            if (suplier == null)
                ClearFileds();
            else
                SetPropertiesOfSupplier(suplier);
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }

        private void lbAutoPurchaseWithName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            if (lbAutoPurchaseWithName.SelectedIndex != -1)
            {
                SupplierModel suplier = _supliers.FirstOrDefault(x => x.SupplierName == Convert.ToString(lbAutoPurchaseWithName.SelectedItem));
                SetPropertiesOfSupplier(suplier);
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            List<StockModel> newStocks = new List<StockModel>();
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
            {
                decimal? nullval = null;
                DateTime? nulldate = null;
                PurchaseModel model = new PurchaseModel(0, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()), supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                    , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text), string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), UserModelVm.CompanyId, UserModelVm.BranchId, string.Empty);
                int purchaseId = purchaseController.SaveUpdateDirectPurchase(model);

                newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                {
                    return new StockModel(0, purchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.Discount, x.BatchNo, x.ProductCode, null);
                }).ToList());

                purchaseController.SaveUpdateStocks(newStocks);
                Common.Notification((string)myResourceDictionary["purchase_savedmsg"], header, false);
                ClearData();
                NavigateToBackPage();
            }
            else
                Common.ErrorMessage((string)myResourceDictionary["purchase_requiredFields"], header);
        }


        private void reorder_level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            DirectPurchase form = new DirectPurchase();
            NavigationService.Navigate(form);
        }

        private void dgPurchase_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgPurchases.SelectedItem;
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            stock_AddItem(productItem);
        }
        public void stock_AddItem(ProductModel itemToAdd)
        {
            if (purchaseStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_already"], header, true);
                form.ShowDialog();
                if (Common._isChecked)
                    AddpurchaseItemSource(itemToAdd);
            }
            else
                AddpurchaseItemSource(itemToAdd);
        }
        private void AddpurchaseItemSource(ProductModel purchaseModel)
        {
            PurchaseStockModel _purchaseStock = new PurchaseStockModel();
            _purchaseStock.ProductName = purchaseModel.ItemName;
            _purchaseStock.productCode = Convert.ToInt64(purchaseModel.Id);
            purchaseStocks[rowIndex] = _purchaseStock;
            lstPurchase.ItemsSource = purchaseStocks;
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
        }

        private void btn_select_purchase_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = true;
            ListViewItem lvi = GetAncestorByType(
   e.OriginalSource as DependencyObject, typeof(ListViewItem)) as ListViewItem;
            if (lvi != null)
            {
                lstPurchase.SelectedIndex =
                    lstPurchase.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lstPurchase.SelectedIndex;

            }
        }

        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            return GetAncestorByType(VisualTreeHelper.GetParent(element), type);
        }


        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }
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

        private void DirectPurchase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Remove((PurchaseStockModel)lstPurchase.SelectedItem);
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
            netAmount.Text = Convert.ToString(Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal)));
            totalAmount.Text = Convert.ToString(Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));
        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            ListViewItem lvi = GetAncestorByType(
             e.OriginalSource as DependencyObject, typeof(ListViewItem)) as ListViewItem;
            if (lvi != null)
            {
                lstPurchase.SelectedIndex =
                    lstPurchase.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lstPurchase.SelectedIndex;
                _selectedStock = (PurchaseStockModel)lstPurchase.SelectedItem;
            }
        }

        private void txt_decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.DecimalValueChecker(sender, e);
        }

        private void txt_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxName = (TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource);
            if (cv == null)
                return;
            if (!string.IsNullOrEmpty(filterText))
            {

                cv.Filter = o =>
                {
                    /* change to get data row value */
                    ProductModel p = o as ProductModel;
                    return (p.ItemName.ToUpper().StartsWith(filterText.ToUpper()) || p.BarCode.ToUpper().StartsWith(filterText.ToUpper()) || Convert.ToString(p.Id).ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };
            }
            else
                cv.Filter = null;
        }

        private void purchase_discount_KeyUp(object sender, KeyEventArgs e)
        {
            objValidation.ValidateTextBoxForDiscount(sender, header, (string)Application.Current.Resources["purchase_invalidDiscount"]);
        }

        private void TextBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;
            if (e.Key == Key.Space)
            {
                textbox.Text = Regex.Replace(textbox.Text, " ", "");
                e.Handled = true;
            }
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoPurchaseWithName.Visibility == Visibility.Visible)
                lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            else
                ReusableCodeTemp("arrow_Click");
        }

        /// <summary>
        /// this method is created for setting values in Product searchable textbox
        /// this method is called at two diffrent places one at arrow click which will populate all products if query serach will be empty
        /// </summary>
        /// <param name=""></param>
        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var data = _supliers;
                string query = supplier_name.Text;
                lbAutoPurchaseWithName.Items.Clear();
                if (query.Length == 0)
                {
                    if (callingMethod == "arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            addProductName(obj);
                            found = true;
                        }
                        lbAutoPurchaseWithName.IsEnabled = true;
                        lbAutoPurchaseWithName.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbAutoPurchaseWithName.IsEnabled = false;
                        lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (Convert.ToString(obj.SupplierName).ToLower().StartsWith(query.ToLower()))
                        {
                            addProductName(obj);
                            found = true;
                        }
                    }
                    lbAutoPurchaseWithName.IsEnabled = true;
                    lbAutoPurchaseWithName.Visibility = Visibility.Visible;
                }
                if (!found)
                {
                    lbAutoPurchaseWithName.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
                    lbAutoPurchaseWithName.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void amountCount_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
            else
            {
                List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
                netAmount.Text = amount.NetTotal;
                totalAmount.Text = amount.TotalAmount;
                subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbAutoPurchaseWithName.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
            lbAutoPurchaseWithName.IsEnabled = false;
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }
        private void cmbTax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _taxValue = cmbTax.SelectedIndex > 0 ? Convert.ToString(_taxdetails.FirstOrDefault(item => item.TaxCode == Convert.ToInt32(this.cmbTax.SelectedValue)).Rate) : "0";
                List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
                netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));
                subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
            }
            catch (Exception ex)
            {
            }
        }

        private void purchase_cashdiscountDoller_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }


        private void lstPurchase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lstPurchase_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion
        #region Search Box
        private void txt_search_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_search.Text = string.Empty;
        }
        private void txt_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text))
                txt_search.Text = "Search";
        }
        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            SetPurchaseGridValues();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
                txt_search.Text = "Search";
        }
        private void arrowCross_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }

        #endregion

        private void supplier_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void supplier_mobile_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

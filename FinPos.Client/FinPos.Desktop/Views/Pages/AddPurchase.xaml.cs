using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddPurchase.xaml
    /// </summary>c
    public partial class AddPurchase : Page
    {
        SupplierController controller = new SupplierController();
        ProductController productController = new ProductController();
        PurchaseController purchaseController = new PurchaseController();
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private List<SupplierModel> _supliers;
        public PurchaseStockModel _selectedStock;
        private IList<ProductModel> _products;
        private ObservableCollection<PurchaseStockModel> purchaseStocks;
        public int rowIndex = 0;
        public ListViewItem item = null;
        private int _noOfErrorsOnScreen = 0;
        BrushConverter color = new BrushConverter();
        ResourceDictionary myResourceDictionary;
        public string header = "Purchase";
        private int supplierCode;
        TaxController taxController = new TaxController();
        private IList<TaxModel> _taxdetails;
        private string _taxValue;
        CommonFunction.Validations objValidation = new CommonFunction.Validations(); 
        public AddPurchase()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ClearData();

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
                if (_products != null && _products.Count > 0)
                {
                    _products?.ToList().ForEach(x =>
                {
                    int currentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(x.Id));
                    //ProductModel product = new ProductModel(x.Id, x.ItemCode, x.ItemName, x.RetailPrice.Value, x.TradePrice.Value, x.CategoryCode.Value, x.ItemType, x.BarCode, x.TaxPercentage.Value, x.CategoryName);
                    x.CurrentStock = currentStock;
                });

                    dgPurchases.ItemsSource = _products;
                    dgPurchases.Visibility = Visibility.Visible;
                }
                else
                {
                    dgPurchases.Visibility = Visibility.Collapsed;
                    brd_exp.Visibility = Visibility.Visible;
                }
            }
            if (purchaseStocks == null)
            {
                additems();
            }

            btn_AddRow.IsEnabled = true;
            btn_Save.IsEnabled = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            _supliers = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId, UserModelVm.BranchId).ToList();
            lstPurchase.ItemsSource = purchaseStocks;
        }
        public void ChangeHeightWidth()
        {
            this.AddPoPage.Height = HeightWidth.Height - 65;
            this.AddPoPage.Width = HeightWidth.width;

        }
        private void lstPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        }
        public void additems()
        {
            purchaseStocks = new ObservableCollection<PurchaseStockModel>();
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }

        private void btn_AddRow_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
        }
        private void addProductCode(SupplierModel suplier)
        {
            lbAutoPurchase.Items.Add(suplier.SupplierName);
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
                SupplierModel suplier = new SupplierModel();
                suplier = _supliers?.FirstOrDefault(x => Convert.ToString(x.Id.Value) == Convert.ToString(lbAutoPurchase.SelectedItem));
                // supplierCode = lbAutoPurchase.SelectedItem;
                supplier_name.Text = suplier.SupplierName;
                supplier_mobile.Text = suplier.Mobile;
                txt_suplierAddress.Text = suplier.Address;

            }
        }

        //private void txtSuplierCode_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    SupplierModel suplier = _supliers?.FirstOrDefault(x => Convert.ToString(x.Id).ToLower() == txt_SuplierCode.Text.ToLower());
        //    if (suplier == null)
        //    {
        //      //  txt_SuplierCode.Text = "";
        //        supplier_name.Text = "";
        //        supplier_mobile.Text = "";
        //        txt_suplierAddress.Text = "";
        //    }
        //    else
        //    {
        //      ///  txt_SuplierCode.Text = Convert.ToString(suplier.Id);
        //        supplier_name.Text = suplier.SupplierName;
        //        supplier_mobile.Text = suplier.Mobile;
        //        txt_suplierAddress.Text = suplier.Address;
        //    }
        //    lbAutoPurchase.Visibility = Visibility.Collapsed;
        //}

        private void supplier_name_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("supplier_name_KeyUp");
        }

        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierModel suplier = _supliers?.FirstOrDefault(x => x.SupplierName.ToLower() == supplier_name.Text.ToLower());
            if (suplier == null)
            {
                // txt_SuplierCode.Text = "";
                supplier_name.Text = "";
                supplier_mobile.Text = "";
                txt_suplierAddress.Text = "";
            }
            else
            {
                supplierCode = suplier.Id.Value;
                supplier_name.Text = suplier.SupplierName;
                supplier_mobile.Text = suplier.Mobile;
                txt_suplierAddress.Text = suplier.Address;
            }
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }

        private void lbAutoPurchaseWithName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            if (lbAutoPurchaseWithName.SelectedIndex != -1)
            {
                SupplierModel suplier = new SupplierModel();
                suplier = _supliers.FirstOrDefault(x => x.SupplierName == Convert.ToString(lbAutoPurchaseWithName.SelectedItem));
                supplierCode = suplier.Id.Value;
                supplier_name.Text = Convert.ToString(lbAutoPurchaseWithName.SelectedItem);
                supplier_mobile.Text = suplier.Mobile;
                txt_suplierAddress.Text = suplier.Address;

            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (supplierCode > 0)
            {
                List<StockModel> newStocks = new List<StockModel>();
                List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
                {
                    decimal? nullval = null;
                    DateTime? nulldate = null;
                    int? nullint = null;
                    PurchaseOrderModel model = new PurchaseOrderModel(0, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()), supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                        , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text), string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), (int)CommonEnum.PurchaseStatus.WaitingForApproval, null, null, UserModelVm.CompanyId, UserModelVm.BranchId, string.Empty, string.Empty, purchase_invoiceNo.Text, purchase_invoiceDate.Text);
                    int purchaseId = purchaseController.SaveUpdatePurchase(model);

                    // stocks = lstPurchase.ItemsSource.Cast<StockModel>().ToList();
                    newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                   {
                       return new StockModel(0, null, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.Discount, x.BatchNo, x.ProductCode, purchaseId);
                       // newStocks.Add(stock);
                   }).ToList());

                    purchaseController.SaveUpdateStocks(newStocks);
                    //  ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_savedmsg"], header, false);
                    //  form.ShowDialog();
                    Common.Notification((string)Application.Current.Resources["purchase_savedmsg"], header, false);
                    ClearData();
                    Purchase form1 = new Purchase();
                    NavigationService.Navigate(form1);
                }
                else
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_requiredFields"], header, false);
                    form.ShowDialog();
                    // Common.ErrorNotification((string)Application.Current.Resources["purchase_requiredFields"], header, false);
                }
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["supplier_requiredFields"], header, false);
                form.ShowDialog();
                //  Common.ErrorNotification((string)Application.Current.Resources["supplier_requiredFields"], header, false);
            }
        }


        private void reorder_level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        private void ClearData()
        {
            //  txt_SuplierCode.Text = "";
            supplier_name.Text = "";
            supplier_mobile.Text = "";
            txt_suplierAddress.Text = "";
            purchase_expiryDate.Text = "";
            purchase_deliveryDate.Text = "";
            purchase_cashdiscount.Text = "";
            purchase_cashdiscountDoller.Text = "";
            // purchase_cashTaxAmount.Text = "";
            purchase_cashSubChargeAmo.Text = "";
            purchase_invoiceNo.Text = "";
            purchase_invoiceDate.Text = "";
            cmbTax.SelectedIndex = 0;
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Purchase form = new Purchase();
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
                {
                    AddpurchaseItemSource(itemToAdd);
                }
            }
            else
            {
                AddpurchaseItemSource(itemToAdd);
            }
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
            // btn_remove.IsEnabled = false;
        }

        private void btn_select_purchase_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = true;
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
   e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
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
            CommonFunctions.DecimalValueChecker(sender, e);
            //e.Handled = !IsTextAllowed(e.Text);
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

        private void Purchase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Remove((PurchaseStockModel)lstPurchase.SelectedItem);
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
            netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
            totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
  e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
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
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
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
            {
                lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReusableCodeTemp("arrow_Click");
            }

        }

        //private void quantity_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    dynamic row = e.Source;
        //    dynamic purchase = row.DataContext;
        //    var TotalAmount = (purchase.CostPrice * purchase.Quantity + purchase.Discount) + purchase_cashdiscount.Text + purchase_cashdiscountDoller.Text;
        //    //  object company = dd.DataContext;
        //    // var dd = sender as ListViewItem;
        //}

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
                            //if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                            //{
                            addProductName(obj);
                            found = true;
                            //}
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
                //decimal total = 0;
                //dynamic row = e.Source;
                //dynamic purchase = row.DataContext;
                List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                //Stocks.ForEach(x =>
                //{
                //    if (x.Quantity > 0 && x.CostPrice > 0)
                //        total += x.CostPrice * Convert.ToDecimal(x.Quantity) - ((x.CostPrice * Convert.ToDecimal(x.Quantity) * x.Discount) / 100); //+( string.IsNullOrEmpty(purchase_cashdiscount.Text) ? 0 : int.Parse(purchase_cashdiscount.Text)) +( string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? 0 : int.Parse(purchase_cashdiscountDoller.Text));

                //});
                //MessageBox.Show(Convert.ToString(total));
                ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
                netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

                subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbAutoPurchaseWithName.IsEnabled = false;
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }

        private void btn_showPop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            dgPurchases.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
            //SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void arrowCross_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }

        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            dgPurchases.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
        }
        #endregion

        private void cmbTax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbTax.SelectedIndex > 0)
                {
                    _taxValue = Convert.ToString(_taxdetails.FirstOrDefault(item => item.TaxCode == Convert.ToInt32(this.cmbTax.SelectedValue)).Rate);
                    List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                    ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
                    netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                    totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

                    subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                    discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                    taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
                }
                else
                {
                    _taxValue = "0";
                    List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
                    ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
                    netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                    totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

                    subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                    discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                    taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
                }
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

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click_4(object sender, RoutedEventArgs e)
        {

        }
    }
}

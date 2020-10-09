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
    /// Interaction logic for EditDirectPurchase.xaml
    /// </summary>
    public partial class EditDirectPurchase : Page
    {
        #region Properties
        SupplierController controller = new SupplierController();
        ProductController productController = new ProductController();
        PurchaseController purchaseController = new PurchaseController();
        private List<SupplierModel> _supliers;
        private List<StockModel> _orignalLstStocks;
        private List<ProductModel> _products;
        private ObservableCollection<PurchaseStockModel> purchaseStocks;
        public int rowIndex = 0;
        public List<StockModel> _deletestocks = new List<StockModel>();
        public PurchaseStockModel _selectedStock;
        BrushConverter color = new BrushConverter();
        public PurchaseModel _purchaseData;
        private int _noOfErrorsOnScreen = 0;
        public string header = "Purchase";
        private int? _supplierCode = null;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        private IList<TaxModel> _taxdetails;
        private string _taxValue;
        TaxController taxController = new TaxController();
        #endregion
        #region Constructor
        public EditDirectPurchase(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            SupplierModel suplier = GetSupplier(row);
            Settext(row, suplier);
            _orignalLstStocks = purchaseController.GetDirectStocks(row.PurchaseId);
            _purchaseData = row;
            GetTax(row);
            GetProducts();
            SetNetAmountDetails(row);
            EnableButtons();
        }
        #endregion
        #region Common Methods
        private void EnableButtons()
        {
            btn_AddRow.IsEnabled = true;
            btn_Save.IsEnabled = true;
            lstPurchase.Visibility = Visibility.Visible;
            btn_AddRow.Visibility = Visibility.Visible;
            btn_remove.Visibility = Visibility.Visible;
        }

        private void SetNetAmountDetails(dynamic row)
        {
            List<PurchaseStockModel> purchaseStockData = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
            purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockData);
            ProductAmount amount = CommonFunctions.RetrunProductAmount(purchaseStockData, Convert.ToString(row.DiscountPercentage), purchase_cashdiscountDoller.Text, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
            netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
            totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));
            subChargeAmount.Text = Convert.ToString(row.SurChargeAmount);
            discountChargeAmount.Text = Convert.ToString(row.DiscountPercentage);
            taxChargeAmount.Text = Convert.ToString(row.TaxPercentage);
            lstPurchase.ItemsSource = purchaseStocks;
        }

        private void GetTax(dynamic row)
        {
            ResponseVm responseTax = taxController.GetTax();
            if (responseTax.FaultData == null)
            {
                TaxModel objTaxModel = new TaxModel(0, "Select");
                _taxdetails = responseTax.Response.Cast<TaxModel>().ToList();
                _taxdetails.Insert(0, objTaxModel);
                this.cmbTax.ItemsSource = _taxdetails;
                this.cmbTax.DisplayMemberPath = "TaxDetail";
                this.cmbTax.SelectedValuePath = "TaxCode";
            }
            _taxValue = Convert.ToString(row.TaxPercentage);
            if (row.TaxPercentage != null && Convert.ToDecimal(row.TaxPercentage) != Convert.ToDecimal(0) && _taxdetails.Any(item => Convert.ToDouble(item.Rate) == Convert.ToDouble(_taxValue)))
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.Text = Convert.ToString(_taxdetails.FirstOrDefault(item => item.Rate == Convert.ToInt32(row.TaxPercentage)).TaxDetail);
            }
            else
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.SelectedIndex = 0;
            }
        }

        private void GetProducts()
        {
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                _products?.ToList().ForEach(x =>
                {
                    x.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(x.Id));
                });
                dgPurchases.ItemsSource = _products;
                dgPurchases.Visibility = Visibility.Visible;
            }
        }

        private void Settext(dynamic row, SupplierModel suplier)
        {
            supplier_name.Text = row.SuplierName;
            supplier_mobile.Text = Convert.ToString(suplier?.Mobile);
            txt_suplierAddress.Text = Convert.ToString(suplier?.Address);
            purchase_expiryDate.Text = Convert.ToString(row.ExpiryDate);
            purchase_deliveryDate.Text = Convert.ToString(row.DeliveryDate);
            purchase_cashdiscount.Text = Convert.ToString(row.DiscountPercentage);
            purchase_cashdiscountDoller.Text = Convert.ToString(row.DiscountAmount);

            purchase_cashSubChargeAmo.Text = Convert.ToString(row.SurChargeAmount);
            purchase_pono.Text = Convert.ToString(row.PurchaseId);
        }

        private SupplierModel GetSupplier(dynamic row)
        {
            _supliers = controller.GetSuppliers().ToList();
            SupplierModel suplier = _supliers?.FirstOrDefault(x => x.Id == row.SuplierCode);
            _supplierCode = row.SuplierCode;
            return suplier;
        }

        public void ChangeHeightWidth()
        {
            this.EditDirectPurchasePage.Height = HeightWidth.Height - 65;
            this.EditDirectPurchasePage.Width = HeightWidth.width;

        }

        //private int GetRetrunedQuantity(int quantity, int returedQuantity)
        //{
        //   // int orignalQuantity= returedQuantity == 0 ? 0 : quantity - returedQuantity;
        //    return orignalQuantity;// y.Sum(f => f.Quantity) - _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().productCode && d.BatchNo == y.FirstOrDefault().BatchNo).Quantity)
        //}
        private void DisableTextBox()
        {
            // txt_SuplierCode.IsEnabled = false;
            supplier_name.IsEnabled = false;
            supplier_mobile.IsEnabled = false;
            txt_suplierAddress.IsEnabled = false;
            purchase_expiryDate.IsEnabled = false;
            purchase_deliveryDate.IsEnabled = false;
            purchase_cashdiscount.IsEnabled = false;
            purchase_cashdiscountDoller.IsEnabled = false;
            cmbTax.IsEnabled = false;//  purchase_cashTaxAmount.IsEnabled = false;
            purchase_cashSubChargeAmo.IsEnabled = false;
            purchase_pono.IsEnabled = false;
            arrow.IsEnabled = false;
        }

        //private ObservableCollection<PurchaseStockModel> GetGroupOfPurchaseStocks(ObservableCollection<PurchaseStockModel> purchaseStocks)
        //{
        //    return new ObservableCollection<PurchaseStockModel>(purchaseStocks.GroupBy(x => new { x.ProductCode, x.BatchNo }).Select(y => new PurchaseStockModel(y.FirstOrDefault().ProductCode, y.FirstOrDefault().ProductName
        //              , y.Sum(z => z.Quantity), y.FirstOrDefault().CostPrice, y.FirstOrDefault().Discount, y.FirstOrDefault().BatchNo, 0,
        //              _orignalLstPurchaseRetrun.Any() && _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo) != null ?
        //              _orignalLstPurchaseRetrun.Where(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo).Sum(z => z.Quantity) : 0,
        //             0, 00, y.FirstOrDefault().ReasonForRetrun)));
        //}
        private string GetProductName(int productCode)
        {
            return _products.FirstOrDefault(x => x.Id.Value == productCode).ItemName;
        }
        public void additems()
        {
            purchaseStocks = new ObservableCollection<PurchaseStockModel>();
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }
        #endregion
        private void btn_AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseStocks == null)
            {
                additems();
                lstPurchase.ItemsSource = purchaseStocks;
            }
            else
                purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
            lstPurchase.ItemsSource = purchaseStocks;
        }
        private void addProductCode(SupplierModel suplier)
        {
            lbAutoPurchase.Items.Add(suplier.Id);
        }
        private void addProductName(SupplierModel suplier)
        {
            lbAutoPurchaseWithName.Items.Add(suplier.SupplierName);
        }
        private void lbAutoPurchaseWithName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            if (lbAutoPurchaseWithName.SelectedIndex != -1)
            {
                SupplierModel suplier = new SupplierModel();
                suplier = _supliers.FirstOrDefault(x => x.SupplierName == Convert.ToString(lbAutoPurchaseWithName.SelectedItem));
                _supplierCode = suplier.Id.Value;
                supplier_name.Text = Convert.ToString(lbAutoPurchaseWithName.SelectedItem);
                supplier_mobile.Text = suplier?.Mobile;
                txt_suplierAddress.Text = suplier?.Address;

            }
        }

        private void lbAutoPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoPurchase.Visibility = Visibility.Collapsed;
            if (lbAutoPurchase.SelectedIndex != -1)
            {
                SupplierModel suplier = new SupplierModel();
                suplier = _supliers.FirstOrDefault(x => Convert.ToString(x.Id.Value) == Convert.ToString(lbAutoPurchase.SelectedItem));
                // _supplierCode = lbAutoPurchase.SelectedItem;
                supplier_name.Text = suplier.SupplierName;
                supplier_mobile.Text = suplier.Mobile;
                txt_suplierAddress.Text = suplier.Address;

            }
        }

        //private void txtSuplierCode_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    SupplierModel suplier = _supliers.FirstOrDefault(x => Convert.ToString(x.Id).ToLower() == txt_SuplierCode.Text.ToLower());
        //    if (suplier == null)
        //    {
        //        txt_SuplierCode.Text = "";
        //        supplier_name.Text = "";
        //        supplier_mobile.Text = "";
        //        txt_suplierAddress.Text = "";
        //    }
        //    else
        //    {
        //        txt_SuplierCode.Text = Convert.ToString(suplier.Id);
        //        supplier_name.Text = suplier.SupplierName;
        //        supplier_mobile.Text = suplier.Mobile;
        //        txt_suplierAddress.Text = suplier.Address;
        //    }
        //    lbAutoPurchase.Visibility = Visibility.Collapsed;
        //}

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
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
        private void supplier_name_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            //  var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            var data = _supliers;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear 
                lbAutoPurchaseWithName.Items.Clear();
                lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbAutoPurchaseWithName.IsEnabled = true;
                lbAutoPurchaseWithName.Visibility = Visibility.Visible;
            }

            // Clear the list 
            lbAutoPurchaseWithName.Items.Clear();

            // Add the result 
            foreach (var obj in data)
            {
                if (Convert.ToString(obj.SupplierName).ToLower().StartsWith(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work 
                    addProductName(obj);
                    found = true;
                }
            }

            if (!found)
            {
                lbAutoPurchase.Items.Add(new TextBlock() { Text = "No results found." });
                lbAutoPurchase.IsEnabled = false;
            }
        }
        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierModel suplier = _supliers.FirstOrDefault(x => x.SupplierName.ToLower() == supplier_name.Text.ToLower());
            if (suplier == null)
            {
                // txt_SuplierCode.Text = "";
                supplier_name.Text = "";
                supplier_mobile.Text = "";
                txt_suplierAddress.Text = "";
            }
            else
            {
                _supplierCode = suplier.Id.Value;
                supplier_name.Text = suplier.SupplierName;
                supplier_mobile.Text = suplier.Mobile;
                txt_suplierAddress.Text = suplier.Address;
            }
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }
        private void reorder_level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        private void ClearData()
        {
            // txt_SuplierCode.Text = "";
            supplier_name.Text = "";
            supplier_mobile.Text = "";
            txt_suplierAddress.Text = "";
            purchase_expiryDate.Text = "";
            purchase_deliveryDate.Text = "";
            purchase_cashdiscount.Text = "";
            purchase_cashdiscountDoller.Text = "";
            cmbTax.SelectedIndex = 0; // purchase_cashTaxAmount.Text = "";
            purchase_cashSubChargeAmo.Text = "";
        }
        private void lstPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            //  dynamic row = e.Source;
            btn_AddRow.IsEnabled = true;
            btn_remove.IsEnabled = true;
            btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            new ObservableCollection<PurchaseStockModel>();
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
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_already"], header, true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    PurchaseStockModel _purchaseStock = new PurchaseStockModel();
                    _purchaseStock.ProductName = itemToAdd.ItemName;
                    _purchaseStock.productCode = Convert.ToInt64(itemToAdd.Id);
                    purchaseStocks[rowIndex] = _purchaseStock;
                    lstPurchase.ItemsSource = purchaseStocks;
                }
            }
            else
            {
                // if (Common._isChecked)
                // {
                PurchaseStockModel _purchaseStock = new PurchaseStockModel();
                _purchaseStock.ProductName = itemToAdd.ItemName;
                _purchaseStock.productCode = Convert.ToInt64(itemToAdd.Id);
                purchaseStocks[rowIndex] = _purchaseStock;
                lstPurchase.ItemsSource = purchaseStocks;
                // }
            }
            if (_selectedStock.StockId != null)
                _deletestocks.Add(new StockModel(_selectedStock.StockId, _purchaseData.PurchaseId.Value, _selectedStock.Quantity, _selectedStock.CostPrice, _selectedStock.SellingPrice, _selectedStock.MRP, _selectedStock.Discount, _selectedStock.BatchNo, _selectedStock.ProductCode, null));
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
            //ProductListPopUp productList = new ProductListPopUp();
            //productList.ShowDialog();
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

        private void btn_Save_Click(object sender, RoutedEventArgs e)


        {
            List<StockModel> newStocks = new List<StockModel>();
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
            {
                decimal? nullval = null;
                DateTime? nulldate = null;
                dynamic row = new PurchaseModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, string.IsNullOrEmpty(supplier_name.Text) ? null : _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                    , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
                    , string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName);
                var result = purchaseController.SaveUpdateDirectPurchase(row);
                newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                {
                    return new StockModel(x.StockId == null ? 0 : x.StockId, _purchaseData.PurchaseId.Value, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.Discount, x.BatchNo, x.ProductCode, null);
                    // newStocks.Add(stock);
                }).ToList());

                purchaseController.UpdateStocks(newStocks, _deletestocks);
                //  ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_updated"], header, false);
                // form.ShowDialog();
                Common.Notification((string)Application.Current.Resources["purchase_updated"], header, false);
                ClearData();
                DirectPurchase form1 = new DirectPurchase();
                NavigationService.Navigate(form1);
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_requiredFields"], header, false);
                form.ShowDialog();
                // Common.ErrorNotification((string)myResourceDictionary["purchase_requiredFields"], header, false);
            }
        }

        private void DirectPurchase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {

        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            PurchaseStockModel purchase = (PurchaseStockModel)lstPurchase.SelectedItem;

            if (purchase.StockId != null)
                _deletestocks.Add(new StockModel(purchase.StockId, _purchaseData.PurchaseId.Value, purchase.Quantity, purchase.CostPrice, purchase.SellingPrice, purchase.MRP, purchase.Discount, purchase.BatchNo, purchase.ProductCode, null));
            purchaseStocks.Remove(purchase);
            lstPurchase.ItemsSource = purchaseStocks;
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            ProductAmount amount = CommonFunctions.RetrunProductAmount(Stocks, purchase_cashdiscount.Text, purchase_cashdiscountDoller.Text, _taxValue, purchase_cashSubChargeAmo.Text);
            netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
            totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));
        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            //ProductListPopUp productList = new ProductListPopUp();
            //productList.ShowDialog();
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
                    return (p.ItemName.ToUpper().StartsWith(filterText.ToUpper()) || p.BarCode.ToUpper().StartsWith(filterText.ToUpper()) || p.Id.ToString().ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };
            }
            else
            {
                cv.Filter = null;
            }
        }

        private void purchase_cashdiscount_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(purchase_cashdiscount.Text))
            {
                if (Convert.ToDecimal(purchase_cashdiscount.Text) > 100)
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["invalid_DiscountSupplier"], header, false);
                    form.ShowDialog();
                    // Common.ErrorNotification((string)myResourceDictionary["invalid_DiscountSupplier"], header, false);
                    purchase_cashdiscount.Text = string.Empty;
                }
            }
        }

        private void TextBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;
            if (e.Key == Key.Space)
            {
                textbox.Text = Regex.Replace(textbox.Text, @"\s+", "");
                e.Handled = true;
            }
        }

        private void purchase_discount_KeyUp(object sender, KeyEventArgs e)
        {
            objValidation.ValidateTextBoxForDiscount(sender, header, (string)Application.Current.Resources["purchase_invalidDiscount"]);
        }

        private void supplier_name_TextChanged(object sender, TextChangedEventArgs e)
        {

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
                netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

                subChargeAmount.Text = string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? "0" : purchase_cashSubChargeAmo.Text;
                discountChargeAmount.Text = string.IsNullOrEmpty(purchase_cashdiscount.Text) ? "0" : purchase_cashdiscount.Text;
                taxChargeAmount.Text = string.IsNullOrEmpty(_taxValue) ? "0" : _taxValue;
            }
        }

        private void ContentPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbAutoPurchaseWithName.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
            lbAutoPurchaseWithName.IsEnabled = false;
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
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

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstPurchase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lstPurchase_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}

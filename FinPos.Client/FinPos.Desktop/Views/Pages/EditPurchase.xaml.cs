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
using System.Reflection;
using System.Resources;
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
    /// Interaction logic for EditPurchase.xaml
    /// </summary>
    public partial class EditPurchase : Page
    {
        SupplierController controller = new SupplierController();
        ProductController productController = new ProductController();
        PurchaseController purchaseController = new PurchaseController();
        private List<SupplierModel> _supliers;
        private List<StockModel> _orignalLstStocks;
        private List<PurchaseReturnModel> _orignalLstPurchaseRetrun;
        private IList<ProductModel> _products;
        private ObservableCollection<PurchaseStockModel> purchaseStocks;
        public int rowIndex = 0;
        public int RowId;
        public List<StockModel> _deletestocks = new List<StockModel>();
        public PurchaseStockModel _selectedStock;
        BrushConverter color = new BrushConverter();
        public PurchaseOrderModel _purchaseData;
        private int _noOfErrorsOnScreen = 0;
        TaxController taxController = new TaxController();

        private int _noOfErrorsOnReason = 0;
        ResourceDictionary myResourceDictionary;
        public string header = "Purchase";
        private int _supplierCode;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        private IList<TaxModel> _taxdetails;
        private string _taxValue;
        public EditPurchase(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            var reasonSet = (PurchaseStockModel)Application.Current.Resources["purchaseStock"];
            reasonSet.ReasonForRetrun = "";

            // a = new PurchaseStockModel();
            //  btnReturn.IsEnabled = false;
            //  this.Resources["purchaseStock"] = new PurchaseStockModel();
            myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/ResourceFiles/En.xaml",
                        UriKind.RelativeOrAbsolute);
            _supliers = controller.GetSuppliers().ToList();
            SupplierModel suplier = _supliers?.FirstOrDefault(x => x.Id == row.SuplierCode);
            _supplierCode = row.SuplierCode;
            supplier_name.Text = row.SuplierName;
            supplier_mobile.Text = Convert.ToString(suplier?.Mobile);
            txt_suplierAddress.Text = Convert.ToString(suplier?.Address);
            purchase_expiryDate.Text = Convert.ToString(row.ExpiryDate);
            purchase_deliveryDate.Text = Convert.ToString(row.DeliveryDate);
            purchase_invoiceDate.Text = row.InvoiceDate;
            purchase_invoiceNo.Text = row.InvoiceNo;
            purchase_cashdiscount.Text = Convert.ToString(row.DiscountPercentage);
            purchase_cashdiscountDoller.Text = Convert.ToString(row.DiscountAmount);

            purchase_cashSubChargeAmo.Text = Convert.ToString(row.SurChargeAmount);
            purchase_pono.Text = Convert.ToString(row.PurchaseId);
            RowId = row.PurchaseId;
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
            _orignalLstStocks = purchaseController.GetStocksByPurchaseId(row.PurchaseId);
            _orignalLstPurchaseRetrun = purchaseController.GetPurchaseReturns(row.PurchaseId);
            _purchaseData = row;
             //_supliers = controller.GetSuppliers().ToList();
            arrow.IsEnabled = false;
            supplier_name.IsEnabled = false;
            supplier_mobile.IsEnabled = false;
            txt_suplierAddress.IsEnabled = false;
            ResponseVm responseTax = taxController.GetTax();
            if (responseTax.FaultData == null)
            {
                TaxModel objTaxModel = new TaxModel(0, "Select", 0, "", "", "", "");
                _taxdetails = responseTax.Response.Cast<TaxModel>().ToList();
                _taxdetails.Insert(0, objTaxModel);
                this.cmbTax.ItemsSource = _taxdetails;
                this.cmbTax.DisplayMemberPath = "TaxDetail";
                this.cmbTax.SelectedValuePath = "TaxCode";
                cmbTax.IsEnabled = false;
                // this.cmbTax.SelectedIndex = 0;
            }
          //  _taxValue = Convert.ToString(row.TaxPercentage);
            if (row.TaxPercentage != null && Convert.ToDecimal(row.TaxPercentage) != Convert.ToDecimal(0) && _taxdetails.Any(item => Convert.ToDouble(item.Rate) == Convert.ToDouble(_taxValue)))
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.Text = Convert.ToString(_taxdetails.FirstOrDefault(item => item.Rate == Convert.ToDouble(_taxValue)).TaxDetail);
            }
            else
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.SelectedIndex = 0;
            }
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
            // if ()
            //  {
            //  additems();
            //  }
            //  else
            //  {

            //  }
            //  btn_Save.IsEnabled = true;
            List<PurchaseStockModel> purchaseStockData = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
            purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockData);
            lstPurchase.ItemsSource = purchaseStocks;
            ProductAmount amount = CommonFunctions.RetrunProductAmount(purchaseStockData, Convert.ToString(row.DiscountPercentage), purchase_cashdiscountDoller.Text, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
            netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
            totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

            subChargeAmount.Text = Convert.ToString(row.SurChargeAmount);
            discountChargeAmount.Text = Convert.ToString(row.DiscountPercentage);
            taxChargeAmount.Text = Convert.ToString(row.TaxPercentage);
            btn_AddRow.IsEnabled = true;
            ;
            if (row.Status == (int)CommonEnum.PurchaseStatus.WaitingForApproval)
            {
                btn_Save.IsEnabled = true;
                btnApproval.IsEnabled = true;
                btnReturn.IsEnabled = false;
                btn_remove.IsEnabled = false;
                btn_remove.Background = Brushes.Gray;
                btnReturn.Background = Brushes.Gray;
                btnPrint.IsEnabled = true;
                lstPurchase.Visibility = Visibility.Visible;
                lstPurchaseWithReturned.Visibility = Visibility.Collapsed;
                cmbTax.IsEnabled = true;
            }
            else if (row.Status == (int)CommonEnum.PurchaseStatus.Approved || row.Status == (int)CommonEnum.PurchaseStatus.Returned)
            {
                //QuantityColumn.Content = (string)Application.Current.Resources["purchase_returnedqty"];
                //QuantityColumn.Resources= (ResourceDictionary)Application.Current.Resources["supplier_requiredFields"];
                //QuantityColumn.Name = "Return Quantity";
                //btnReturn.IsEnabled = false;
                btnReturn.IsEnabled = true;
                btn_Save.IsEnabled = false;
                btn_Save.Background = Brushes.Gray;
                btn_AddRow.IsEnabled = false;
                btn_remove.IsEnabled = false;
                btnApproval.IsEnabled = false;
                btnPrint.IsEnabled = true;
                btnApproval.Background = Brushes.Gray;
                btn_AddRow.Background = Brushes.Gray;
                btn_remove.Background = Brushes.Gray;
                lstPurchase.Visibility = Visibility.Collapsed;
                lstPurchaseWithReturned.Visibility = Visibility.Visible;
                DisableTextBox();
                var retrunedPurchaseStocks = GetGroupOfPurchaseStocks(purchaseStocks);
                lstPurchaseWithReturned.ItemsSource = retrunedPurchaseStocks;
                List<PurchaseStockModel> stocksofetrunedPurchased = new List<PurchaseStockModel>(retrunedPurchaseStocks);
                ProductAmount retrunedAmount = CommonFunctions.RetrunProductAmountWithReturnProduct(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), purchase_cashdiscountDoller.Text, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));

                //  subChargeAmount.Text = Convert.ToString(row.SurChargeAmount);
                //  discountChargeAmount.Text = Convert.ToString(row.DiscountPercentage);
                //  taxChargeAmount.Text = Convert.ToString(row.TaxPercentage);


            }
            else if (row.Status == (int)CommonEnum.PurchaseStatus.FullyReturned)
            {
                btnPrint.IsEnabled = true;
                btnReturn.IsEnabled = false;
                btnApproval.IsEnabled = false;
                btn_Save.IsEnabled = false;
                //btnPrint.Background = Brushes.Gray;
                btn_Save.Background = Brushes.Gray;
                btnApproval.Background = Brushes.Gray;
                btnReturn.Background = Brushes.Gray;
                btn_AddRow.IsEnabled = false;
                btn_remove.IsEnabled = false;
                btn_AddRow.Background = Brushes.Gray;
                btn_remove.Background = Brushes.Gray;
                lstPurchase.Visibility = Visibility.Collapsed;
                lstPurchaseWithReturned.Visibility = Visibility.Visible;
                DisableTextBox();
                ObservableCollection<PurchaseStockModel> pp = new ObservableCollection<PurchaseStockModel>();
                var retrunedPurchaseStocks = GetGroupOfPurchaseStocks(purchaseStocks);
                lstPurchaseWithReturned.ItemsSource = retrunedPurchaseStocks;
                // lstPurchaseWithReturned.Items.Clear();
                // lstPurchaseWithReturned.ItemsSource = pp;
                List<PurchaseStockModel> stocksofetrunedPurchased = new List<PurchaseStockModel>(retrunedPurchaseStocks);
                ProductAmount retrunedAmount = CommonFunctions.RetrunProductAmountWithReturnProduct(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), purchase_cashdiscountDoller.Text, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                netAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.NetTotal))); //CommonFunction.Common.RoundOff(amount.NetTotal.ToString());
                totalAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(amount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(amount.TotalAmount)));


                //purchaseStocks.GroupBy(x => new { x.ProductCode, x.BatchNo }).Select(y => new PurchaseStockModel(y.FirstOrDefault().ProductCode, y.FirstOrDefault().ProductName
                //   , y.Sum(z => z.Quantity), y.FirstOrDefault().CostPrice, y.FirstOrDefault().Discount, y.FirstOrDefault().BatchNo, 0,
                //   _orignalLstPurchaseRetrun.Any() && _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo) != null ?
                //   _orignalLstPurchaseRetrun.Where(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo).Sum(z => z.Quantity) : 0,
                //  0, 00));
            }
            //else if(row.Status == (int)CommonEnum.PurchaseStatus.DirectPurchased)
            //{
            //    btnReturn.Visibility = Visibility.Collapsed;
            //    btnApproval.Visibility = Visibility.Collapsed;
            //    btn_Save.IsEnabled = true;
            //    btn_AddRow.Visibility = Visibility.Visible;
            //    btn_remove.Visibility = Visibility.Visible;
            //    lstPurchase.Visibility = Visibility.Visible;
            //    lstPurchaseWithReturned.Visibility = Visibility.Collapsed;
            //}
            else
            {
                btn_Save.IsEnabled = true;
                btnApproval.IsEnabled = false;
                btnReturn.IsEnabled = false;
                lstPurchaseWithReturned.Visibility = Visibility.Collapsed;
                lstPurchase.Visibility = Visibility.Visible;
            }


        }
        public void ChangeHeightWidth()
        {
            this.EditPoPage.Height = HeightWidth.Height - 65;
            this.EditPoPage.Width = HeightWidth.width;

        }
        //private int GetRetrunedQuantity(int quantity, int returedQuantity)
        //{
        //   // int orignalQuantity= returedQuantity == 0 ? 0 : quantity - returedQuantity;
        //    return orignalQuantity;// y.Sum(f => f.Quantity) - _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().productCode && d.BatchNo == y.FirstOrDefault().BatchNo).Quantity)
        //}

        //    private ProductAmount GetProductAmount()
        //{
        //    ProductAmount amount = CommonFunctions.RetrunProductAmount(purchaseStockData, Convert.ToString(row.DiscountPercentage), purchase_cashdiscountDoller.Text, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
        //    netAmount.Text = amount.NetTotal;
        //    totalAmount.Text = amount.TotalAmount;
        //    subChargeAmount.Text = Convert.ToString(row.SurChargeAmount);
        //    discountChargeAmount.Text = Convert.ToString(row.DiscountPercentage);
        //    taxChargeAmount.Text = Convert.ToString(row.TaxPercentage);
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
            cmbTax.IsEnabled = false;// purchase_cashTaxAmount.IsEnabled = false;
            purchase_cashSubChargeAmo.IsEnabled = false;
            purchase_pono.IsEnabled = false;
            arrow.IsEnabled = false;
        }

        private ObservableCollection<PurchaseStockModel> GetGroupOfPurchaseStocks(ObservableCollection<PurchaseStockModel> purchaseStocks)
        {
            return new ObservableCollection<PurchaseStockModel>(purchaseStocks.GroupBy(x => new { x.ProductCode, x.BatchNo }).Select(y => new PurchaseStockModel(y.FirstOrDefault().ProductCode, y.FirstOrDefault().ProductName
                      , y.Sum(z => z.Quantity), y.FirstOrDefault().CostPrice, y.FirstOrDefault().Discount, y.FirstOrDefault().BatchNo, 0,
                      _orignalLstPurchaseRetrun.Any() && _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo) != null ?
                      _orignalLstPurchaseRetrun.Where(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo).Sum(z => z.Quantity) : 0,
                     0, 00, y.FirstOrDefault().ReasonForRetrun)));
        }
        private string GetProductName(int productCode)
        {
            return  _products.FirstOrDefault(x => x.Id.Value == productCode).ItemName;
        }
        public void additems()
        {
            purchaseStocks = new ObservableCollection<PurchaseStockModel>();
            purchaseStocks.Add(new PurchaseStockModel() { ProductName = "" });
        }
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

        private void supplier_name_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("supplier_name_KeyUp");
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
            cmbTax.SelectedIndex = 0;//purchase_cashTaxAmount.Text = "";
            purchase_cashSubChargeAmo.Text = "";
            _taxValue = "";
        }
        private void lstPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            //  dynamic row = e.Source;
            btn_remove.IsEnabled = true;
            btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        }
        private void lstRetunPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            //  dynamic row = e.Source;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            var a = (PurchaseStockModel)Application.Current.Resources["purchaseStock"];
            a = new PurchaseStockModel();
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
                _deletestocks.Add(new StockModel(_selectedStock.StockId, null, _selectedStock.Quantity, _selectedStock.CostPrice, _selectedStock.SellingPrice, _selectedStock.MRP, _selectedStock.Discount, _selectedStock.BatchNo, _selectedStock.ProductCode, _purchaseData.PurchaseId.Value));
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            //  btn_remove.IsEnabled = false;
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
                dynamic row = new PurchaseOrderModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                    , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
                    , string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.Status, _purchaseData.ApprovalDate, _purchaseData.ApprovedBy, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName, _purchaseData.StatusName, purchase_invoiceNo.Text, purchase_invoiceDate.Text);
                var result = purchaseController.UpdateStatus(row, _purchaseData.Status);
                newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                {
                    return new StockModel(x.StockId == null ? 0 : x.StockId, null, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.Discount, x.BatchNo, x.ProductCode, _purchaseData.PurchaseId.Value);
                    // newStocks.Add(stock);
                }).ToList());

                purchaseController.UpdateStocks(newStocks, _deletestocks);
                //  ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_updated"], header, false);
                //  form.ShowDialog();
                Common.Notification((string)myResourceDictionary["purchase_updated"], header, false);
                ClearData();
                Purchase form1 = new Purchase();
                NavigationService.Navigate(form1);
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_requiredFields"], header, false);
                form.ShowDialog();
                // Common.ErrorNotification((string)myResourceDictionary["purchase_requiredFields"], header, false);
            }
        }

        private void Purchase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnReason == 0;
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
                _deletestocks.Add(new StockModel(purchase.StockId, null, purchase.Quantity, purchase.CostPrice, purchase.SellingPrice, purchase.MRP, purchase.Discount, purchase.BatchNo, purchase.ProductCode, _purchaseData.PurchaseId.Value));
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

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            List<StockModel> newStocks = new List<StockModel>();
            List<PurchaseStockModel> incoreectQuantityLst = new List<PurchaseStockModel>();
            List<PurchaseStockModel> Stocks = lstPurchaseWithReturned.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            var dd = lstPurchaseWithReturned.Items;
            List<PurchaseReturnModel> returnedPurchaseModel = new List<PurchaseReturnModel>();
            int returnedCount = 0;
            int reasonFieldCount = 0;
            Stocks.ForEach(x =>
            {
                // var oldstock = _orignalLstStocks.FirstOrDefault(z => z.StockId == x.StockId);
                if (x.Quantity - x.ReturnedQuantity < x.QuantityForRetrun)
                    incoreectQuantityLst.Add(new PurchaseStockModel(x.productCode, x.ProductName, x.Quantity, x.CostPrice, x.Discount, x.BatchNo, x.QuantityForRetrun, x.ReturnedQuantity, x.PurchaseRetrunId, (int)x.SellingPrice, x.ReasonForRetrun));
                else if (x.ReturnedQuantity == x.Quantity || x.ReturnedQuantity + x.QuantityForRetrun == x.Quantity)
                    returnedCount++;
                if (string.IsNullOrEmpty(x.ReasonForRetrun) && x.QuantityForRetrun != 0)
                    reasonFieldCount++;
            });
            if (incoreectQuantityLst.Any())
            {
                string quantity = string.Empty;
                foreach (var item in incoreectQuantityLst)
                {
                    quantity += "Product : " + item.ProductName
                     + "  Quantity : " + item.Quantity + "\n" + " your limit of return products exceed the quantity\n";
                }
                ConfirmationPopup form = new ConfirmationPopup(quantity, header, false);
                form.ShowDialog();
            }
            else if (reasonFieldCount > 0)
            {
                ConfirmationPopup form = new ConfirmationPopup("Reason is required for every retruned product", header, false);
                form.ShowDialog();
            }

            else
            {
                if (Stocks.Any(x => x.ProductCode > 0 && x.QuantityForRetrun > 0))
                {
                    if (returnedCount == Stocks.Count)
                        purchaseController.UpdateStatus(_purchaseData, (int)CommonEnum.PurchaseStatus.FullyReturned);
                    else
                        purchaseController.UpdateStatus(_purchaseData, (int)CommonEnum.PurchaseStatus.Returned);
                    returnedPurchaseModel.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.QuantityForRetrun > 0).Select(x =>
                        {
                            return new PurchaseReturnModel(x.PurchaseRetrunId, _purchaseData.PurchaseId.Value, x.ProductCode, x.BatchNo, x.QuantityForRetrun, UserModelVm.UserId, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()), x.ReasonForRetrun);
                            // newStocks.Add(stock);
                        }).ToList());

                    purchaseController.UpdatePurchaseRetrun(returnedPurchaseModel);
                    // ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_retunedmsg"], header, false);
                    // form.ShowDialog();
                    Common.Notification((string)myResourceDictionary["purchase_retunedmsg"], header, false);
                    ClearData();
                    Purchase form1 = new Purchase();
                    NavigationService.Navigate(form1);
                }
                else
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_returnQty_Zero"], header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification((string)myResourceDictionary["purchase_requiredFields"], header, false);
                }

            }
        }

        private void btnApproval_Click(object sender, RoutedEventArgs e)
        {
            List<StockModel> newStocks = new List<StockModel>();
            List<PurchaseStockModel> Stocks = lstPurchase.Items.Cast<PurchaseStockModel>().Select(x => x).ToList();
            if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
            {
                decimal? nullval = null;
                DateTime? nulldate = null;
                int? nullint = null;
                dynamic row = new PurchaseOrderModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                    , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
                    , string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.Status, _purchaseData.ApprovalDate, _purchaseData.ApprovedBy, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName, _purchaseData.StatusName, purchase_invoiceNo.Text, purchase_invoiceDate.Text);
                var result = purchaseController.UpdateStatus(row, (int)CommonEnum.PurchaseStatus.Approved);
                newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                {
                    return new StockModel(x.StockId == null ? 0 : x.StockId, null, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.Discount, x.BatchNo, x.ProductCode, _purchaseData.PurchaseId.Value);
                    // newStocks.Add(stock);
                }).ToList());

                purchaseController.UpdateStocks(newStocks, _deletestocks);
                // ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_approvedmsg"], header, false);
                /// form.ShowDialog();
                Common.Notification((string)myResourceDictionary["purchaseOrder_approvedmsg"], header, false);
                ClearData();
                Purchase form1 = new Purchase();
                NavigationService.Navigate(form1);
            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_requiredFields"], header, false);
                form.ShowDialog();
                // Common.ErrorNotification((string)myResourceDictionary["purchase_requiredFields"], header, false);
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

        private void txt_ErrorForReason(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnReason--;
            else
                _noOfErrorsOnReason++;
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
                    //  Common.ErrorNotification((string)Application.Current.Resources["invalid_DiscountSupplier"], header, false);
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

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            CommandManager.InvalidateRequerySuggested();
            e.Handled = true;
        }

        //private void reason_KeyUp(object sender, KeyEventArgs e)
        //{
        //    var txt = sender as TextBox;
        //    if (_noOfErrorsOnScreen <= 0)
        //        btnReturn.IsEnabled = true;
        //}

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
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
        private void btnPrinterPopupOk_Click(object sender, RoutedEventArgs e)
        {

            if (cmbType.SelectedIndex > 0)
            {
                if (cmbType.Text == "Approved")
                {
                    this.printPopUp.IsOpen = false;
                    decimal? nullval = null;
                    DateTime? nulldate = null;
                    dynamic row = new PurchaseOrderModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                           , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
                           , string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.Status, _purchaseData.ApprovalDate, _purchaseData.ApprovedBy, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName, _purchaseData.StatusName, purchase_invoiceNo.Text, purchase_invoiceDate.Text);

                    invoice form = new invoice(row, "Purchase", cmbType.Text);

                }
                else
                {
                    this.printPopUp.IsOpen = false;
                    decimal? nullval = null;
                    DateTime? nulldate = null;
                    dynamic row = new PurchaseOrderModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : CommonFunctions.ParseDateToFinclave(purchase_expiryDate.Text)
                           , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
                           , string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.Status, _purchaseData.ApprovalDate, _purchaseData.ApprovedBy, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName, _purchaseData.StatusName, purchase_invoiceNo.Text, purchase_invoiceDate.Text);

                    invoice form = new invoice(row, "Purchase", cmbType.Text);
                }

            }

            //List<PurchaseOrderModel> row = _purchase.Where(item => item.PurchaseId == Convert.ToInt32(cmbPO.Text)).ToList();
            //if (row.Count > 0)
            //{
            //    PurchaseOrderModel obj = row[0];
            //    invoice form = new invoice(obj, "Purchase");
            //}
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            this.printPopUp.IsOpen = true;
            //decimal? nullval = null;
            //DateTime? nulldate = null;
            //dynamic row = new PurchaseOrderModel(_purchaseData.PurchaseId, _purchaseData.PurchaseDate, _supplierCode, string.IsNullOrEmpty(purchase_cashdiscount.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscount.Text), string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? nullval : Convert.ToDecimal(purchase_cashdiscountDoller.Text), string.IsNullOrEmpty(purchase_deliveryDate.Text) ? nulldate : Convert.ToDateTime(purchase_deliveryDate.Text), string.IsNullOrEmpty(purchase_expiryDate.Text) ? nulldate : Convert.ToDateTime(purchase_expiryDate.Text)
            //       , string.IsNullOrEmpty(purchase_cashSubChargeAmo.Text) ? nullval : Convert.ToDecimal(purchase_cashSubChargeAmo.Text)
            //       , string.IsNullOrEmpty(purchase_cashTaxAmount.Text) ? nullval : Convert.ToDecimal(purchase_cashTaxAmount.Text), _purchaseData.CreatedBy, _purchaseData.CreatedDate, _purchaseData.Status, _purchaseData.ApprovalDate, _purchaseData.ApprovedBy, _purchaseData.CompanyCode, _purchaseData.BranchCode, _purchaseData.SuplierName, _purchaseData.StatusName, purchase_invoiceNo.Text, purchase_invoiceDate.Text);

            //invoice form = new invoice(row, "Purchase");
            //  NavigationService.Navigate(form);          
        }
        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                //  var border = (resultStack.Parent as ScrollViewer).Parent as Border;
                var data = _supliers;
                lbAutoPurchaseWithName.Items.Clear();
                string query = supplier_name.Text;

                if (query.Length == 0)
                {
                    if (callingMethod == "arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            // The word starts with this... Autocomplete must work 
                            addProductCode(obj);
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

                    //lbAutoPurchase.Visibility = Visibility.Collapsed;
                    //  txt_SuplierCode.Text = "";
                    txt_suplierAddress.Text = "";
                    supplier_mobile.Text = "";
                    supplier_name.Text = "";
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (Convert.ToString(obj.Id).ToLower().Contains(query.ToLower()))
                        {
                            // The word starts with this... Autocomplete must work 
                            addProductCode(obj);
                            found = true;
                        }
                    }
                    lbAutoPurchaseWithName.IsEnabled = true;
                    lbAutoPurchaseWithName.Visibility = Visibility.Visible;
                }

                // Clear the list 


                // Add the result 


                if (!found)
                {
                    lbAutoPurchaseWithName.Items.Add(new TextBlock() { Text = "No results found." });
                    lbAutoPurchaseWithName.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            lbAutoPurchaseWithName.IsEnabled = false;
            lbAutoPurchaseWithName.Visibility = Visibility.Collapsed;
        }
        private void btn_showPop_Click(object sender, RoutedEventArgs e)
        {
            printPopUp.IsOpen = true;
        }



        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void closePrintpop_Click(object sender, RoutedEventArgs e)
        {
            this.printPopUp.IsOpen = false;
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

        private void purchase_cashdiscountDoller_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void GridViewColumnHeader_Click_3(object sender, RoutedEventArgs e)
        {

        }
        private void GridViewColumnHeader_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void purchase_pono_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click_4(object sender, RoutedEventArgs e)
        {

        }
    }
}

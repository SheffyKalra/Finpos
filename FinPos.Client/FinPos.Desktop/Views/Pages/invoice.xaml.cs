using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for invoice.xaml
    /// </summary>
    public partial class invoice : Page
    {
        SupplierController controller = new SupplierController();
        ProductController productController = new ProductController();
        PurchaseController purchaseController = new PurchaseController();

        private List<DomainContracts.DataContracts.SupplierModel> _supliers;
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
        public string header = "Purchase";
        private int _supplierCode;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        public invoice(dynamic row, string callingPage, string recieptType)
        {
            InitializeComponent();
            try
            {
                if (callingPage == "Purchase")
                {
                    if (recieptType == "Approved")
                    {
                        grdPrintPurchase.Visibility = Visibility.Visible;
                        grdReturnProducts.Visibility = Visibility.Collapsed;
                        _supliers = controller.GetSuppliers().ToList();
                        SupplierModel suplier = _supliers?.FirstOrDefault(x => x.Id == row.SuplierCode);
                        _supplierCode = row.SuplierCode;
                        ResponseVm responce = productController.GetProductsByCompanyAndBranch();
                        _products = responce.Response.Cast<ProductModel>().ToList();
                        _orignalLstStocks = purchaseController.GetStocksByPurchaseId(row.PurchaseId);
                        _orignalLstPurchaseRetrun = purchaseController.GetPurchaseReturns(row.PurchaseId);
                        if (row.Status == (int)CommonEnum.PurchaseStatus.WaitingForApproval)
                        {
                            BindItems(row, suplier);
                        }
                        else
                        {
                            txtFromPhone.Text = string.Empty;
                            txtFromAddress.Text = "Any";
                            txtFromCity.Text = "Mohali";  //Convert.ToString(suplier?.);
                                                          // txtFromEmail.Text = "Finpos@finpos.com";
                            txtToPhone.Text = Convert.ToString(suplier?.Mobile);
                            txtToAddress.Text = Convert.ToString(suplier?.Address);
                            txtToCity.Text = string.Empty; //Convert.ToString(suplier?.);
                                                           // txtToEmail.Text = suplier?.Email;
                            List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
                            purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockDataWIthPurchaseStockModel);
                            var retrunedPurchaseStocks = GetGroupOfPurchaseStocks(purchaseStocks);
                            List<PurchaseStockModel> stocksofetrunedPurchased = new List<PurchaseStockModel>(retrunedPurchaseStocks);
                            ProductAmount retrunedAmount = CommonFunctions.RetrunProductAmountWithReturnProduct(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                            List<PrintPurchaseStockModel> purchaseStockData = stocksofetrunedPurchased.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, (x.Quantity - x.ReturnedQuantity) * x.CostPrice)).ToList();
                            //  _orignalLstStocks = purchaseController.GetStocks(row.PurchaseId);
                            //  List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
                            // List<PrintPurchaseStockModel> purchaseStockData = _orignalLstStocks.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.Quantity * x.CostPrice)).ToList();
                            // purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockData);
                            //ProductAmount amount = CommonFunctions.RetrunProductAmount(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                            SetTextToTextBoxes(row, retrunedAmount);
                            lstPrintPurchase.ItemsSource = purchaseStockData;
                            Print();
                        }
                    }
                    else
                    {
                        List<DomainContracts.DataContracts.PrintPurchaseReturnModel> objPrintModel = new List<DomainContracts.DataContracts.PrintPurchaseReturnModel>();
                        grdPrintPurchase.Visibility = Visibility.Collapsed;
                        grdReturnProducts.Visibility = Visibility.Visible;
                        _supliers = controller.GetSuppliers().ToList();
                        SupplierModel suplier = _supliers?.FirstOrDefault(x => x.Id == row.SuplierCode);
                        _supplierCode = row.SuplierCode;
                        ResponseVm responce = productController.GetProductsByCompanyAndBranch();
                        _products = responce.Response.Cast<ProductModel>().ToList();
                        _orignalLstStocks = purchaseController.GetStocksByPurchaseId(row.PurchaseId);
                        _orignalLstPurchaseRetrun = purchaseController.GetPurchaseReturns(row.PurchaseId);
                        txtFromPhone.Text = Convert.ToString(suplier?.Mobile);
                        txtFromAddress.Text = Convert.ToString(suplier?.Address);
                        txtFromCity.Text = string.Empty; //Convert.ToString(suplier?.);
                                                         //txtFromEmail.Text = suplier?.Email;
                        txtToPhone.Text = string.Empty;
                        txtToAddress.Text = "Any";
                        txtToCity.Text = "Mohali"; //Convert.ToString(suplier?.);
                                                   //  txtToEmail.Text = "Finpos@finpos.com";
                        List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
                        purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockDataWIthPurchaseStockModel);
                        var retrunedPurchaseStocks = GetGroupOfPurchaseStocks(purchaseStocks);
                        List<PurchaseStockModel> stocksofetrunedPurchased = new List<PurchaseStockModel>(retrunedPurchaseStocks);
                        ProductAmount retrunedAmount = CommonFunctions.RetrunProductAmountOfReturnProduct(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                        List<PrintPurchaseStockModel> purchaseStockData = stocksofetrunedPurchased.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, (x.ReturnedQuantity) * x.CostPrice)).ToList();
                        objPrintModel = purchaseStockData.Select(x => new DomainContracts.DataContracts.PrintPurchaseReturnModel(x.PrintProductCode, x.PrintProductName, x.PrintQuantity, x.PrintCostPrice, x.PrintTotal, _orignalLstPurchaseRetrun.Count > 0 ? _orignalLstPurchaseRetrun.FirstOrDefault(z => z.ProductCode == x.PrintProductCode).Reason : string.Empty, _orignalLstPurchaseRetrun.Count > 0 ? _orignalLstPurchaseRetrun.FirstOrDefault(z => z.ProductCode == x.PrintProductCode).Quantity : 0)).ToList();
                        SetTextToTextBoxes(row, retrunedAmount);
                        lstPrintReturnPurchase.ItemsSource = objPrintModel;
                        Print();
                        // BindItems(row, suplier);
                    }
                }
                else if (callingPage == "DirectPurchase")
                {
                    grdPrintPurchase.Visibility = Visibility.Visible;
                    grdReturnProducts.Visibility = Visibility.Collapsed;
                    _supliers = controller.GetSuppliers().ToList();
                    SupplierModel suplier = _supliers?.FirstOrDefault(x => x.Id == row.SuplierCode);
                    _supplierCode = row.SuplierCode;
                    ResponseVm responce = productController.GetProductsByCompanyAndBranch();
                    _products = responce.Response.Cast<ProductModel>().ToList();
                    _orignalLstStocks = purchaseController.GetDirectStocks(row.PurchaseId);
                    txtFromPhone.Text = string.Empty;
                    txtFromAddress.Text = "Any";
                    txtFromCity.Text = "Mohali";  //Convert.ToString(suplier?.);
                                                  // txtFromEmail.Text = "Finpos@finpos.com";
                    txtToPhone.Text = Convert.ToString(suplier?.Mobile);
                    txtToAddress.Text = Convert.ToString(suplier?.Address);
                    txtToCity.Text = string.Empty; //Convert.ToString(suplier?.);
                                                   // txtToEmail.Text = suplier?.Email;
                    List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();



                    purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockDataWIthPurchaseStockModel);
                    var retrunedPurchaseStocks = GetGroupOfPurchaseStocksForDirectPurchase(purchaseStocks);
                    List<PurchaseStockModel> stocksofetrunedPurchased = new List<PurchaseStockModel>(retrunedPurchaseStocks);
                    ProductAmount retrunedAmount = CommonFunctions.RetrunProductAmountWithReturnProduct(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                    List<PrintPurchaseStockModel> purchaseStockData = stocksofetrunedPurchased.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, (x.Quantity - x.ReturnedQuantity) * x.CostPrice)).ToList();
                    //  _orignalLstStocks = purchaseController.GetStocks(row.PurchaseId);
                    //  List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
                    // List<PrintPurchaseStockModel> purchaseStockData = _orignalLstStocks.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.Quantity * x.CostPrice)).ToList();
                    // purchaseStocks = new ObservableCollection<PurchaseStockModel>(purchaseStockData);
                    //ProductAmount amount = CommonFunctions.RetrunProductAmount(stocksofetrunedPurchased, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
                    SetTextToTextBoxes(row, retrunedAmount);
                    lstPrintPurchase.ItemsSource = purchaseStockData;
                    Print();
                }


            }
            catch (Exception ex)
            {


            }
        }

        private void SetTextToTextBoxes(dynamic row, ProductAmount retrunedAmount)
        {
            txtPrintNetAmount.Text = Convert.ToString(CommonFunction.Common.RoundOff(retrunedAmount.NetTotal == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(retrunedAmount.NetTotal)));
            txtPrintSubtotal.Text = Convert.ToString(CommonFunction.Common.RoundOff(retrunedAmount.TotalAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(retrunedAmount.TotalAmount)));
            txtPrintSubCharge.Text = Convert.ToString(CommonFunction.Common.RoundOff(row.SurChargeAmount == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(row.SurChargeAmount)));
            txtPrintDiscount.Text = Convert.ToString(CommonFunction.Common.RoundOff(row.DiscountPercentage == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(row.DiscountPercentage)));
            txtPrintTax.Text = Convert.ToString(CommonFunction.Common.RoundOff(row.TaxPercentage == null ? Convert.ToDecimal("0.00") : Convert.ToDecimal(row.TaxPercentage)));
        }

        private void BindItems(dynamic row, SupplierModel suplier)
        {
            txtFromPhone.Text = Convert.ToString(suplier?.Mobile);
            txtFromAddress.Text = Convert.ToString(suplier?.Address);
            txtFromCity.Text = string.Empty; //Convert.ToString(suplier?.);
                                             //txtFromEmail.Text = suplier?.Email;
            txtToPhone.Text = string.Empty;
            txtToAddress.Text = "Any";
            txtToCity.Text = "Mohali"; //Convert.ToString(suplier?.);
                                       //  txtToEmail.Text = "Finpos@finpos.com";
            List<PurchaseStockModel> purchaseStockDataWIthPurchaseStockModel = _orignalLstStocks.Select(x => new PurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, Convert.ToInt32(x.ItemTaxPercentage), x.BatchNo, x.StockId)).ToList();
            List<PrintPurchaseStockModel> purchaseStockData = _orignalLstStocks.Select(x => new PrintPurchaseStockModel(x.ProductCode, GetProductName(Convert.ToInt32(x.ProductCode)), x.Quantity, x.CostPrice, x.Quantity * x.CostPrice)).ToList();

            ProductAmount amount = CommonFunctions.RetrunProductAmount(purchaseStockDataWIthPurchaseStockModel, Convert.ToString(row.DiscountPercentage), string.Empty, Convert.ToString(row.TaxPercentage), Convert.ToString(row.SurChargeAmount));
            SetTextToTextBoxes(row, amount);
            lstPrintPurchase.ItemsSource = purchaseStockData;
            Print();
        }

        private void Print()
        {
            try
            {
                PrintDialog printDlg = new PrintDialog();
                if (printDlg.ShowDialog() == true)
                {
                    System.Printing.PrintCapabilities capabilities =
                 printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                    //get scale of the print wrt to screen of WPF visual
                    double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                                   this.ActualHeight);
                    Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                    this.Height = capabilities.PageImageableArea.ExtentHeight;
                    this.Width = capabilities.PageImageableArea.ExtentWidth;
                    this.Measure(sz);
                    //form.Width = 850;
                    //this.Arrange(new Rect(new Point(0, 0), this.DesiredSize));
                    // myPanel.Children.Add(form);
                    printDlg.PrintVisual(this, "Print Document");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private ObservableCollection<PurchaseStockModel> GetGroupOfPurchaseStocks(ObservableCollection<PurchaseStockModel> purchaseStocks)
        {
            return new
                ObservableCollection<PurchaseStockModel>(purchaseStocks.GroupBy(x => new { x.ProductCode, x.BatchNo })?
                .Select(y => new PurchaseStockModel(y.FirstOrDefault().ProductCode, y.FirstOrDefault().ProductName
                      , y.Sum(z => z.Quantity), y.FirstOrDefault().CostPrice, y.FirstOrDefault().Discount,
                      y.FirstOrDefault().BatchNo, 0,
                      _orignalLstPurchaseRetrun.Any() &&
                      _orignalLstPurchaseRetrun.FirstOrDefault(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo) != null ?

                      _orignalLstPurchaseRetrun.Where(d => d.ProductCode == y.FirstOrDefault().ProductCode && d.BatchNo == y.FirstOrDefault().BatchNo).Sum(z => z.Quantity) : 0,
                     0, 00, y.FirstOrDefault().ReasonForRetrun)));
        }
        private ObservableCollection<PurchaseStockModel> GetGroupOfPurchaseStocksForDirectPurchase(ObservableCollection<PurchaseStockModel> purchaseStocks)
        {
            return new
                ObservableCollection<PurchaseStockModel>(purchaseStocks.GroupBy(x => new { x.ProductCode, x.BatchNo })?
                .Select(y => new PurchaseStockModel(y.FirstOrDefault().ProductCode, y.FirstOrDefault().productName, y.Sum(z => z.Quantity),
                y.FirstOrDefault().CostPrice, y.FirstOrDefault().SellingPrice, y.FirstOrDefault().MRP, y.FirstOrDefault().Discount,
                      y.FirstOrDefault().BatchNo, y.FirstOrDefault().StockId)));
        }
        private string GetProductName(int productCode)
        {
            return _products?.FirstOrDefault(x => x.Id.Value == productCode).ItemName;
        }
    }
}

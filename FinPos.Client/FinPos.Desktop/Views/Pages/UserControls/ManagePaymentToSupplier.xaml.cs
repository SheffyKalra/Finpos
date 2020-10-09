using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
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

namespace FinPos.Client.Views.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for ManagePaymentToSupplier.xaml
    /// </summary>
    public partial class ManagePaymentToSupplier : UserControl
    {
        // public CommonEnums.PaymentType PaymentType { get; set; }
        SupplierController controller = new SupplierController();
        PurchaseController controllerPurchase = new PurchaseController();
        public int? paymentType = null;
        private List<PurchaseModel> _directPurchase;
        private List<PurchaseOrderModel> _purchaseOrder;
        private List<SupplierModel> _supliers;
        public int purchaseType;
        public int supplierCode;
        public decimal? TotalCostPrice;
        public decimal? PaidAmount;
        private int _noOfErrorsOnScreen = 0;
        public ManagePaymentToSupplier()
        {
            InitializeComponent();
            ClearFields();
            _supliers = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId, UserModelVm.BranchId).ToList();
            DisableInvoiceType();
            //   _purchaseOrder = controllerPurchase.GetPurchaseByCompanyAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
            ///  _directPurchase = controllerPurchase.GetDirectPurchaseByCompanyAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
        }
        public void DisableInvoiceType()
        {
            if (invoiceNo_ != null)
            {
                invoiceNo_.IsEnabled = false;
                arrowInvoice.IsEnabled = false;
            }

        }
        public void EnableInvoiceType()
        {
            invoiceNo_.IsEnabled = true;
            arrowInvoice.IsEnabled = true;

        }
        public void ClearFields()
        {
            if (invoiceNo_ != null)
            {
                this.invoiceNo_.Text = string.Empty;
                this.supplierName_.Text = string.Empty;
                this.paymentDate_.Text = string.Empty;
                this.bank_.Text = string.Empty;
                this.amount_.Text = string.Empty;
                this.details_.Text = string.Empty;
                this.accountNo_.Text = string.Empty;
                this.totalAmount_.Text = Convert.ToString(0);
                this.pendingAmount_.Text = Convert.ToString(0);
                this.paidAmount_.Text = Convert.ToString(0);
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
          //  paymentType = Convert.ToInt32((CommonEnum.PaymentType)Enum.Parse(typeof(CommonEnum.PaymentType), Convert.ToString(radioButton.Content)));
            paymentType =Convert.ToInt32(Utility.CommonMethods.CommonFunctions.GetValueFromDescription<Utility.CommonEnums.CommonEnum.PaymentType>(Convert.ToString(radioButton.Content)));
        }

        private void lbAutoSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoSupplierName.Visibility = Visibility.Collapsed;
            if (lbAutoSupplierName.SelectedIndex != -1)
            {
                SupplierModel suplier = new SupplierModel();
                suplier = _supliers.FirstOrDefault(x => x.SupplierName == Convert.ToString(lbAutoSupplierName.SelectedItem));
                supplierName_.Text = Convert.ToString(lbAutoSupplierName.SelectedItem);
                _purchaseOrder = controllerPurchase.GetPurchaseBySupplierId(UserModelVm.CompanyId, UserModelVm.BranchId, suplier.Id.Value);
                _directPurchase = controllerPurchase.GetDirectPurchaseBySupplierId(UserModelVm.CompanyId, UserModelVm.BranchId, suplier.Id.Value);
                supplierCode = suplier.Id.Value;
                EnableInvoiceType();
            }
        }

        private void supplier_name_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("supplier_name_KeyUp");
        }

        private void supplier_name_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierModel suplier = _supliers?.FirstOrDefault(x => x.SupplierName.ToLower() == supplierName_.Text.ToLower());
            if (suplier == null)
            {
                // txt_SuplierCode.Text = "";
                supplierName_.Text = "";
            }
            //  else
            // {

            //  }
            lbAutoSupplierName.Visibility = Visibility.Collapsed;
        }
        private void addProductName(SupplierModel suplier)
        {

            lbAutoSupplierName.Items.Add(suplier.SupplierName);
        }
        private void addInvoiceNoByPurchaseOrder(PurchaseOrderModel purchaseorder)
        {
            lbAutoInvoiceNo.Items.Add(purchaseorder.PurchaseId);
        }
        private void addInvoiceNoByDirectPurchase(PurchaseModel directPurchase)
        {

            lbAutoInvoiceNo.Items.Add(directPurchase.PurchaseId);
        }
        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var data = _supliers;
                string query = supplierName_.Text;
                lbAutoSupplierName.Items.Clear();
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
                        lbAutoSupplierName.IsEnabled = true;
                        lbAutoSupplierName.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbAutoSupplierName.IsEnabled = false;
                        lbAutoSupplierName.Visibility = Visibility.Collapsed;
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
                    lbAutoSupplierName.IsEnabled = true;
                    lbAutoSupplierName.Visibility = Visibility.Visible;
                }
                if (!found)
                {
                    lbAutoSupplierName.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
                    lbAutoSupplierName.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ReusableCodeTempByInvoice(string callingMethod)
        {


        }

        private void SelectInvoiceNoByPurchaseOrder(List<PurchaseOrderModel> data, string callingMethod)
        {
            try
            {
                bool found = false;
                // var data = PurchaseType == "hello" ? _purchaseOrder : _directPurchase;
                string query = invoiceNo_.Text;
                lbAutoInvoiceNo.Items.Clear();
                if (query.Length == 0)
                {
                    if (callingMethod == "invoice_arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            //if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                            //{
                            addInvoiceNoByPurchaseOrder(obj);
                            found = true;
                            //}
                        }
                        lbAutoInvoiceNo.IsEnabled = true;
                        lbAutoInvoiceNo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbAutoInvoiceNo.IsEnabled = false;
                        lbAutoInvoiceNo.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (Convert.ToString(obj.PurchaseId).ToLower().StartsWith(query.ToLower()))
                        {
                            addInvoiceNoByPurchaseOrder(obj);
                            found = true;
                        }
                    }
                    lbAutoInvoiceNo.IsEnabled = true;
                    lbAutoInvoiceNo.Visibility = Visibility.Visible;
                }
                if (!found)
                {
                    lbAutoInvoiceNo.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
                    lbAutoInvoiceNo.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void SelectInvoiceNoByDirectPurchase(List<PurchaseModel> data, string callingMethod)
        {
            try
            {
                bool found = false;
                // var data = PurchaseType == "hello" ? _purchaseOrder : _directPurchase;
                string query = invoiceNo_.Text;
                lbAutoInvoiceNo.Items.Clear();
                if (query.Length == 0)
                {
                    if (callingMethod == "invoice_arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            //if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                            //{
                            addInvoiceNoByDirectPurchase(obj);
                            found = true;
                            //}
                        }
                        lbAutoInvoiceNo.IsEnabled = true;
                        lbAutoInvoiceNo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbAutoInvoiceNo.IsEnabled = false;
                        lbAutoInvoiceNo.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (Convert.ToString(obj.PurchaseId).ToLower().StartsWith(query.ToLower()))
                        {
                            addInvoiceNoByDirectPurchase(obj);
                            found = true;
                        }
                    }
                    lbAutoInvoiceNo.IsEnabled = true;
                    lbAutoInvoiceNo.Visibility = Visibility.Visible;
                }
                if (!found)
                {
                    lbAutoInvoiceNo.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
                    lbAutoInvoiceNo.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoSupplierName.Visibility == Visibility.Visible)
            {
                lbAutoSupplierName.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReusableCodeTemp("arrow_Click");
            }
        }


        private void radioButtonInvoiceChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            //  int dd=Convert.ToInt32(Utility.CommonMethods.CommonFunctions.GetValueFromDescription<Utility.CommonEnums.CommonEnum.PurchaseTypes>(Convert.ToString(radioButton.Content)));
            purchaseType = Convert.ToInt32(Utility.CommonMethods.CommonFunctions.GetValueFromDescription<Utility.CommonEnums.CommonEnum.PurchaseTypes>(Convert.ToString(radioButton.Content)));
            // invoiceNo_.Text = string.Empty;
            ClearFields();
            DisableInvoiceType();
        }

        private void lbAutoInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoSupplierName.Visibility = Visibility.Collapsed;
            if (lbAutoInvoiceNo.SelectedIndex != -1)
            {
                if (purchaseType == (int)CommonEnum.PurchaseTypes.PO)
                {
                    PurchaseOrderModel purchaseOrder = new PurchaseOrderModel();
                    purchaseOrder = _purchaseOrder.FirstOrDefault(x => x.PurchaseId == Convert.ToInt32(lbAutoInvoiceNo.SelectedItem));
                    GetTotalAmount(Convert.ToInt32(lbAutoInvoiceNo.SelectedItem));
                }
                else
                {
                    PurchaseModel purchase = new PurchaseModel();
                    purchase = _directPurchase.FirstOrDefault(x => x.PurchaseId == Convert.ToInt32(lbAutoInvoiceNo.SelectedItem));
                    GetTotalAmount(Convert.ToInt32(lbAutoInvoiceNo.SelectedItem));
                }
                GetPaidAmount(Convert.ToInt32(lbAutoInvoiceNo.SelectedItem), purchaseType);
                invoiceNo_.Text = Convert.ToString(lbAutoInvoiceNo.SelectedItem);
                totalAmount_.Text = Convert.ToString(TotalCostPrice);
                paidAmount_.Text = Convert.ToString(PaidAmount);
                pendingAmount_.Text = Convert.ToString(TotalCostPrice - PaidAmount);
            }
        }

        public void GetTotalAmount(int purchaseId)
        {
            if (purchaseId == (int)CommonEnum.PurchaseTypes.DirectPurchase)
                TotalCostPrice = controllerPurchase.GetDirectStocks(purchaseId).Sum(x => x.CostPrice);
            else
                TotalCostPrice = controllerPurchase.GetStocksByPurchaseId(purchaseId).Sum(x => x.CostPrice);
        }
        public void GetPaidAmount(int invoiceNo, int purchaseType)
        {
            PaidAmount = controller.GetPaymentsByPaymentTypeAndInvoiceNo(UserModelVm.CompanyId, UserModelVm.BranchId, invoiceNo, purchaseType).Sum(x => x.Amount);

        }
        private void invoice_no_KeyUp(object sender, KeyEventArgs e)
        {
            if (purchaseType == (int)CommonEnum.PurchaseTypes.PO)
            {
                SelectInvoiceNoByPurchaseOrder(_purchaseOrder, "invoice_no_KeyUp");
            }
            else
            {
                SelectInvoiceNoByDirectPurchase(_directPurchase, "invoice_no_KeyUp");
            }

        }

        private void invoice_no_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isTrue = false;
            if (purchaseType == (int)CommonEnum.PurchaseTypes.PO && !string.IsNullOrWhiteSpace(invoiceNo_.Text))
            {
                PurchaseOrderModel purchaseOrder = _purchaseOrder?.FirstOrDefault(x => x.PurchaseId == Convert.ToInt64(invoiceNo_.Text));
                isTrue = purchaseOrder != null ? true : false;
            }
            else
            {
                PurchaseModel directPurchase = _directPurchase?.FirstOrDefault(x => x.PurchaseId == Convert.ToInt64(invoiceNo_.Text));
                isTrue = directPurchase != null ? true : false;
            }
            if (!isTrue)
            {
                invoiceNo_.Text = "";
            }
            lbAutoInvoiceNo.Visibility = Visibility.Collapsed;
        }

        private void invoice_arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoInvoiceNo.Visibility == Visibility.Visible)
            {
                lbAutoInvoiceNo.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (purchaseType == (int)CommonEnum.PurchaseTypes.PO)
                    SelectInvoiceNoByPurchaseOrder(_purchaseOrder, "invoice_arrow_Click");
                else
                    SelectInvoiceNoByDirectPurchase(_directPurchase, "invoice_arrow_Click");
            }
        }

        private void amount__PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.CommonMethods.CommonFunctions.DecimalValueChecker(sender, e);
            if (Regex.IsMatch(amount_.Text, @"\.\d\d"))
            {
                e.Handled = true;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void txt_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void invoiceNo__PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Utility.CommonMethods.CommonFunctions.IntegerValueChecker(sender, e);
        }

        private void amount__LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }
    }
}

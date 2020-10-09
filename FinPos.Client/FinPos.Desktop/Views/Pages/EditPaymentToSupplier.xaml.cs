using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
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

namespace FinPos.Client.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditPaymentToSupplier.xaml
    /// </summary>
    public partial class EditPaymentToSupplier : Page
    {
        SupplierController controller = new SupplierController();
        public string header = (string)Application.Current.Resources["payment_EditHeader"];// "Edit Paymnet To Supplier";
        public int RowId;
        public int PaymentType;
       public CommonFunction.Validations objValidations = new CommonFunction.Validations();
        public EditPaymentToSupplier(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            ManageControlEdit.invoiceNo_.Text = Convert.ToString(row.InvoiceNo);
            ManageControlEdit.supplierName_.Text = row.SupplierName;
            ManageControlEdit.supplierName_.IsEnabled = false;
            ManageControlEdit.paymentDate_.Text = row.PaymentDate;
            ManageControlEdit.bank_.Text = row.BankName;
            ManageControlEdit.amount_.Text = Convert.ToString(row.Amount);
            ManageControlEdit.details_.Text = row.Description;
            ManageControlEdit.accountNo_.Text = row.AccountNo;
            ManageControlEdit.invoiceNo.Visibility = Visibility.Collapsed;
            ManageControlEdit.purchaseOrder.Visibility = Visibility.Collapsed;
            ManageControlEdit.directPurchase.Visibility = Visibility.Collapsed;
            ManageControlEdit.arrow.IsEnabled = false;
            ManageControlEdit.supplierCode = row.SupplierCode;
            ManageControlEdit.GetTotalAmount(row.InvoiceNo);
            ManageControlEdit.GetPaidAmount(row.InvoiceNo, row.PurchaseType);
            ManageControlEdit.totalAmount_.Text = Convert.ToString(ManageControlEdit.TotalCostPrice);
            ManageControlEdit.paidAmount_.Text = Convert.ToString(ManageControlEdit.PaidAmount);
            ManageControlEdit.pendingAmount_.Text = Convert.ToString(ManageControlEdit.TotalCostPrice - ManageControlEdit.PaidAmount);
          
            RowId = row.PaymentTosupplierId;
            PaymentType = row.PaymentType;
            if (PaymentType == (int)(CommonEnum.PaymentType.Cash))
                ManageControlEdit.cash.IsChecked = true;
           else if (PaymentType == (int)(CommonEnum.PaymentType.Cheque))
                ManageControlEdit.cheque.IsChecked = true;
            else
                ManageControlEdit.transfer.IsChecked = true;
        }
        public void ChangeHeightWidth()
        {
            this.EditPaymentToSupplierPage.Height = HeightWidth.Height - 65;
            this.EditPaymentToSupplierPage.Width = HeightWidth.width;

        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ManageControlEdit.ClearFields();
            PaymentToSupplier form = new PaymentToSupplier();
            NavigationService.Navigate(form);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!objValidations.PaymentValidation(ManageControlEdit, header))
            {
                PaymentToSupplierModel model = new PaymentToSupplierModel(RowId, ManageControlEdit.supplierCode, Convert.ToDecimal(ManageControlEdit.amount_.Text), ManageControlEdit.paymentDate_.Text, ManageControlEdit.details_.Text, Convert.ToInt32(ManageControlEdit.invoiceNo_.Text), ManageControlEdit.accountNo_.Text, Convert.ToInt32(UserModelVm.UserId), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), null, string.Empty, PaymentType, ManageControlEdit.bank_.Text, Convert.ToInt32(UserModelVm.CompanyId), UserModelVm.BranchId, string.Empty, string.Empty, ManageControlEdit.purchaseType);
                controller.SaveUpdatePayment(model);
                Common.Notification((string)Application.Current.Resources["Payment_Updated_Success"], header, false);
                //  ClearFields();
                PaymentToSupplier _wastage = new PaymentToSupplier();
                NavigationService.Navigate(_wastage);
            }
        }
    }
}

using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
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
    /// Interaction logic for AddPaymentToSupplier.xaml
    /// </summary>
    public partial class AddPaymentToSupplier : Page
    {
        #region Properties
        public SupplierController controller = new SupplierController();
        public string header = (string)Application.Current.Resources["payment_AddHeader"];
        public CommonFunction.Validations objValidations =new CommonFunction.Validations();
        #endregion
        #region Constructor
        public AddPaymentToSupplier()
        {
            InitializeComponent();
            ChangeHeightWidth();
        }
        #endregion
        #region Common Method
        public void ChangeHeightWidth()
        {
            this.AddPaymentTosupplierPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddPaymentTosupplierPage.Width = HeightWidth.width;

        }
        public void NavigateToBackPage()
        {
            PaymentToSupplier _wastage = new PaymentToSupplier();
            NavigationService.Navigate(_wastage);
        }
        #endregion

        #region CRUD Opertion
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!objValidations.PaymentValidation(ManageControl, header))
            {
                PaymentToSupplierModel model = new PaymentToSupplierModel(0, ManageControl.supplierCode, Convert.ToDecimal(ManageControl.amount_.Text), ManageControl.paymentDate_.Text, ManageControl.details_.Text, Convert.ToInt32(ManageControl.invoiceNo_.Text), ManageControl.accountNo_.Text, Convert.ToInt32(UserModelVm.UserId), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), null, string.Empty, ManageControl.paymentType.Value, ManageControl.bank_.Text, Convert.ToInt32(UserModelVm.CompanyId), UserModelVm.BranchId, string.Empty, string.Empty, ManageControl.purchaseType);
                controller.SaveUpdatePayment(model);
                Common.Notification((string)Application.Current.Resources["Payment_Savet_Success"], header, false);
                //  ClearFields();
                NavigateToBackPage();
            }
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ManageControl.ClearFields();
            NavigateToBackPage();
        }
        #endregion
    }
}

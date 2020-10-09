using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using FinPos.Utility.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PaymentToSupplier.xaml
    /// </summary>
    /// 
    public partial class PaymentToSupplier : Page
    {
        SupplierController controller = new SupplierController();
        private IList<PaymentToSupplierModel> _payments;
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        BrushConverter color = new BrushConverter();
        private string header = "Payment";
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public PaymentToSupplier()
        {
            InitializeComponent();
            ChangeHeightWidth();
            DisableIcons();
            ResponseVm response = controller.GetPaymentsByCompanyIdAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
            List<SupplierModel> suppliers = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId, UserModelVm.BranchId).ToList();
            if (response.FaultData == null)
            {
                _payments = response.Response.Cast<PaymentToSupplierModel>().ToList();
                lvPayments.ItemsSource = _payments;
                SupplierModel obj = new SupplierModel(0, (string)Application.Current.Resources["Combo_Select"]);
                suppliers.Insert(0, obj);
                this.cmbSupplier.ItemsSource = suppliers;
                this.cmbSupplier.DisplayMemberPath = "SupplierName";
                this.cmbSupplier.SelectedValue = "Id";
                cmbSupplier.SelectedIndex = 0;
                DisableIcons();
            }
            else
            {
                Common.ErrorMessage(response.FaultData.Detail.ErrorDetails, header);
            }
        }
        public void ChangeHeightWidth()
        {
            this.PaymentToSupplierPage.Height = HeightWidth.Height - 65;
            this.PaymentToSupplierPage.Width = HeightWidth.width;

        }
        private void payment_search_GotFocus(object sender, RoutedEventArgs e)
        {
            payment_search.Text = string.Empty;
        }

        private void lvPayments_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);

            if (item != null || item.IsSelected)
            {
                EnableIcons();
            }
        }

        private void payment_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(payment_search.Text))
            {
                payment_search.Text = "Search";
            }
        }

        private void EnableIcons()
        {
            edit_Payment.IsEnabled = true;
            edit_Payment.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
            btn_clear.IsEnabled = true;
            btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
            btn_Delete.IsEnabled = true;
            btn_Delete.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
        }
        private void DisableIcons()
        {
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            edit_Payment.IsEnabled = false;
            edit_Payment.Background = Brushes.Gray;
        }
        private void payment_search_KeyUp(object sender, KeyEventArgs e)
        {
            DisableIcons();
            var text = payment_search.Text.ToLower();

            lvPayments.ItemsSource = _payments.Where(x => Convert.ToString(x.PaymentTosupplierId).Contains(text) || x.SupplierName.ToLower().Contains(text) || x.PaymentTypeName.ToLower().Contains(text) || Convert.ToString(x.SupplierCode).Contains(text) || Convert.ToString(x.InvoiceNo).Contains(text) || Convert.ToString(x.Amount).Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvPayments.ItemsSource).Refresh();
        }

        private void PaymentGetByDate(string fromDate, string toDate)
        {
            controller.GetPaymentByDateFilter(UserModelVm.CompanyId, UserModelVm.BranchId, fromDate, toDate);
        }

        private void lvPaymentsColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvPayments.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvPayments.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            payment_search.Text = string.Empty;
            lvPayments.ItemsSource = _payments.Where(x => Convert.ToString(x.PaymentTosupplierId).Contains(payment_search.Text) || x.SupplierName.ToLower().Contains(payment_search.Text) || x.PaymentTypeName.ToLower().Contains(payment_search.Text) || Convert.ToString(x.SupplierCode).Contains(payment_search.Text) || Convert.ToString(x.InvoiceNo).Contains(payment_search.Text) || Convert.ToString(x.Amount).Contains(payment_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvPayments.ItemsSource).Refresh();
            SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(payment_search.Text))
            {
                payment_search.Text = "Search";
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lvPayments.SelectedItem = null;
            DisableIcons();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPayments.SelectedItem;
            ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["payment_delete_alert"], header, true);
            form.ShowDialog();
            if (Common._isChecked)
            {
                controller.DeletePayment(row.PaymentTosupplierId);
                // if (result.FaultData == null)
                // {
                ResponseVm response = controller.GetPaymentsByCompanyIdAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);//.ToList();
                if (response.FaultData == null)
                {
                    _payments = response.Response.Cast<PaymentToSupplierModel>().ToList();
                    lvPayments.ItemsSource = _payments;
                    Common.Notification((string)Application.Current.Resources["deletePayment_success_alert"], header, false);
                    DisableIcons();
                }
                else
                {
                    Common.ErrorMessage(response.FaultData.Detail.ErrorDetails, header);
                }
                //  }
                // else
                //  {
                //    Common.ErrorMessage((string)Application.Current.Resources["product_exist_exeption"], header);
                //  }
            }
        }

        private void btn_addPayment_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentToSupplier page = new AddPaymentToSupplier();
            NavigationService.Navigate(page);
        }

        private void edit_Payment_Click(object sender, RoutedEventArgs e)
        {
            var row = lvPayments.SelectedItem;
            EditPaymentToSupplier editProduct = new EditPaymentToSupplier(row);
            // ProductsHistory editProduct = new ProductsHistory();
            NavigationService.Navigate(editProduct);
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSupplier.SelectedIndex > 0)
            {
                var val = (SupplierModel)cmbSupplier.SelectedValue;
                _payments = controller.GetPaymentBySupplierCode(UserModelVm.CompanyId, UserModelVm.BranchId, val.Id.Value);
                lvPayments.ItemsSource = _payments;
            }
            else
            {
                ResponseVm response = controller.GetPaymentsByCompanyIdAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
                // _payments = controller.GetPaymentsByCompanyIdAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
                _payments = response.Response.Cast<PaymentToSupplierModel>().ToList();
                lvPayments.ItemsSource = _payments;
            }
        }

        private void lvPayments_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvPayments_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}

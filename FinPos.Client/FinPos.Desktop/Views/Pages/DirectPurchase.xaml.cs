using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using NLog;
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

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DirectPurchase.xaml
    /// </summary>
    public partial class DirectPurchase : Page
    {
        #region Properties
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        PurchaseController controller = new PurchaseController();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<PurchaseModel> _purchase;
        BrushConverter color = new BrushConverter();
        ResourceDictionary myResourceDictionary;
        public string header = "Purchase";
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion

        #region Constructor
        public DirectPurchase()
        {
            InitializeComponent();
            ChangeHeightWidth();
            GetResourceDictonary();
            BindDirectPurchases();
            EnableDisableButtons(false);
        }
        #endregion

        #region Common Methods
        private void ButtonDisable()
        {
            EnableDisableButtons(false);
            btnPrint.IsEnabled = false;
            btnPrint.Background = Brushes.Gray;
            lvPurchase.SelectedItem = null;
        }
        private void BindDirectPurchases()
        {
            lvPurchase.ItemsSource = _purchase = controller.GetDirectPurchaseByCompanyAndBranchId().Where(x => CommonFunctions.ParseDateToFinclave(x.CreatedDate) >= CommonFunctions.ParseDateToFinclave(Settings.FinalYearStartDate) && CommonFunctions.ParseDateToFinclave(x.CreatedDate) <= CommonFunctions.ParseDateToFinclave(Settings.FinalYearEndDate)).OrderBy(x => x.PurchaseId).ToList();
        }
        private void GetResourceDictonary()
        {
            myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/ResourceFiles/En.xaml",
                        UriKind.RelativeOrAbsolute);
        }
        private void EnableDisableButtons(bool Enable)
        {
            if (!Enable)
            {
                btn_editPurchase.Background = Brushes.Gray;
                btn_clear.Background = Brushes.Gray;
                btn_Delete.Background = Brushes.Gray;
            }
            else
            {
                btn_editPurchase.Background = (Brush)color.ConvertFrom("#0091EA");
                btn_clear.Background = (Brush)color.ConvertFrom("#eb5151");
                btn_Delete.Background = (Brush)color.ConvertFrom("#eb5151");
            }
            btn_Delete.IsEnabled = Enable;
            btn_clear.IsEnabled = Enable;
            btn_editPurchase.IsEnabled = Enable;
        }
        public void ChangeHeightWidth()
        {
            this.DirectPurchasePage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.DirectPurchasePage.Width = HeightWidth.width;
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txtPurchase_search.Text))
            {
                txtPurchase_search.Text = "Search";
            }
        }
        #endregion

        #region Events
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvPurchase.Items.SortDescriptions.Clear();
            }
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvPurchase.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void addPurchase_Click(object sender, RoutedEventArgs e)
        {
            AddDirectPurchase form = new AddDirectPurchase();
            NavigationService.Navigate(form);
        }

        private void lvPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            dynamic row = e.Source;
            dynamic purchase = row.DataContext;
            if (item != null || item.IsSelected)
            {
                EnableDisableButtons(true);
                btnPrint.Visibility = Visibility.Visible;
                btnPrint.Background = (Brush)color.ConvertFrom("#0091EA");
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ButtonDisable();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;
            Common.ErrorMessage((string)myResourceDictionary["purchase_confirmationmsg"], header);
            if (Common._isChecked)
            {
                controller.DeleteDirectPurchase(row.PurchaseId);
                BindDirectPurchases();
                Common.Notification((string)myResourceDictionary["purchase_deletedsuccessmsg"], header, false);
                ButtonDisable();
            }
        }
        private void btn_editPurchase_Click(object sender, RoutedEventArgs e)
        {
            var row = lvPurchase.SelectedItem;
            EditDirectPurchase editProduct = new EditDirectPurchase(row);
            NavigationService.Navigate(editProduct);
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;
            List<StockModel> stocks = controller.GetStocksByPurchaseId(row.PurchaseId);
            var purchaseReturns = stocks?.GroupBy(z => new { z.ProductCode, z.BatchNo }).Select(x => new PurchaseReturnModel(null, x.FirstOrDefault().PurchaseOrderId.Value, x.FirstOrDefault().ProductCode, x.FirstOrDefault().BatchNo, x.Sum(y => y.Quantity), UserModelVm.UserId, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()), string.Empty)).ToList();
            if (controller.SaveUpdatePurchaseReturns(purchaseReturns))
            {
                controller.UpdateStatus(row, (int)CommonEnum.PurchaseStatus.FullyReturned);
                BindDirectPurchases();
                Common.Notification((string)myResourceDictionary["purchase_retunedmsg"], header, false);
                ButtonDisable();
            }
        }
        private void btnApproval_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;
            var result = controller.UpdateStatus(row, (int)CommonEnum.PurchaseStatus.Approved);
            BindDirectPurchases();
            Common.Notification((string)myResourceDictionary["purchase_approvedmsg"], header, false);
            ButtonDisable();
        }
        private void purchase_search_KeyUp(object sender, KeyEventArgs e)
        {
            btnPrint.Visibility = Visibility.Collapsed;
            EnableDisableButtons(false);
            var text = txtPurchase_search.Text.ToLower();
            lvPurchase.ItemsSource = _purchase.Where(x => Convert.ToString(x.PurchaseId).Contains(text) || x.SuplierName.ToLower().Contains(text) || Convert.ToString(x.SuplierCode).ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvPurchase.ItemsSource).Refresh();
        }
        private void txtPurchase_search_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPurchase_search.Text = string.Empty;
        }
        private void txtPurchase_search_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTextOnSearch();
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            txtPurchase_search.Text = string.Empty;
            lvPurchase.ItemsSource = _purchase.Where(x => Convert.ToString(x.PurchaseId).Contains(string.Empty) || x.SuplierName.ToLower().Contains(string.Empty) || Convert.ToString(x.SuplierCode).ToLower().Contains(string.Empty)).ToList();
            CollectionViewSource.GetDefaultView(lvPurchase.ItemsSource).Refresh();
            SetTextOnSearch();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var row = lvPurchase.SelectedItem;
            invoice form = new invoice(row, "DirectPurchase", "");
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion
    }
}


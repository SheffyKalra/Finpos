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
using System.Drawing.Printing;
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
    /// Interaction logic for Purchase.xaml
    /// </summary>
    public partial class Purchase : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        PurchaseController controller = new PurchaseController();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<PurchaseOrderModel> _purchase;
        BrushConverter color = new BrushConverter();
        ResourceDictionary myResourceDictionary;
        public string header = (string)Application.Current.Resources["purchase_Title"];
        public string selectedPO = "";
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Purchase()
        {
            InitializeComponent();
            ChangeHeightWidth();
            myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source =
                new Uri("/ResourceFiles/En.xaml",
                        UriKind.RelativeOrAbsolute);
            var lstPurchase = controller.GetPurchase().Where(x => CommonFunctions.ParseDateToFinclave(x.CreatedDate) >= CommonFunctions.ParseDateToFinclave(Settings.FinalYearStartDate) && CommonFunctions.ParseDateToFinclave(x.CreatedDate) <= CommonFunctions.ParseDateToFinclave(Settings.FinalYearEndDate)).ToList();
            if (lstPurchase != null)
            {
                _purchase = lstPurchase;
                lvPurchase.ItemsSource = _purchase;
            }
            else
            {
                // ConfirmationPopup form = new ConfirmationPopup(lstPurchase.FaultData.Detail.ErrorDetails, "Fault", false);
                // form.ShowDialog();
            }
            btn_editPurchase.IsEnabled = false;
            btn_editPurchase.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btnApproval.IsEnabled = false;
            btnApproval.Background = Brushes.Gray;
            btnReturn.IsEnabled = false;
            btnReturn.Background = Brushes.Gray;
            btnPrint.IsEnabled = false;
            btnPrint.Background = Brushes.Gray;
        }
        public void ChangeHeightWidth()
        {
            this.PoPage.Height = HeightWidth.Height - 65;
            this.PoPage.Width = HeightWidth.width;

        }
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
            AddPurchase form = new AddPurchase();
            NavigationService.Navigate(form);
        }

        private void lvPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            dynamic row = e.Source;
            dynamic purchase = row.DataContext;
            if (item != null || item.IsSelected)
            {
                selectedPO = purchase.PurchaseId.ToString();
                btn_editPurchase.IsEnabled = true;
                btn_editPurchase.Background = (Brush)color.ConvertFrom("#0091EA");
                btn_clear.IsEnabled = true;
                btn_clear.Background = (Brush)color.ConvertFrom("#eb5151");
                btn_Delete.IsEnabled = true;
                btn_Delete.Background = (Brush)color.ConvertFrom("#eb5151");
                if (purchase.StatusName == ((int)(CommonEnum.PurchaseStatus.WaitingForApproval)).ToString())
                {
                    btnReturn.IsEnabled = false;
                    btnReturn.Background = Brushes.Gray; ;
                    btnApproval.IsEnabled = true;
                    btnApproval.Background = (Brush)color.ConvertFrom("#0091EA");
                    btnPrint.IsEnabled = false;
                    btnPrint.Background = Brushes.Gray;
                }
                else if (purchase.StatusName == ((int)(CommonEnum.PurchaseStatus.Approved)).ToString())
                {
                    btnPrint.IsEnabled = true;
                    btnPrint.Background = (Brush)color.ConvertFrom("#0091EA");
                    btnReturn.IsEnabled = true;
                    btnReturn.Background = (Brush)color.ConvertFrom("#0091EA");
                    btnApproval.IsEnabled = false;
                    btnApproval.Background = Brushes.Gray; ;
                }
                else
                {
                    btnPrint.IsEnabled = false;
                    btnPrint.Background = Brushes.Gray;//(Brush)color.ConvertFrom("#0091EA");
                    btnReturn.IsEnabled = false;
                    btnReturn.Background = Brushes.Gray; ;
                    btnApproval.IsEnabled = false;
                    btnApproval.Background = Brushes.Gray;
                }
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ButtonDisable();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;
            ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["purchase_confirmationmsg"], header, true);
            form.ShowDialog();
            if (Common._isChecked)
            {
                var result = controller.DeletePurchase(row.PurchaseId);
                //if (result == true)
                //{
                List<PurchaseOrderModel> lstPurchase = controller.GetPurchase().ToList();
                // if (lstPurchase.Any())
                //  {
                _purchase = lstPurchase;
                lvPurchase.ItemsSource = _purchase;
                // ConfirmationPopup form1 = new ConfirmationPopup((string)myResourceDictionary["purchase_deletedsuccessmsg"], header, false);
                // form1.ShowDialog();
                Common.Notification((string)myResourceDictionary["purchaseOrder_deletedsuccessmsg"], header, false);
                ButtonDisable();
                // }
                // else
                // {
                //  ConfirmationPopup form2 = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
                //    form2.ShowDialog();
                // }
            }
        }

        private void ButtonDisable()
        {
            btn_editPurchase.IsEnabled = false;
            btn_editPurchase.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btnReturn.IsEnabled = false;
            btnReturn.Background = Brushes.Gray;
            btnApproval.IsEnabled = false;
            btnApproval.Background = Brushes.Gray;
            btnPrint.IsEnabled = false;
            btnPrint.Background = Brushes.Gray;
            lvPurchase.SelectedItem = null;
        }

        private void btn_editPurchase_Click(object sender, RoutedEventArgs e)
        {
            var row = lvPurchase.SelectedItem;
            EditPurchase editProduct = new EditPurchase(row);
            NavigationService.Navigate(editProduct);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;

            List<StockModel> stocks = controller.GetStocksByPurchaseId(row.PurchaseId);
            var purchaseReturns = stocks?.GroupBy(z => new { z.ProductCode, z.BatchNo }).Select(x => new PurchaseReturnModel(null, x.FirstOrDefault().PurchaseOrderId.Value, x.FirstOrDefault().ProductCode, x.FirstOrDefault().BatchNo, x.Sum(y => y.Quantity), UserModelVm.UserId, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()), string.Empty)).ToList();
            bool isSaved = controller.SaveUpdatePurchaseReturns(purchaseReturns);
            if (isSaved)
            {
                var result = controller.UpdateStatus(row, (int)CommonEnum.PurchaseStatus.FullyReturned);

                //if (result == true)
                //{
                List<PurchaseOrderModel> lstPurchase = controller.GetPurchase().ToList();
                if (lstPurchase.Any())
                {
                    _purchase = lstPurchase;
                    lvPurchase.ItemsSource = _purchase;
                    //   ConfirmationPopup form1 = new ConfirmationPopup((string)myResourceDictionary["purchase_retunedmsg"], "Purchase", false);
                    //  form1.ShowDialog();
                    Common.Notification((string)myResourceDictionary["purchase_retunedmsg"], header, false);
                    ButtonDisable();
                }
                else
                {
                    //  ConfirmationPopup form2 = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
                    //    form2.ShowDialog();
                }
            }
        }

        private void btnApproval_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvPurchase.SelectedItem;
            var result = controller.UpdateStatus(row, (int)CommonEnum.PurchaseStatus.Approved);
            //if (result == true)
            //{
            List<PurchaseOrderModel> lstPurchase = controller.GetPurchase().ToList();
            if (lstPurchase.Any())
            {
                _purchase = lstPurchase;
                lvPurchase.ItemsSource = _purchase;
                // ConfirmationPopup form1 = new ConfirmationPopup((string)myResourceDictionary["purchase_approvedmsg"], "Purchase", false);
                // form1.ShowDialog();
                Common.Notification((string)myResourceDictionary["purchaseOrder_approvedmsg"], header, false);
                ButtonDisable();
            }
            else
            {
                //  ConfirmationPopup form2 = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
                //    form2.ShowDialog();
            }
        }

        private void purchase_search_KeyUp(object sender, KeyEventArgs e)
        {
            btnApproval.IsEnabled = false;
            btnApproval.Background = Brushes.Gray;
            btnReturn.IsEnabled = false;
            btnReturn.Background = Brushes.Gray;
            btnPrint.IsEnabled = false;
            btnPrint.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btn_editPurchase.IsEnabled = false;
            btn_editPurchase.Background = Brushes.Gray;
            var text = txtPurchase_search.Text.ToLower();

            lvPurchase.ItemsSource = _purchase.Where(x => Convert.ToString(x.PurchaseId).Contains(text) || x.SuplierName.ToLower().Contains(text) || Convert.ToString(x.SuplierCode).ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvPurchase.ItemsSource).Refresh();
        }

        private void txtPurchase_search_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPurchase_search.Text = string.Empty;
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txtPurchase_search.Text))
            {
                txtPurchase_search.Text = "Search";
            }
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
            invoice form = new invoice(row, header, (string)Application.Current.Resources["purchase_printHeader"]);

        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }



        //private void btn_showPop_Click(object sender, RoutedEventArgs e)
        //{
        //    printPopUp.IsOpen = true;
        //    cmbPO.ItemsSource = _purchase;
        //    cmbPO.DisplayMemberPath = "PurchaseId";
        //    cmbPO.SelectedValue = "PurchaseId";
        //    cmbPO.Text = selectedPO;
        //    selectedPO = "";
        //}

        //private void btn_close_Click(object sender, RoutedEventArgs e)
        //{
        //    printPopUp.IsOpen = false;
        //    this.IsEnabled = true;
        //}

        ////private void btnPrinterPopupOk_Click(object sender, RoutedEventArgs e)
        ////{
        ////    List<PurchaseOrderModel> row = _purchase.Where(item => item.PurchaseId == Convert.ToInt32(cmbPO.Text)).ToList();
        ////    if (row.Count > 0)
        ////    {
        ////        PurchaseOrderModel obj = row[0];
        //        invoice form = new invoice(obj, "Purchase");
        ////    }
        ////}
    }

}


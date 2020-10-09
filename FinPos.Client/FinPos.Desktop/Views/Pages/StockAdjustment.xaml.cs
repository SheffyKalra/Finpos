using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Model;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interactivity;
using FinPos.Client.Controls;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StockManagement.xaml
    /// </summary>
    public partial class StockAdjustment : Page
    {
        private ProductController controller = new ProductController();
        private IList<ProductModel> _products;
        private StockAdjustmentController StockAdjustmentController = new StockAdjustmentController();
        private ObservableCollection<StockAdjustmentModel> StockAdjustments;
        BrushConverter color = new BrushConverter();
        public int rowIndex = 0;
        private string msg = string.Empty;
        private int _noOfErrorsOnScreen = 0;
        private string header = (string)Application.Current.Resources["stock_adjustment"];
        public StockAdjustmentModel _selectedStock;
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public StockAdjustment()
        {

            InitializeComponent();
            ChangeHeightWidth();
            //productPopUp.MouseMove += new MouseEventHandler(pop_MouseMove);
            btn_remove.Background = Brushes.Gray;
            ResponseVm responce = controller.GetProductsByCompanyAndBranch();
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                if (_products != null && _products.Count > 0)
                {
                    dgProducts.ItemsSource = _products;
                    dgProducts.Visibility = Visibility.Visible;
                }
                else
                {
                    dgProducts.Visibility = Visibility.Collapsed;
                    brd_exp.Visibility = Visibility.Visible;
                }
            }

            if (StockAdjustments == null)
            {
                additems();
            }
            lvStockAdjustment.ItemsSource = StockAdjustments;//.Where(x=>Convert.ToDateTime(x.CreatedDate)>=Convert.ToDateTime(Settings.FinalYearStartDate) && Convert.ToDateTime(x.CreatedDate)<=Convert.ToDateTime(Settings.FinalYearEndDate));
        }
        public void ChangeHeightWidth()
        {
            this.StockManagementPage.Height = HeightWidth.Height - 65;
            this.StockManagementPage.Width = HeightWidth.width;

        }

        //void pop_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        productPopUp.PlacementRectangle = new Rect(new Point(e.GetPosition(this).X,
        //            e.GetPosition(this).Y), new Point(1200, 700));

        //    }
        //}
        public void additems()
        {
            StockAdjustments = new ObservableCollection<StockAdjustmentModel>();
            StockAdjustments.Add(new StockAdjustmentModel() { ProductName = "" });
        }
        private void btn_addRow_Click(object sender, RoutedEventArgs e)
        {
            var reasonSet = (StockAdjustmentModel)Application.Current.Resources["stockAdjustmentModel"];
            reasonSet.Reason = "";
            reasonSet.Quantity = 0;

            StockAdjustments.Add(new StockAdjustmentModel() { ProductName = "" });
            _noOfErrorsOnScreen++;
        }
        private void AddCustomer_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            List<StockAdjustmentModel> Stocks = lvStockAdjustment.Items.Cast<StockAdjustmentModel>().Select(x => x).ToList();
            bool IsTrue = Stocks.Where(x => x.ProductCode == 0 || x.Quantity == 0).Any();
            if (IsTrue)
            {
                Common.ErrorMessage((string)Application.Current.Resources["stock_adjustmentRequiredFields"], header);
            }
            else
            {
                ObservableCollection<QuantityViewModel> listRemainings = new ObservableCollection<QuantityViewModel>();
                listRemainings.Clear();
                int reasonCount = 0;
                Stocks.ForEach(x =>
                {
                    if (x.Quantity < 0 && string.IsNullOrEmpty(x.Reason))
                        reasonCount++;
                });
                dgProQuant.ItemsSource = listRemainings;
                foreach (StockAdjustmentModel stock in Stocks)
                {
                    var currentStockByBatch = StockAdjustmentController.GetItemCurrentStockByBatchNo(stock.BatchNo, stock.productCode);
                    var observeDeduction = currentStockByBatch + stock.Quantity;
                    var takePoint = stock.Quantity.ToString().Substring(0, 1);
                    if (currentStockByBatch < stock.Quantity && currentStockByBatch != 0 && takePoint == "-" || observeDeduction < 0 && takePoint == "-")
                    {
                        listRemainings.Add(new QuantityViewModel
                        {
                            ProductName = stock.ProductName,
                            AvailQuantity = currentStockByBatch
                        });
                    }
                    stock.CreatedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    stock.CompanyCode = UserModelVm.CompanyId;
                    stock.BranchCode = UserModelVm.BranchId;
                    stock.CreatedBy = UserModelVm.UserId;
                }
                if (listRemainings.Count > 0)
                {
                    dgProQuant.ItemsSource = listRemainings;
                    PopupQuantityAlert.IsOpen = true;
                    quantAlert.Text = (string)Application.Current.Resources["quantity_exeed_exeption"];
                    return;
                }
                else if (reasonCount > 0)
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["reason_forneagative_exeption"], header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification((string)Application.Current.Resources["reason_forneagative_exeption"], header, false);
                    return;
                }
                StockAdjustmentController.SaveStockAdjustment(Stocks);
                additems();
                lvStockAdjustment.ItemsSource = StockAdjustments;
                Common.Notification((string)Application.Current.Resources["stock_adjustmentSuccessMsg"], header, false);
            }
        }

        public void stock_AddItem(ProductModel itemToAdd)
        {
            if (StockAdjustments.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_already"], "OpeningStock", true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    AddItemSource(itemToAdd);
                }
            }
            else
            {
                AddItemSource(itemToAdd);
            }

        }
        private void AddItemSource(ProductModel openingModel)
        {
            StockAdjustmentModel _stockAdjustment = new StockAdjustmentModel();
            if (openingModel != null)
            {
                _stockAdjustment.ProductName = openingModel.ItemName;
                _stockAdjustment.productCode = Convert.ToInt64(openingModel.Id);
                _stockAdjustment.CurrentStock = StockAdjustmentController.GetCurrentStockByProductCode(Convert.ToInt64(openingModel.Id));
                StockAdjustments[rowIndex] = _stockAdjustment;
                lvStockAdjustment.ItemsSource = StockAdjustments;
            }
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
        }
        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            if (productItem != null)
            {
                productPopUp.IsOpen = false;
                this.IsEnabled = true;
                btn_remove.IsEnabled = false;
                btn_remove.Background = Brushes.Gray;
                bool stockExists = StockAdjustmentController.CheckStockByProductCode(Convert.ToInt64(productItem.Id));
                if (stockExists)
                {
                    stock_AddItem(productItem);
                }
                else
                {

                    ResourceDictionary myResourceDictionary = new ResourceDictionary();
                    myResourceDictionary.Source =
                    new Uri("/ResourceFiles/En.xaml",
                            UriKind.RelativeOrAbsolute);
                    // string errorMessage = (string)myResourceDictionary["error_message_false_product_selection"];
                    // msg = errorMessage;
                    ConfirmationPopup form = new ConfirmationPopup((string)myResourceDictionary["error_message_false_product_selection"], header, false);
                    form.ShowDialog();
                    //   Common.ErrorNotification((string)myResourceDictionary["error_message_false_product_selection"], header, false);
                }
            }
        }
        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            System.Windows.Controls.ListViewItem lvi = CommonFunction.Common.GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lvStockAdjustment.SelectedIndex =
                    lvStockAdjustment.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lvStockAdjustment.SelectedIndex;
                _selectedStock = (StockAdjustmentModel)lvStockAdjustment.SelectedItem;

            }
        }
        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgProducts.ItemsSource);
            if (cv == null)
            {
                return;
            }
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
        private void lvStockAdjustment_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = (sender as System.Windows.Controls.ListViewItem);
            dynamic row = e.Source;
            //dynamic _openingStock = row.DataContext;///found unused code
            if (item != null || item.IsSelected)
            {
                btn_remove.IsEnabled = true;
                btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PopupQuantityAlertClose_Click(object sender, RoutedEventArgs e)
        {
            PopupQuantityAlert.IsOpen = false;
        }

        private void txtEmail_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (productPopUp.IsOpen == true)
            {
                return;
            }
            StockAdjustments.Remove((StockAdjustmentModel)lvStockAdjustment.SelectedItem);
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            _noOfErrorsOnScreen--;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);

        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
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
            dgProducts.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
            //SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }

        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            dgProducts.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgProducts.ItemsSource).Refresh();
        }

        private void lvStockAdjustment_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as System.Windows.Controls.ListView);
        }

        private void lvStockAdjustment_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as System.Windows.Controls.ListView);
        }
        #endregion
        //private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        //{
        //    Thumb t = (Thumb)sender;
        //    t.Cursor = Cursors.Hand;
        //}

        //private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    double yadjust = this.productPopUp.Height + e.VerticalChange;
        //    double xadjust = this.productPopUp.Width + e.HorizontalChange;
        //    if ((xadjust >= 0) && (yadjust >= 0))
        //    {
        //        this.productPopUp.Width = xadjust;
        //        this.productPopUp.Height = yadjust;
        //    }
        //}

        //private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        //{
        //    Thumb t = (Thumb)sender;
        //    t.Cursor = null;
        //}
    }

}

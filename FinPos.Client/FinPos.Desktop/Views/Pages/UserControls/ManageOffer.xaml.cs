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
    /// Interaction logic for ManageOffer.xaml
    /// </summary>
    public partial class ManageOffer : UserControl
    {
        public string CallingPage { get { return this.page_Name.Content.ToString(); } set { } }
        public string OfferType { get { { return Convert.ToString(this.cmbOfferType.SelectedIndex); } } set { } }
        public string FromDate { get { return this.DTPFromDate.SelectedDate.Value.Date.ToShortDateString(); } set { } }
        public string ToDate { get { return this.DTPToDate.SelectedDate.Value.Date.ToShortDateString(); } set { } }
        public int MinimumValue { get { return Convert.ToInt32(this.txtMinimumValue.Text); } set { } }
        //public string DiscountType { get { { return Convert.ToString(this.cmbDiscountType.SelectedIndex); } } set { } }
        public int OfferId { get; set; }
        //private ObservableCollection<OfferdetailModel> offerDetails;
        private IList<OfferdetailModel> offerDetails;
        private ProductController productController = new ProductController();
        private CouponManagementController couponManagmentCoutroller = new CouponManagementController();
        BrushConverter color = new BrushConverter();
        CommonFunction.Validations objValidate = new CommonFunction.Validations();
        public int rowIndex = 0;
        public OfferdetailModel _offerDetails;
        public decimal Discount
        {
            get
            {
                return
            Convert.ToDecimal(txtDiscount.Text == null ? "0" : txtDiscount.Text == "" ? "0" : txtDiscount.Text);
            }
            set { this.txtDiscount.Text = Convert.ToString(Discount); }
        }
        public object ParentPage { get; set; }
        public string OfferName { get { return this.txtName.Text; } set { } }
        public int TabControlOfferTypeSelectdIndex { get; set; }
        private IList<ProductModel> _products;
        public ManageOffer()
        {
            InitializeComponent();
            BindOfferType();
            SetTextBoxsToEmpty();
            SetCalender();
            BindProduct();
            ChangeHeightWidth();
        }
        public void ChangeHeightWidth()
        {
            this.ManageOfferUC.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.ManageOfferUC.Width = HeightWidth.width;
        }
        private void BindProduct()
        {
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
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
        }

        private void SetCalender()
        {
            CalendarDateRange cdr = new CalendarDateRange(CommonFunctions.ParseDateToFinclave(DateTime.MinValue.ToShortDateString()), CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()).AddDays(-1));
            DTPFromDate.BlackoutDates.Add(cdr);
            DTPToDate.BlackoutDates.Add(cdr);
        }
        private void BindOfferType()
        {
            var discountTypes = Enum.GetValues(typeof(FinPos.Utility.CommonEnums.CommonEnum.OfferType))
   .Cast<Enum>()
   .Select(value => new
   {
       (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description
   })
   .ToList();
            var select = new { Description = (string)Application.Current.Resources["Combo_Select"] };
            discountTypes.Insert(0, select);
            cmbOfferType.DisplayMemberPath = "Description";
            cmbOfferType.SelectedValue = "Value";
            cmbOfferType.ItemsSource = discountTypes.Where(item => item.Description != Utility.CommonMethods.CommonFunctions.GetDescriptionFromEnumValue(
                        (Utility.CommonEnums.CommonEnum.OfferType)Enum.Parse(typeof(Utility.CommonEnums.CommonEnum.OfferType), Convert.ToString((int)CommonEnum.OfferType.ItemSpecific))));
            cmbOfferType.SelectedIndex = 0;

        }
        private void SetTextBoxsToEmpty()
        {
            txtName.Text = string.Empty;
            txtMinimumValue.Text = string.Empty;
            txtDiscount.Text = string.Empty;
        }
        public void SetItemSpecificValues(OfferModel offerModel)
        {
            if (offerModel.Id > 0)
            {
                ItemSpecific.Visibility = Visibility.Visible;
                default_Tab.Visibility = Visibility.Collapsed;
                offerDetails = couponManagmentCoutroller.GetItemSpecificOfferDetails(offerModel.Id).ToList<OfferdetailModel>();
                lvProducts.ItemsSource = offerDetails;
            }
            this.tabControl.SelectedIndex = (int)CommonEnum.ManageOfferTabControls.ItemSpecific;
            this.page_Name.Content = offerModel.CallingPage;
            this.txtOfferName.Text = offerModel.Name;
            btn_addRow.IsEnabled = false;

        }
        public void SetText(OfferModel couponModel)
        {
            if (couponModel.Id > 0)
            {
                ItemSpecific.Visibility = Visibility.Collapsed;
            }

            this.page_Name.Content = couponModel.CallingPage;
            this.CallingPage = couponModel.CallingPage;
            this.cmbOfferType.Text = couponModel.OfferType;
            this.txtName.Text = couponModel.Name;
            this.txtMinimumValue.Text = Convert.ToString(couponModel.MinimumValue);
            this.txtDiscount.Text = Convert.ToString(couponModel.Discount);
            this.DTPFromDate.SelectedDate = CommonFunctions.ParseDateToFinclave(couponModel.FromDate).Date;
            this.DTPToDate.SelectedDate = CommonFunctions.ParseDateToFinclave(couponModel.ToDate).Date;
        }

        private void cmbOfferType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOfferType.SelectedIndex > 0)
            {
                this.OfferType = Convert.ToString(cmbOfferType.SelectedIndex);
            }
        }

        private void txtDiscount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        //private void txtDiscount_LostFocus1(object sender, RoutedEventArgs e)
        //{

        //    if (cmbOfferType.SelectedIndex > 0)
        //    {

        //        objValidate.ValidateTextBoxForDiscount(sender, CallingPage, (string)Application.Current.Resources["Offer_valueExeededErrorMsg"]);
        //        //if (!string.IsNullOrEmpty(txtDiscount.Text) && Convert.ToDecimal(txtDiscount.Text) > 100 && Convert.ToString(CommonFunctions.GetValueFromDescription<Utility.CommonEnums.CommonEnum.OfferType>(cmbOfferType.Text)) == Enum.GetName(typeof(FinPos.Utility.CommonEnums.CommonEnum.OfferType), (int)CommonEnum.OfferType.MinimumPurchase))
        //        //{
        //        //    CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["Offer_valueExeededErrorMsg"], CallingPage);
        //        //    txtDiscount.Text = "";
        //        //}
        //    }
        //    else
        //    {
        //        CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["offer_Discount_Type_ErrorMsg"], CallingPage);
        //    }
        //}
        private void txtDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbOfferType.SelectedIndex > 0)
            {
                if (!string.IsNullOrEmpty(txtDiscount.Text) && Convert.ToDecimal(txtDiscount.Text) > 100 && Convert.ToString(CommonFunctions.GetValueFromDescription<Utility.CommonEnums.CommonEnum.OfferType>(cmbOfferType.Text)) == Enum.GetName(typeof(FinPos.Utility.CommonEnums.CommonEnum.OfferType), (int)CommonEnum.OfferType.MinimumPurchase))
                {
                    CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["Offer_valueExeededErrorMsg"], CallingPage);
                    txtDiscount.Text = "";
                }
            }
            else
            {
                CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["offer_Discount_Type_ErrorMsg"], CallingPage);
            }
        }

        private void txtMinimumValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void txtMinimumValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.IntegerValueChecker(sender, e);
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            //this.IsEnabled = true;
            //btn_remove.IsEnabled = false;
            //btn_remove.Background = Brushes.Gray;
        }
        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = false;
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            if (productItem != null)
            {
                AddItemsToProductList(productItem);
            }
        }
        private void lvProducts_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = (sender as System.Windows.Controls.ListViewItem);
            //dynamic row = e.Source;
            if (item != null || item.IsSelected)
            {
                btn_remove.IsEnabled = true;
                btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex > 0)
            {
                if (offerDetails == null)
                {
                    offerDetails = new ObservableCollection<OfferdetailModel>();
                    offerDetails.Add(new OfferdetailModel() { ProductCode = 0 });
                }
                lvProducts.ItemsSource = offerDetails;
                //BindDiscountType();
            }
            this.TabControlOfferTypeSelectdIndex = tabControl.SelectedIndex;
        }

        private void AddItemsToProductList(ProductModel productItem)
        {
            if (offerDetails.Any(x => x.ProductCode == productItem.Id) && _offerDetails.ProductCode != productItem.Id)
            {
                // Common.ErrorMessage((string)Application.Current.Resources["purchase_already"], CallingPage);
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["Offer_ProductproductExists"], CallingPage, true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    AddItemSource(productItem);
                }
            }
            else
            {
                AddItemSource(productItem);
            }
        }
        private void AddItemSource(ProductModel productModel)
        {
            OfferdetailModel objOfferDetail = new OfferdetailModel();
            if (productModel != null)
            {
                objOfferDetail.ProductCode = productModel.Id;
                objOfferDetail.ProductName = productModel.ItemName;
                offerDetails[rowIndex] = objOfferDetail;
                lvProducts.ItemsSource = offerDetails;
            }
        }

        private void btn_addRow_Click(object sender, RoutedEventArgs e)
        {
            offerDetails.Add(new OfferdetailModel() { ProductCode = 0 });
        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            System.Windows.Controls.ListViewItem lvi = CommonFunction.Common.GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lvProducts.SelectedIndex =
                    lvProducts.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lvProducts.SelectedIndex;
                _offerDetails = (OfferdetailModel)lvProducts.SelectedItem;

            }
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

        private void txtDiscount_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void txtDiscount_LostFocus_1(object sender, RoutedEventArgs e)
        {///todo
            var check = ((System.Windows.FrameworkElement)e.OriginalSource).DataContext as OfferdetailModel;
            //if (check.DiscountType != 0)
            //{
            //    if (!string.IsNullOrEmpty(Convert.ToString(check.Discount)) && Convert.ToDecimal(Convert.ToString(check.Discount)) > 100 && check.DiscountType == (int)CommonEnum.DiscountType.Percentage)
            //    {
            //        CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["Offer_valueExeededErrorMsg"], CallingPage);
            //        check.Discount = 0;
            //    }
            //}
            //else
            //{
            //    CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["coupon_Discount_Type_ErrorMsg"], CallingPage);
            //}
        }

        private void cmbDiscountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = ((System.Windows.FrameworkElement)e.Source).DataContext as OfferdetailModel;

            row.DiscountType = Convert.ToString(Enum.Parse(typeof(CommonEnum.DiscountType), ((System.Windows.Controls.Primitives.Selector)e.Source).SelectedValue.ToString()));
        }

        //private void cmbDiscountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    OfferdetailModel context = ((System.Windows.FrameworkElement)e.Source).DataContext as OfferdetailModel;

        //    //this.context.DiscountType = ((System.Windows.Controls.Primitives.Selector)sender).SelectedItem
        //}



        //private void btn_save_Offer_Click(object sender, RoutedEventArgs e)
        //{

        //    List<OfferdetailModel> Stocks = lvProducts.Items.Cast<OfferdetailModel>().Select(x => x).ToList();
        //}
        #endregion
        //private void BindDiscountType()
        //{
        //    var discountTypes = Enum.GetNames(typeof(CommonEnum.DiscountType)).ToList();
        //    discountTypes.Insert(0, (string)Application.Current.Resources["Combo_Select"]);
        //    cmbDiscountType.ItemsSource = discountTypes;
        //    cmbDiscountType.SelectedIndex = 0;
        //}

        //private void cmbDiscountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbDiscountType.SelectedIndex > 0)
        //        this.DiscountType = Convert.ToString(cmbDiscountType.SelectedIndex);
        //}
    }
}

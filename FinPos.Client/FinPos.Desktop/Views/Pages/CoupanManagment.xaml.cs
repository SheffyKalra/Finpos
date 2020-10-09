using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
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
    /// Interaction logic for CoupanManagment.xaml
    /// </summary>
    public partial class CoupanManagment : Page
    {
        #region Properties
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private IList<CouponModel> _coupons;
        private IList<OfferModel> _offers;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private CouponManagementController controller = new CouponManagementController();
        BrushConverter color = new BrushConverter();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion

        #region Constructor
        public CoupanManagment()
        {
            PageEvents();
            rdbCouponManagment.IsChecked = true;
            Enable_Disable_Buttons(false);
            ToggleGrid();
            ChangeHeightWidth();
        }
        public CoupanManagment(string page)
        {
            PageEvents();
            if (!string.IsNullOrEmpty(page))
            {
                if (page == (string)Application.Current.Resources["Coupons_leftHeader"])
                    rdbCouponManagment.IsChecked = true;
                else
                    rdbOffers.IsChecked = true;
                Enable_Disable_Buttons(false);
                ToggleGrid();
            }
        }
        #endregion

        #region Common Methods
        private void PageEvents()
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        public void ChangeHeightWidth()
        {
            this.CouponManagementPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.CouponManagementPage.Width = HeightWidth.width;

        }

        private void Enable_Disable_Buttons(bool isEnabled)
        {
            btn_Delete.IsEnabled = isEnabled;
            btnEditCoupon.IsEnabled = isEnabled;
            if (!isEnabled)
            {
                btnEditCoupon.Background = Brushes.Gray;
                btn_Delete.Background = Brushes.Gray;
            }
            else
            {
                btnEditCoupon.Background = (Brush)color.ConvertFrom("#0091EA");
                btn_Delete.Background = (Brush)color.ConvertFrom("#eb5151");
            }
        }
        private void AddClick()
        {
            AddCoupon _addCoupon = new AddCoupon(Convert.ToString(page_name.Content));
            NavigationService.Navigate(_addCoupon);
        }
        private void BindCouponsOrOffers()
        {
            if (rdbCouponManagment.IsChecked.Value)
            {
                ResponseVm responce = controller.GetCoupons();
                _coupons = responce.Response.Cast<CouponModel>().ToList();
                lstCoupons.ItemsSource = _coupons;
            }
            else
            {
                ResponseVm responce = controller.GetOffers();
                _offers = responce.Response.Cast<OfferModel>().ToList();
                lstOffers.ItemsSource = _offers;
            }
        }
        private void EditClick()
        {
            EditCoupon _editCoupon;
            if (rdbCouponManagment.IsChecked == true)
            {
                var row = lstCoupons.SelectedItem;
                _editCoupon = new EditCoupon(row, (string)Application.Current.Resources["Coupons_leftHeader"]);
            }
            else
            {
                var row = lstOffers.SelectedItem;
                _editCoupon = new EditCoupon(row, (string)Application.Current.Resources["Offers_leftHeader"]);
            }
            NavigationService.Navigate(_editCoupon);
        }
        /// <summary>
        /// This method is created to choose from coupons grid and Offers grid
        /// </summary>
        private void ToggleGrid()
        {
            if (rdbCouponManagment.IsChecked.Value)
            {
                ///Enable grid Coupon Management
                EnableGrid((string)Application.Current.Resources["Coupons_leftHeader"], false, true, Visibility.Visible, Visibility.Collapsed);
            }
            else
            {
                ///Enable grid Offer Management
                EnableGrid((string)Application.Current.Resources["Offers_leftHeader"], true, false, Visibility.Collapsed, Visibility.Visible);
            }
            BindCouponsOrOffers();
        }

        /// <summary>
        /// This method will enable the grid as per the input 
        /// </summary>
        /// <param name="pagename">Calling page either Coupon Managment or Offer Managment</param>
        /// <param name="isrdbOffersChecked">radio button Offer</param>
        /// <param name="isrdbCouponManagmentCheck"> radio button Coupon Managment</param>
        /// <param name="visCoupon"> holds the visibility of Coupon Radio Button</param>
        /// <param name="visOffer">holds the visibility of Offer Radio Button</param>
        private void EnableGrid(string pagename, bool isrdbOffersChecked, bool isrdbCouponManagmentCheck, Visibility visCoupon, Visibility visOffer)
        {
            page_name.Content = pagename;
            //  rdbOffers.IsChecked = isrdbOffersChecked;
            // rdbCouponManagment.IsChecked = isrdbCouponManagmentCheck;
            gridCoupons.Visibility = visCoupon;
            gridOffers.Visibility = visOffer;
        }
        #endregion

        #region Events
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void btnAddCoupons_Click(object sender, RoutedEventArgs e)
        {
            AddClick();
        }

        private void btnEditCoupon_Click(object sender, RoutedEventArgs e)
        {
            EditClick();
        }

        private void CouponsGridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lstCoupons.Items.SortDescriptions.Clear();
            }
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;
            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lstCoupons.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void OffersGridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lstOffers.Items.SortDescriptions.Clear();
            }
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;
            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lstOffers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void lstCoupon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null || item.IsSelected)
            {
                Enable_Disable_Buttons(true);
            }
        }

        private void btnAddOffer_Click(object sender, RoutedEventArgs e)
        {
            AddClick();
        }

        private void btnEditOffers_Click(object sender, RoutedEventArgs e)
        {
            EditClick();
        }

        private void rdbCouponManagment_Checked(object sender, RoutedEventArgs e)
        {
            ToggleGrid();
        }


        private void rdbOffers_Checked(object sender, RoutedEventArgs e)
        {
            ToggleGrid();
        }

        private void lstOffers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lstOffers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lstCoupons_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lstCoupons_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion
    }
}

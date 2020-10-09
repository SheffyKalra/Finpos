using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.Client.Model;
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

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditCoupon.xaml
    /// </summary>
    public partial class EditCoupon : Page

    {

        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #region Properties
        private int rowId;
        CommonFunction.Validations objValidations;
        CouponManagementController _CouponManagmentControllers;
        IList<CouponDetailsModel> objCouponDetails;
        private string parentPage;
        #endregion

        #region Constructor
        public EditCoupon(dynamic row, string callingPage)
        {
            _CouponManagmentControllers = new CouponManagementController();
            InitializeComponent();
            ChangeHeightWidth();
            rowId = row.Id;
            EnableUC(row, callingPage);
            parentPage = callingPage;
            objValidations = new CommonFunction.Validations();
        }
        #endregion
      
        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.EditCouponPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.EditCouponPage.Width = HeightWidth.width;
        }
        private void EnableUC(dynamic row, string callingPage)
        {
            if (callingPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {
                EnableDisableUC(Visibility.Visible,Visibility.Collapsed);
                CouponModel coupanmodel = new CouponModel(row.Id, "Edit Coupon", row.DiscountType, row.FromDate, row.ToDate, row.Value, row.NoOfCoupons, row.Name);
                objCouponDetails = _CouponManagmentControllers.GetCouponDetails(row.Id);
                lstCouponsDetails.ItemsSource = objCouponDetails;
                this.UCManageCoupon.SetText(coupanmodel);

            }
            else if (callingPage == (string)Application.Current.Resources["Offers_leftHeader"])
            {
                lstCouponsDetails.Visibility = Visibility.Collapsed;
                EnableDisableUC(Visibility.Collapsed, Visibility.Visible);
                if (row.OfferType != Utility.CommonMethods.CommonFunctions.GetDescriptionFromEnumValue(
                        (Utility.CommonEnums.CommonEnum.OfferType)Enum.Parse(typeof(Utility.CommonEnums.CommonEnum.OfferType), Convert.ToString((int)Utility.CommonEnums.CommonEnum.OfferType.ItemSpecific))))
                {
                    OfferModel offerModel = new OfferModel(row.Id, "Edit Offer", row.OfferType, row.FromDate, row.ToDate, row.Discount, row.MinimumValue, row.Name);
                    this.UCMangeOffer.SetText(offerModel);
                }
                else
                {
                    OfferModel offerModel = new OfferModel(row.Id, "Edit Offer", row.Name);
                    this.UCMangeOffer.SetItemSpecificValues(offerModel);
                }
            }
        }
        private void SuccessRetrun()
        {
            if (parentPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {
                Common.Notification((string)Application.Current.Resources["coupon_saveSuccessMsg"], this.GetType().Name, false);
            }
            else
            {
                Common.Notification((string)Application.Current.Resources["offer_saveSuccessMsg"], this.GetType().Name, false);
            }
            NavigateToParentPage();
        }
        private void NavigateToParentPage()
        {
            CoupanManagment objCoupanManagment = new CoupanManagment(parentPage);
            NavigationService.Navigate(objCoupanManagment);
        }
        private void EnableDisableUC(Visibility visUSManageCoupon, Visibility visUSManageOffer)
        {
            this.UCManageCoupon.Visibility = visUSManageCoupon;
            this.UCMangeOffer.Visibility = visUSManageOffer;
        }
        #endregion

        #region Events
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            NavigateToParentPage();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (parentPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {
                if (objValidations.CouponValidation(UCManageCoupon))
                {
                    CouponModel objSave = new CouponModel(rowId, UCManageCoupon.DiscountType, UCManageCoupon.FromDate,
                   UCManageCoupon.ToDate, UCManageCoupon.NoOfCoupons, UCManageCoupon.Value, UserModelVm.CompanyId, UserModelVm.BranchId, UCManageCoupon.CouponName, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()));
                    if (_CouponManagmentControllers.SaveCouponDetails(objSave))
                        SuccessRetrun();
                }
            }
            else
            {
                if (UCMangeOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.MinimumPurchaseAndDateTimeSpecific)
                {
                    if (objValidations.OfferValidation(UCMangeOffer))
                    {
                        OfferModel objSave = new OfferModel(rowId, UCMangeOffer.OfferType, UCMangeOffer.FromDate,
                                          UCMangeOffer.ToDate, UCMangeOffer.Discount, UCMangeOffer.MinimumValue, UserModelVm.CompanyId, UserModelVm.BranchId, UCMangeOffer.OfferName, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()));
                        bool isSaved = _CouponManagmentControllers.SaveOfferDetails(objSave);
                        if (_CouponManagmentControllers.SaveOfferDetails(objSave))
                            SuccessRetrun();
                    }
                }
                else if (UCMangeOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.ItemSpecific)
                {
                    List<OfferdetailModel> listOffer = UCMangeOffer.lvProducts.Items.Cast<OfferdetailModel>().Select(x => x).Select(y => new OfferdetailModel(y.Id, y.OfferId, y.ProductCode, y.Discount, y.FromDate, y.ToDate,
                        Convert.ToString(Enum.Parse(typeof(CommonEnum.DiscountType), Convert.ToString(y.DiscountType))), y.ProductName)).ToList();
                    OfferModel offerDetails = new OfferModel(rowId, Convert.ToString((int)CommonEnum.OfferType.ItemSpecific), UserModelVm.CompanyId,
                                      UserModelVm.BranchId, UCMangeOffer.txtOfferName.Text, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), listOffer);
                    if (_CouponManagmentControllers.SaveItemSpecificOfferDetails(offerDetails))
                        SuccessRetrun();
                }
            }
        }
        #endregion

       private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
            
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (parentPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {
                UCManageCoupon.ChangeHeightWidth();
            }
            else if (parentPage == (string)Application.Current.Resources["Offers_leftHeader"])
            {
                UCMangeOffer.ChangeHeightWidth();
            }
        }
    }
}

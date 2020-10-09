using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.Client.Model;
using FinPos.Data.Entities;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using FinPos.Utility.Constants;
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
    /// Interaction logic for AddCoupon.xaml
    /// </summary>
    public partial class AddCoupon : Page
    {
        #region Properties
        private static Random random = new Random();
        CouponManagementController _CouponManagmentControllers;
        CommonFunction.Validations objValidations;
        private string parentPage;
        #endregion
        #region Constructor
        public AddCoupon(string callingPage)
        {
            _CouponManagmentControllers = new CouponManagementController();
            InitializeComponent();
            ChangeHeightWidth();
            if (callingPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {

                this.UCManageCoupon.Visibility = Visibility.Visible;
                this.UCManageOffer.Visibility = Visibility.Collapsed;
                CouponModel coupanmodel = new CouponModel(0, "Add Coupon", (string)Application.Current.Resources["Combo_Select"], CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), Convert.ToDecimal(0), 0, "");
                UCManageCoupon.SetText(coupanmodel);
                objValidations = new CommonFunction.Validations();

            }
            else if (callingPage == (string)Application.Current.Resources["Offers_leftHeader"])
            {
                this.UCManageCoupon.Visibility = Visibility.Collapsed;
                this.UCManageOffer.Visibility = Visibility.Visible;
                OfferModel coupanmodel = new OfferModel(0, "Add Offer", (string)Application.Current.Resources["Combo_Select"], CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), Convert.ToDecimal(0), 0, "");
                UCManageOffer.SetText(coupanmodel);
            }
            parentPage = callingPage;
        }
        #endregion
        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.AddCouponPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddCouponPage.Width = HeightWidth.width;
        }
        private void NavigateToParentPage()
        {
            CoupanManagment objCoupanManagment = new CoupanManagment(parentPage);
            NavigationService.Navigate(objCoupanManagment);
        }
        #endregion
        #region CRUD Methods
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
                    CouponModel objSave = new CouponModel(0, UCManageCoupon.DiscountType, UCManageCoupon.FromDate,
                                      UCManageCoupon.ToDate, UCManageCoupon.NoOfCoupons, UCManageCoupon.Value, UserModelVm.CompanyId, UserModelVm.BranchId, UCManageCoupon.CouponName, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()));
                    List<CouponDetailsModel> objListCoupons = GenerateCoupons();
                    objSave.CouponDetails = objListCoupons;
                    bool isSaved = _CouponManagmentControllers.SaveCouponDetails(objSave);
                    if (isSaved)
                        SuccessRetrun();
                }

            }
            else if (parentPage == (string)Application.Current.Resources["Offers_leftHeader"])
            {
                if (UCManageOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.MinimumPurchaseAndDateTimeSpecific)
                {
                    OfferModel offerDetails = new OfferModel(0, UCManageOffer.OfferType, UCManageOffer.FromDate,
                                      UCManageOffer.ToDate, UCManageOffer.Discount, UCManageOffer.MinimumValue, UserModelVm.CompanyId, UserModelVm.BranchId, UCManageOffer.OfferName, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()));
                    if (_CouponManagmentControllers.SaveOfferDetails(offerDetails))
                        SuccessRetrun();
                }
                else if (UCManageOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.ItemSpecific)
                {
                    List<OfferdetailModel> listOffer = UCManageOffer.lvProducts.Items.Cast<OfferdetailModel>().Select(x => x).Select(y => new OfferdetailModel(0, 0, y.ProductCode, y.Discount, y.FromDate, y.ToDate,
                        y.DiscountType, y.ProductName)).ToList();
                    OfferModel offerDetails = new OfferModel(0, Convert.ToString((int)CommonEnum.OfferType.ItemSpecific), UserModelVm.CompanyId,
                                      UserModelVm.BranchId, UCManageOffer.txtOfferName.Text, true, false, UserModelVm.UserId, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.Date.ToShortDateString()), listOffer);
                    if (_CouponManagmentControllers.SaveItemSpecificOfferDetails(offerDetails))
                        SuccessRetrun();
                }
            }
        }

        private void SuccessRetrun()
        {
            if (UCManageOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.MinimumPurchaseAndDateTimeSpecific)
                Common.Notification((string)Application.Current.Resources["coupon_saveSuccessMsg"], this.GetType().Name, false);
            else if (UCManageOffer.TabControlOfferTypeSelectdIndex == (int)CommonEnum.ManageOfferTabControls.ItemSpecific)
                Common.Notification((string)Application.Current.Resources["offer_saveSuccessMsg"], this.GetType().Name, false);
            NavigateToParentPage();
        }

        public static string RandomString(int length)
        {
            const string chars = CommonConstants._randomNumber;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private List<CouponDetailsModel> GenerateCoupons()
        {
            CouponDetailsModel coupon;
            List<CouponDetailsModel> objListCoupons = new List<CouponDetailsModel>();
            for (int i = 0; i < UCManageCoupon.NoOfCoupons; i++)
            {
                coupon = new CouponDetailsModel(null, 0, RandomString(8), false, CommonFunctions.ParseDateToFinclaveString(default(DateTime).Date.ToShortDateString()));
                objListCoupons.Add(coupon);
            }
            return objListCoupons;
        }
        #endregion

        public void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (parentPage == (string)Application.Current.Resources["Coupons_leftHeader"])
            {
                UCManageCoupon.ChangeHeightWidth();
            }
            else if (parentPage == (string)Application.Current.Resources["Offers_leftHeader"])
            {
                UCManageOffer.ChangeHeightWidth();
            }
        }
    }
}

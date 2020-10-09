using FinPos.Client.Views;
using FinPos.Client.Views.UserControls;
using FinPos.Utility.CommonEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FinPos.Client.Views.Pages.UserControls;

namespace FinPos.Client.CommonFunction
{
    public class Validations
    {
        /// <summary>
        /// This method is used for validating Manage coupons fields before save it is used at two diffrent places 
        /// at AddCoupon.xaml.cs and EditCoupon.xaml.cs
        /// </summary>
        /// <param name="ucManageCoupon">object of the User Control</param>
        /// <returns>after validating it returns the result as true if everything goes fine and false for any unexpeced error</returns>
        public bool CouponValidation(ManageCoupon ucManageCoupon)
        {
            if (ucManageCoupon.DiscountType != null && ucManageCoupon.DiscountType != "0")
            {
                if (!string.IsNullOrWhiteSpace(ucManageCoupon.Name))
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(ucManageCoupon.Value))
                        &&
                        (ucManageCoupon.Value <= Convert.ToDecimal(100) &&
                        (string)Enum.GetName(typeof(CommonEnum.DiscountType),
                        Convert.ToInt32(ucManageCoupon.DiscountType)) ==
                        (string)Enum.GetName(typeof(CommonEnum.DiscountType), (int)CommonEnum.DiscountType.Percentage)) ||
                        ((string)Enum.GetName(typeof(CommonEnum.DiscountType),
                        Convert.ToInt32(ucManageCoupon.DiscountType)) ==
                        (string)Enum.GetName(typeof(CommonEnum.DiscountType), (int)CommonEnum.DiscountType.Amount)))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ucManageCoupon.Value)) && ucManageCoupon.Value != Convert.ToDecimal("0"))
                        {
                            if (Convert.ToDateTime(ucManageCoupon.ToDate).Date >= Convert.ToDateTime(ucManageCoupon.FromDate).Date)
                            {
                                return true;
                            }
                            else
                            {
                                Common.ErrorMessage((string)Application.Current.Resources["coupon_DateErrorMsg"], ucManageCoupon.CallingPage);
                            }
                        }
                        else
                        {
                            Common.ErrorMessage((string)Application.Current.Resources["coupon_Value_ErrorMsg"], ucManageCoupon.CallingPage);
                        }
                    }
                    else
                    {
                        Common.ErrorMessage((string)Application.Current.Resources["coupon_valueExeededErrorMsg"], ucManageCoupon.CallingPage);
                    }
                }
                else
                {
                    Common.ErrorMessage((string)Application.Current.Resources["coupon_Name_ErrorMsg"], ucManageCoupon.CallingPage);
                }
            }
            else
            {
                Common.ErrorMessage((string)Application.Current.Resources["coupon_Discount_Type_ErrorMsg"], ucManageCoupon.CallingPage);
            }
            return false;
        }
        /// <summary>
        /// method is used to check textbox value less then or equal to 100
        /// </summary>
        /// <param name="sender">textbox object</param>
        /// <param name="pageName">calling page</param>
        /// <param name="errorMessage">error messege</param>
        public void ValidateTextBoxForDiscount(object sender, string pageName, string errorMessage)
        {
            TextBox textbox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textbox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textbox.Text = string.Empty;
           else if (!string.IsNullOrEmpty(textbox.Text))
            {
                if (Convert.ToDecimal(textbox.Text) > 100)
                {
                    Common.ErrorMessage(errorMessage, pageName);
                    textbox.Text = string.Empty;
                }
            }
        }
        /// <summary>
        /// This method is used for validating Manage Offer UC fields before save it is used at two diffrent places 
        /// at AddCoupon.xaml.cs and EditCoupon.xaml.cs
        /// </summary>
        /// <param name="ucManageOffer"></param>
        /// <returns>after validating it returns the result as true if everything goes fine and false for any unexpeced error</returns>
        public bool OfferValidation(ManageOffer ucManageOffer)
        {
            if (ucManageOffer.OfferType != null && ucManageOffer.OfferType != "0")
            {
                if (!string.IsNullOrWhiteSpace(ucManageOffer.Name))
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(ucManageOffer.Discount))
                        &&
                        (ucManageOffer.Discount <= Convert.ToDecimal(100) &&
                        (string)Enum.GetName(typeof(CommonEnum.OfferType),
                        Convert.ToInt32(ucManageOffer.OfferType)) ==
                        (string)Enum.GetName(typeof(CommonEnum.OfferType), (int)CommonEnum.OfferType.MinimumPurchase)) ||
                        ((string)Enum.GetName(typeof(CommonEnum.OfferType),
                        Convert.ToInt32(ucManageOffer.OfferType)) !=
                        (string)Enum.GetName(typeof(CommonEnum.OfferType), (int)CommonEnum.OfferType.MinimumPurchase)))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(ucManageOffer.Discount)) && ucManageOffer.Discount != Convert.ToDecimal("0"))
                        {
                            if (Convert.ToDateTime(ucManageOffer.ToDate).Date >= Convert.ToDateTime(ucManageOffer.FromDate).Date)
                            {
                                return true;
                            }
                            else
                            {
                                Common.ErrorMessage((string)Application.Current.Resources["coupon_DateErrorMsg"], ucManageOffer.CallingPage);
                            }
                        }
                        else
                        {
                            Common.ErrorMessage((string)Application.Current.Resources["Offer_valueExeededErrorMsg"], ucManageOffer.CallingPage);
                        }
                    }
                    else
                    {
                        Common.ErrorMessage((string)Application.Current.Resources["Offer_valueExeededErrorMsg"], ucManageOffer.CallingPage);
                    }
                }
                else
                {
                    Common.ErrorMessage((string)Application.Current.Resources["coupon_Name_ErrorMsg"], ucManageOffer.CallingPage);
                }
            }
            else
            {
                Common.ErrorMessage((string)Application.Current.Resources["offer_Discount_Type_ErrorMsg"], ucManageOffer.CallingPage);
            }
            return false;
        }
        public void UpdateColumnsWidth(ListView listView)
        {
            var gridView = listView.View as GridView;
            if (gridView != null)
            {

                if (double.IsNaN(listView.ActualWidth))
                    listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                double colWidth = listView.ActualWidth / gridView.Columns.Count;
                for (int i = 0; i < gridView.Columns.Count; ++i)
                {
                    var column = gridView.Columns[i].Header;
                    var mwidth = ((System.Windows.FrameworkElement)column).MinWidth;
                    gridView.Columns[i].Width = (mwidth > colWidth) ? mwidth : colWidth;

                }
            }
        }
        public bool PaymentValidation(ManagePaymentToSupplier payment,string page)
        {
            try
            {
                bool isValid = false;
                if (string.IsNullOrWhiteSpace(payment.invoiceNo_.Text) || string.IsNullOrWhiteSpace(payment.supplierName_.Text)|| string.IsNullOrWhiteSpace(payment.bank_.Text)|| string.IsNullOrWhiteSpace(payment.accountNo_.Text) || string.IsNullOrWhiteSpace(payment.amount_.Text) )
                {
                    Common.ErrorMessage((string)Application.Current.Resources["Payment_requiredFileds_Msg"], page);
                    return isValid = true;
                }
                else if(string.IsNullOrEmpty(payment.paymentDate_.Text))
                {
                    Common.ErrorMessage((string)Application.Current.Resources["Payment_paymentDateFileds_Msg"], page);
                    return isValid = true;
                }
                else if (Convert.ToDecimal(payment.amount_.Text)>Convert.ToDecimal(payment.pendingAmount_.Text))
                {
                    Common.ErrorMessage((string)Application.Current.Resources["Payment_exceed_Msg"], page);
                    return isValid = true;
                }
                return isValid;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

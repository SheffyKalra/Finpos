using FinPos.Client.Model;
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
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using FinPos.Utility.CommonEnums;
using FinPos.Client.Controls;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ManageCoupon.xaml
    /// </summary>
    public partial class ManageCoupon : UserControl
    {
        public string CallingPage { get { return this.page_Name.Content.ToString(); } set { } }
        public string DiscountType { get { { return Convert.ToString(this.cmbDiscountType.SelectedIndex); } } set { } }
        public string FromDate { get { return this.DTPFromDate.SelectedDate.Value.Date.ToShortDateString(); } set { } }
        public string ToDate { get { return this.DTPToDate.SelectedDate.Value.Date.ToShortDateString(); } set { } }
        public int NoOfCoupons { get { return Convert.ToInt32(this.txtQuantity.Text); } set { } }
        public decimal Value
        {
            get
            {
                return
            Convert.ToDecimal(txtValue.Text == null ? "0" : txtValue.Text == "" ? "0" : txtValue.Text);
            }
            set { this.txtValue.Text = Convert.ToString(Value); }
        }
        public object ParentPage { get; set; }
        public string CouponName { get { return this.txtName.Text; } set { } }
        public ManageCoupon()
        {
            InitializeComponent();
            BindDiscountType();
            SetTextBoxsToEmpty();
            SetCalender();
            ChangeHeightWidth();
        }

        private void SetCalender()
        {
            CalendarDateRange cdr = new CalendarDateRange(CommonFunctions.ParseDateToFinclave(DateTime.MinValue.ToShortTimeString()), CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()).AddDays(-1));
            DTPFromDate.BlackoutDates.Add(cdr);
            DTPToDate.BlackoutDates.Add(cdr);
        }

        private void SetTextBoxsToEmpty()
        {
            txtName.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtValue.Text = string.Empty;
        }

        private void BindDiscountType()
        {
            var discountTypes = Enum.GetNames(typeof(FinPos.Utility.CommonEnums.CommonEnum.DiscountType)).ToList();
            discountTypes.Insert(0, (string)Application.Current.Resources["Combo_Select"]);
            cmbDiscountType.ItemsSource = discountTypes;
            cmbDiscountType.SelectedIndex = 0;
        }

        public void SetText(CouponModel couponModel)
        {
            if (couponModel.Id == 0)
            {
                txtQuantity.IsEnabled = true;
            }
            this.page_Name.Content = couponModel.CallingPage;
            this.CallingPage = couponModel.CallingPage;
            this.cmbDiscountType.Text = couponModel.DiscountType;
            this.txtName.Text = couponModel.Name;
            this.txtQuantity.Text = Convert.ToString(couponModel.NoOfCoupons);
            this.txtValue.Text = Convert.ToString(couponModel.Value);
            //this.DTPFromDate.SelectedDate = Convert.ToDateTime(couponModel.FromDate).Date;
            //this.DTPToDate.SelectedDate = Convert.ToDateTime(couponModel.ToDate).Date;

        }

        private void cmbDiscountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDiscountType.SelectedIndex > 0)
            {
                this.DiscountType = Convert.ToString(cmbDiscountType.SelectedIndex);
            }
        }

        private void DTPFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTPFromDate.SelectedDate != null && DTPToDate.SelectedDate != null && DTPFromDate.SelectedDate <= DTPToDate.SelectedDate)
            {
                this.FromDate = DTPFromDate.SelectedDate.Value.Date.ToShortDateString();
            }
            else if (DTPFromDate.SelectedDate != null && DTPToDate.SelectedDate != null)
            {
                CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["coupon_DateErrorMsg"], CallingPage);
            }
        }

        //private void ErrorMessage(string errorMessage)
        //{
        //    ConfirmationPopup form = new ConfirmationPopup(errorMessage, CallingPage, false);
        //    form.ShowDialog();
        //}
        public void ChangeHeightWidth()
        {
            this.ManageCoupUC.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.ManageCoupUC.Width = HeightWidth.width;
        }
        private void DTPToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DTPFromDate.SelectedDate != null && DTPToDate.SelectedDate != null && DTPToDate.SelectedDate >= DTPFromDate.SelectedDate)
            {
                this.ToDate = DTPToDate.SelectedDate.Value.Date.ToShortDateString();
            }
            else if (DTPFromDate.SelectedDate != null && DTPToDate.SelectedDate != null)
            {
                CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["coupon_DateErrorMsg"], CallingPage);
            }
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtQuantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void txtValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;

        }

        private void txtValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbDiscountType.SelectedIndex > 0)
            {
                if (!string.IsNullOrEmpty(txtValue.Text) &&  Convert.ToDecimal(txtValue.Text) > 100 && cmbDiscountType.Text == (string)Enum.GetName(typeof(FinPos.Utility.CommonEnums.CommonEnum.DiscountType), (int)CommonEnum.DiscountType.Percentage))
                {
                    CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["coupon_valueExeededErrorMsg"], CallingPage);
                    txtValue.Text = "";
                }
            }
            else
            {
                CommonFunction.Common.ErrorMessage((string)Application.Current.Resources["coupon_Discount_Type_ErrorMsg"], CallingPage);
            }
        }

        private void txtValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.DecimalValueChecker(sender, e);
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.IntegerValueChecker(sender, e);
        }

    }
}

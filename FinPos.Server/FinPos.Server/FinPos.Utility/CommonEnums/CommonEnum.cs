using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Utility.CommonEnums
{
    public class CommonEnum
    {
        public enum IndustryTypes
        {
            [Description("RetailAndSupermarket")]
            RS = 1,
            [Description("FashionAndBoutique")]
            FB = 2,
            [Description("BookStore")]
            BS = 3,
            [Description("RestaurantAndBar")]
            RB = 4,
            [Description("PharmaAndHealthcare")]
            PH = 5
        }

        public enum ItemTypes
        {
            [Description("Standard")]
            Standard = 1,
            [Description("Service")]
            Service = 2,
            [Description("Kit")]
            Kit = 3,
            [Description("Package")]
            Package = 4,
            [Description("Bulk")]
            Bulk = 5,
            [Description("RePack")]
            RePack = 6
        }

        public enum PurchaseTypes
        {
            [Description("Direct purchase")]
            DirectPurchase = 1,
            [Description("PO")]
            PO = 2
        }

        public enum PurchaseStatus
        {
            [Description("Direct purchased")]
            DirectPurchased = 1,
            [Description("Waiting For Approval")]
            WaitingForApproval = 2,
            [Description("Returned")]
            Returned = 3,
            [Description("Approved")]
            Approved = 4,
            [Description("Fully Returned")]
            FullyReturned = 5
        }
        public enum MonuthStatus
        {
            [Description("January")]
            January = 1,
            [Description("February")]
            February = 2,
            [Description("March")]
            March = 3,
            [Description("April")]
            April = 4,
            [Description("May")]
            FullyReturned = 5,
            [Description("June")]
            June = 6,
            [Description("July")]
            July = 7,
            [Description("August")]
            August = 8,
            [Description("September")]
            September = 9,
            [Description("October")]
            October = 10,
            [Description("November")]
            November = 11,
            [Description("December")]
            December = 12
        }
        public enum DiscountType
        {
            [Description("Percentage")]
            Percentage = 1,
            [Description("Amount")]
            Amount = 2,
        }
        public enum OfferType
        {
            [Description("Minimum Purchase")]
            MinimumPurchase = 1,
            [Description("Days & Time Specific")]
            DaysAndTime = 2,
            [Description("Item Specific")]
            ItemSpecific = 3,
        }
        public enum PaymentType
        {
            [Description("Cash")]
            Cash = 1,
            [Description("Cheque")]
            Cheque = 2,
            [Description("Transfer")]
            Transfer = 3,
        }
        //public enum PurchaseType
        //{
        //    [Description("Purchase Order")]
        //    PurchaseOrder = 1,
        //    [Description("Direct Purchase")]
        //    DirectPurchase = 2
        //}

        public enum ManageOfferTabControls
        {

            [Description("Minimum Purchase/Date Time specific")]
            MinimumPurchaseAndDateTimeSpecific = 0,
            [Description("Item Specific")]
            ItemSpecific = 1,
        }
    }
}

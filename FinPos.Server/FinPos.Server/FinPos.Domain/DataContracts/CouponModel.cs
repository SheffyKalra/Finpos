using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class CouponModel
    {
        public CouponModel(int? id, string callingPage, string discountType, string fromDate, string toDate, int noOfCoupons, decimal value, int companyId,
            int? branchId, string name, bool isActive, bool isDeleted, int? createdBy, string createdDate)
        {
            this.CallingPage = callingPage;
            SetValues(id, discountType, fromDate, toDate, noOfCoupons, value, companyId, branchId, name, isActive, createdBy);
        }
        public CouponModel(int? id, string discountType, string fromDate, string toDate, int noOfCoupons, decimal value, int companyId,
           int? branchId, string name, bool isActive, bool isDeleted, int? createdBy, string createdDate)
        {
            SetValues(id, discountType, fromDate, toDate, noOfCoupons, value, companyId, branchId, name, isActive, createdBy);
        }

        public CouponModel(int? id, string callingPage, string discountType, string fromDate, string toDate, decimal value, int noOfCoupons, string name)
        {
            this.Id = id;
            this.CallingPage = callingPage;
            this.DiscountType = discountType;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Value = value;
            this.NoOfCoupons = noOfCoupons;
            this.Name = name;
        }
        private void SetValues(int? id, string discountType, string fromDate, string toDate, int noOfCoupons, decimal value, int companyId, int? branchId, string name, bool isActive, int? createdBy)
        {
            this.Id = id;
            this.DiscountType = discountType;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.FromDate = fromDate;
            this.NoOfCoupons = noOfCoupons;
            this.Value = value;
            this.CompanyCode = companyId;
            this.BranchCode = branchId;
            this.Name = name;
            this.IsActive = isActive;
            this.IsDeleted = IsDeleted;
            this.CreatedBy = createdBy;
            this.CreatedDate = CreatedDate;
        }

        public CouponModel(string callingPage)
        {
            this.CallingPage = callingPage;
        }
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string CallingPage { get; set; }

        [DataMember]
        public string DiscountType { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public string FromDate { get; set; }

        [DataMember]
        public string ToDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string UpdatedDate { get; set; }

        [DataMember]
        public int? ModifiedBy { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public int? BranchCode { get; set; }

        [DataMember]
        public int NoOfCoupons { get; set; }
        [DataMember]
        public List<CouponDetailsModel> CouponDetails { get; set; }

        //#region extra properties for Offer Management 
        //public CouponModel(int? id, string callingPage, string offerType, string fromDate, string toDate, decimal minimumValue, int discount, string name)
        //{
        //    this.Id = id;
        //    this.CallingPage = callingPage;
        //    this. = discountType;
        //    this.FromDate = fromDate;
        //    this.ToDate = toDate;
        //    this.Value = value;
        //    this.NoOfCoupons = noOfCoupons;
        //    this.Name = name;
        //}

        ///// <summary>
        ///// Used for Offer managment
        ///// </summary>
        //[DataMember]
        //public int MinimumValue { get; set; }
        ///// <summary>
        ///// Used for Offer managment
        ///// </summary>
        //[DataMember]
        //public int Discount { get; set; }
        ///// <summary>
        ///// Used for Offer managment
        ///// </summary>
        //[DataMember]
        //public int OfferType { get; set; }
        //#endregion


    }

}

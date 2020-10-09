using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Coupon : BaseEntity
    {
        public Coupon()
        {

        }
        public Coupon(int? id, int discountType, decimal value, string fromDate,
            string toDate, string name, bool isActive, bool isDeleted, string createdDate
            , string updatedDate, int? modifiedBy, int? createdBy, int companyCode,
            int? branchCode, int noOfCoupons)
        {
            this.Id = id;
            this.DiscountType = discountType;
            this.CValue = value;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Name = name;
            this.IsActive = isActive;
            this.IsDelete = isDeleted;
            this.CreatedBy = createdBy;
            this.ModifiedBy = modifiedBy;
            this.CreatedDate = createdDate;
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
            this.NoOfCoupons = noOfCoupons;
            this.ModifiedDate = updatedDate;
        }
        //public Coupon(int discountType, int couponCode, decimal value, string fromDate,
        //    string toDate, string description, bool isActive, bool isDeleted, string createdDate
        //    , string updatedDate, int? modifiedBy, int? createdBy, int companyCode,
        //    int branchCode?, int noOfCoupons)
        //{
        //    this.DiscountType = discountType;
        //    this.CouponCode = couponCode;
        //    this.Value = value;
        //    this.FromDate = fromDate;
        //    this.ToDate = toDate;
        //    this.Description = description;
        //    this.IsActive = isActive;
        //    this.IsDeleted = isDeleted;
        //    this.CreatedBy = createdBy;
        //    this.UpdatedDate = updatedDate;
        //    this.ModifiedBy = modifiedBy;
        //    this.CreatedDate = createdDate;
        //    this.CompanyCode = companyCode;
        //    this.BranchCode = branchCode;
        //    this.NoOfCoupons = noOfCoupons;
        //}

        // public int? Id { get; set; }
        public int DiscountType { get; set; }
        public decimal CValue { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        //public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CreatedBy { get; set; }
        public int CompanyCode { get; set; }
        public int? BranchCode { get; set; }
        public int NoOfCoupons { get; set; }
    }
}

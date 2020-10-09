using FinPos.DAL.Entities;
namespace FinPos.Data.Entities
{
    public class Offer : BaseEntity
    {
        public Offer()
        {

        }
        public Offer(int? id, int offerType, decimal minimumValue, string fromDate,
            string toDate, string name, bool isActive, bool isDeleted, string createdDate
            , string updatedDate, int? modifiedBy, int? createdBy, int companyCode,
            int? branchCode, decimal discount)
        {
            this.Id = id;
            this.OfferType = offerType;
            this.Discount = discount;
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
            this.MinimumValue = minimumValue;
            this.ModifiedDate = updatedDate;
        }

        public int OfferType { get; set; }
        public decimal Discount { get; set; }
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
        public decimal MinimumValue { get; set; }
    }
}


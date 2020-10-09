using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class OfferModel
    {
        public OfferModel(int? id, string callingPage, string offerType, string fromDate, string toDate, decimal discount, decimal minValue, int companyId,
               int? branchId, string name, bool isActive, bool isDeleted, int? createdBy, string createdDate)
        {
            this.CallingPage = callingPage;
            SetValues(id, offerType, fromDate, toDate, discount, minValue, companyId, branchId, name, isActive, createdBy);
        }
        public OfferModel(int? id, string offerType, string fromDate, string toDate, decimal discount, decimal minValue, int companyId,
           int? branchId, string name, bool isActive, bool isDeleted, int? createdBy, string createdDate)
        {
            SetValues(id, offerType, fromDate, toDate, discount, minValue, companyId, branchId, name, isActive, createdBy);
        }
        public OfferModel(int? id, string offerType, int companyId,
           int? branchId, string name, bool isActive, bool isDeleted, int? createdBy, string createdDate, List<OfferdetailModel> OfferDetails)
        {
            this.Id = id;
            this.OfferType = offerType;
            this.CompanyCode = companyId;
            this.BranchCode = branchId;
            this.Name = name;
            this.IsActive = isActive;
            this.IsDeleted = IsDeleted;
            this.CreatedBy = createdBy;
            this.CreatedDate = CreatedDate;
            this.ItemSpecficOfferDetails = OfferDetails;
        }
        public OfferModel(int? id, string callingPage, string offerType, string fromDate, string toDate, decimal discount, decimal minimumValue, string name)
        {
            this.Id = id;
            this.CallingPage = callingPage;
            this.OfferType = offerType;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.MinimumValue = minimumValue;
            this.Discount = discount;
            this.Name = name;
        }
        private void SetValues(int? id, string offerType, string fromDate, string toDate, decimal discount, decimal minimumValue, int companyId, int? branchId, string name, bool isActive, int? createdBy)
        {
            this.Id = id;
            this.OfferType = offerType;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.FromDate = fromDate;
            this.MinimumValue = minimumValue;
            this.Discount = discount;
            this.CompanyCode = companyId;
            this.BranchCode = branchId;
            this.Name = name;
            this.IsActive = isActive;
            this.IsDeleted = IsDeleted;
            this.CreatedBy = createdBy;
            this.CreatedDate = CreatedDate;
        }
        public OfferModel(string callingPage)
        {
            this.CallingPage = callingPage;
        }
        public OfferModel(int? id, string callingPage, string OfferName)
        {
            this.Id = id;
            this.CallingPage = callingPage;
            this.Name = OfferName;
        }
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string CallingPage { get; set; }

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
        public List<OfferdetailModel> ItemSpecficOfferDetails { get; set; }

        [DataMember]
        public decimal MinimumValue { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public string OfferType { get; set; }


    }
}

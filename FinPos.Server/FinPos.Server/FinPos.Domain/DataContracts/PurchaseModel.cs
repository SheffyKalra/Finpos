using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public  class PurchaseModel
    {
        public PurchaseModel()
        {

        }
        public PurchaseModel(int? id, DateTime purchaseDate, int? suplierCode, decimal? discountPercentage, decimal? discountAmount,
             DateTime? delivertDate, DateTime? expiryDate, decimal? surChargeAmount, decimal? taxPercentage, int createdBy, string createdDate,
              int companyCode, int? branchCode, string supliername)
        {
            this.PurchaseId = id;
            this.PurchaseDate = purchaseDate;
            this.SuplierCode = suplierCode;
            this.DiscountPercentage = discountPercentage;
            this.DiscountAmount = discountAmount;
            this.DeliveryDate = delivertDate;
            this.ExpiryDate = expiryDate;
            this.SurChargeAmount = surChargeAmount;
            this.TaxPercentage = taxPercentage;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
            this.SuplierName = supliername;
        }
        [DataMember]
        public int? PurchaseId { get; set; }
        [DataMember]
        public DateTime PurchaseDate { get; set; }
        [DataMember]
        public int? SuplierCode { get; set; }

        [DataMember]
        public string SuplierName { get; set; }
        //[DataMember]
        //public int PurchaseType { get; set; }
        [DataMember]
        public decimal? DiscountPercentage { get; set; }
        [DataMember]
        public decimal? DiscountAmount { get; set; }

        [DataMember]
        public DateTime? DeliveryDate { get; set; }
        [DataMember]
        public DateTime? ExpiryDate { get; set; }
        [DataMember]
        public decimal? SurChargeAmount { get; set; }
        [DataMember]
        public decimal? TaxPercentage { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]

        public string CreatedDate { get; set; }

        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public int? BranchCode { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class PurchaseOrderModel
    {
        public PurchaseOrderModel() { }
        public PurchaseOrderModel(int? id, DateTime purchaseDate, int suplierCode, decimal? discountPercentage, decimal? discountAmount,
          DateTime? delivertDate, DateTime? expiryDate, decimal? surChargeAmount, decimal? taxPercentage, int createdBy, string createdDate,
          int status, DateTime? approvalDate, int? approvedBy, int companyCode, int? branchCode,string supliername,string statusName,string invoiceNo,string invoiceDate)
        {
            this.PurchaseId = id;
            this.PurchaseDate = purchaseDate;
            this.SuplierCode = suplierCode;
          //  this.PurchaseType = purchaseType;
            this.DiscountPercentage = discountPercentage;
            this.DiscountAmount = discountAmount;
            this.DeliveryDate = delivertDate;
            this.ExpiryDate = expiryDate;
            this.SurChargeAmount = surChargeAmount;
            this.TaxPercentage = taxPercentage;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
            this.Status = status;
            this.ApprovalDate = approvalDate;
            this.ApprovedBy = approvedBy;
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
            this.SuplierName = supliername;
            this.StatusName = statusName;
            this.InvoiceNo =invoiceNo;
            this.InvoiceDate =invoiceDate;
        }
        [DataMember]
        public int? PurchaseId { get; set; }
        [DataMember]
        public DateTime PurchaseDate { get; set; }
        [DataMember]
        public int SuplierCode { get; set; }

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
        public int Status { get; set; }

        [DataMember]
        public string StatusName { get; set; }
        [DataMember]

        public DateTime? ApprovalDate { get; set; }
        [DataMember]
        public int? ApprovedBy { get; set; }

        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public int? BranchCode { get; set; }
        [DataMember]
        public string InvoiceNo { get; set; }
        [DataMember]
        public string InvoiceDate { get; set; }

    }
}

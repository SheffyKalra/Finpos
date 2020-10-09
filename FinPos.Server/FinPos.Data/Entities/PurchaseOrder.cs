using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public PurchaseOrder()
        {

        }
        public PurchaseOrder(int? id, DateTime purchaseDate, int suplierCode, decimal? discountPercentage, decimal? discountAmount,
            DateTime? delivertDate, DateTime? expiryDate, decimal? surChargeAmount, decimal? taxPercentage, int createdBy, string createdDate,
            int status, DateTime? approvalDate, int? approvedBy,int companyId,int? branchId,string invoiceNo,string invoiceDate)
        {
            this.Id = id;
            this.PurchaseDate = purchaseDate;
            this.SuplierCode = suplierCode;
         //   this.PurchaseType = purchaseType;
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
            this.CompanyCode = companyId;
            this.BranchCode = branchId;
            this.InvoiceNo =invoiceNo;
            this.InvoiceDate =invoiceDate;
        }
        public DateTime PurchaseDate { get; set; }
        public int SuplierCode { get; set; }
      //  public int PurchaseType { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? SurChargeAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public int CreatedBy { get; set; }
        //  public decimal DiscountAmount { get; set; }
        //  public int TaxPercentage { get; set; }
        public int Status { get; set; }

        public DateTime? ApprovalDate { get; set; }


        public int? ApprovedBy { get; set; }


        public int CompanyCode { get; set; }
        public int? BranchCode { get; set; }
        public string InvoiceNo { get; set; }

        public string InvoiceDate { get; set; }
    }
}














   

using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class PaymentToSupplier : BaseEntity
    {
        public PaymentToSupplier() { }
        public PaymentToSupplier(int? id, int supplierCode, decimal amount, string paymentDate, string description, int invoiceNo, string accountNo, int createdBy, string createdDate, int? modifiedBy, string modifiedDate, int paymentType, string bankName, int companyCode, int? branchCode, int purchaseType)
        {
            Id = id;
            SupplierCode = supplierCode;
            Amount = amount;
            PaymentDate = paymentDate;
            Description = description;
            InvoiceNo = invoiceNo;
            AccountNo = accountNo;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            ModifiedBy = modifiedBy;
            ModifiedDate = modifiedDate;
            PaymentType = paymentType;
            BankName = bankName;
            CompanyCode = companyCode;
            BranchCode = branchCode;
            PurchaseType = purchaseType;
        }
        public int SupplierCode { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDate { get; set; }
        public string Description { get; set; }
        public int InvoiceNo { get; set; }
        public string AccountNo { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public int PaymentType { get; set; }
        public string BankName { get; set; }
        public int CompanyCode { get; set; }
        public int? BranchCode { get; set; }

        public int PurchaseType { get; set; }

    }
}

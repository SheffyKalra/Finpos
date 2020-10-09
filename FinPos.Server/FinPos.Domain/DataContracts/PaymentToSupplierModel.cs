using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class PaymentToSupplierModel
    {
        public PaymentToSupplierModel(int? id, int supplierCode, decimal amount, string paymentDate, string description, int invoiceNo, string accountNo, int createdBy, string createdDate, int? modifiedBy, string modifiedDate, int paymentType, string bankName, int companyCode, int? branchCode, string supplierName, string paymentTypeName, int purchaseType)
        {
            PaymentTosupplierId = id.Value;
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
            SupplierName = supplierName;
            PaymentTypeName = paymentTypeName;
            PurchaseType = purchaseType;
        }
        [DataMember]
        public int PaymentTosupplierId { get; set; }
        [DataMember]
        public int SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string PaymentDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
        [DataMember]
        public int PaymentType { get; set; }
        [DataMember]
        public string PaymentTypeName { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public int? BranchCode { get; set; }
        [DataMember]
        public int PurchaseType { get; set; }
    }
}

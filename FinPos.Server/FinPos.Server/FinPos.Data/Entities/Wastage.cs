using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Wastage : BaseEntity
    {
        public Wastage()
        {

        }
        public Wastage(int? id, int itemCode,string productName, int quantity, string reason, string createdDate, string modifiedDate, int modifiedBy, int createdBy,string batchNo,int? branchCode,int companyCode)
        {
            this.Id = id.Value;
            this.ItemCode = itemCode;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.Reason = reason;
            this.CreatedDate = createdDate;
            this.ModifiedDate = modifiedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;
            this.BatchNo = batchNo;
            this.BranchCode = branchCode;
            this.CompanyCode = companyCode;
        }
        public int ItemCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public string BatchNo { get; set; }
        public int? BranchCode { get; set; }
        public int CompanyCode { get; set; }
    }
}

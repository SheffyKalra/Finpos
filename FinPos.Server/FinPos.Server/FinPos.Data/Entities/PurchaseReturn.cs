using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class PurchaseReturn : BaseEntity
    {
        public PurchaseReturn() { }
        public PurchaseReturn(int? id, int purchaseId,long productCode, int quantity, int returnBy,string batchNo, DateTime returnDate,string createdDate,string reason)
        {
            this.Id = id;
            this.PurchaseId = purchaseId;
            this.ProductCode = productCode;
            this.Quantity = quantity;
            this.ReturnBy = returnBy;
            this.CreatedDate = createdDate;
            this.BatchNo = batchNo;
            this.ReturnDate = returnDate;
            this.Reason = reason;
        }
        public int PurchaseId { get; set; }

        public long ProductCode { get; set; }

        public int Quantity { get; set; }

        public int ReturnBy { get; set; }

        public string BatchNo { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Reason { get; set; }
    }
}

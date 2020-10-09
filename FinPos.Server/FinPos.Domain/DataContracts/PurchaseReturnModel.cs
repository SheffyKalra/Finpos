using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class PurchaseReturnModel
    {
        public PurchaseReturnModel(int? id, int purchaseId,long productCode,string batchNo, int quantity, int returnBy, DateTime returnDate,string reason)
        {
            this.PurchaseReturnId = id;
            this.PurchaseId = purchaseId;
            this.ProductCode = productCode;
            this.BatchNo = batchNo;
            this.Quantity = quantity;
            this.ReturnBy = returnBy;
            this.ReturnDate = returnDate;
            this.Reason = reason;
        }
        [DataMember]
        public int? PurchaseReturnId { get; set; }
        [DataMember]
        public int PurchaseId { get; set; }
        [DataMember]

        public long ProductCode { get; set; }

        [DataMember]
        public string BatchNo { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int ReturnBy { get; set; }
        [DataMember]
        public DateTime ReturnDate { get; set; }

        [DataMember]

        public string Reason { get; set; }
    }
}

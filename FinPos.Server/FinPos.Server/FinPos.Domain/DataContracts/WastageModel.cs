using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class WastageModel
    {
        public WastageModel(int? id, int itemCode,string productName, int quantity, string date, string reason,string batchNo,int? branchCode,int companyCode)
        {
            this.WastageId = id.Value;
            this.ItemCode = itemCode;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.Date = date;
            this.Reason = reason;
            this.BatchNo = batchNo;
            this.BranchCode = branchCode;
            this.CompanyCode = companyCode;
        }

        [DataMember]
        public int WastageId { get; set; }

        [DataMember]
        public int ItemCode { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]

        public string BatchNo { get; set; }
        [DataMember]
        public int? BranchCode { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }

    }
}

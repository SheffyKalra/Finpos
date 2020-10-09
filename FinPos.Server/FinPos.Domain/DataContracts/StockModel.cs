using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class StockModel
    {
        public StockModel(int? id, int? purchaseId, int quantity, decimal costPrice, decimal sellingPrice, decimal mrp, decimal? itemTaxPercentage, string batchNo,long productCode,int? purchaseOrderId)
        {
            this.StockId = id;
            this.PurchaseId = purchaseId;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.SellingPrice = sellingPrice;
            this.MRP = mrp;
            this.ItemTaxPercentage = itemTaxPercentage;
            this.BatchNo = batchNo;
            this.ProductCode = productCode;
            this.PurchaseOrderId = purchaseOrderId;
        }

        [DataMember]
        public int? StockId { get; set; }
        [DataMember]
        public int? PurchaseId { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal CostPrice { get; set; }
        [DataMember]
        public decimal SellingPrice { get; set; }
        [DataMember]
        public decimal MRP { get; set; }
        [DataMember]
        public decimal? ItemTaxPercentage { get; set; }
        [DataMember]
        public string BatchNo { get; set; }

        [DataMember]
        public long ProductCode { get; set; }

        [DataMember]
        public int? PurchaseOrderId { get; set; }
    }
}

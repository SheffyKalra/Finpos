using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Stock : BaseEntity
    {
        public Stock()
        {

        }
        public Stock(int? id, int? purchaseId, int quantity, decimal costPrice, decimal sellingPrice, decimal mrp, decimal? itemTaxPercentage, string batchNo, long productCode, string createdDate, int? purchaseOrderId)
        {
            this.Id = id;
            this.PurchaseId = purchaseId;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.SellingPrice = sellingPrice;
            this.MRP = mrp;
            this.ItemTaxPercentage = itemTaxPercentage;
            this.BatchNo = batchNo;
            this.ProductCode = productCode;
            this.CreatedDate = createdDate;
            this.PurchaseOrderId = purchaseOrderId;
        }
        public int? PurchaseId { get; set; }

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal MRP { get; set; }

        public decimal? ItemTaxPercentage { get; set; }

        public string BatchNo { get; set; }

        public long ProductCode { get; set; }

        public int? PurchaseOrderId { get; set; }
    }
}

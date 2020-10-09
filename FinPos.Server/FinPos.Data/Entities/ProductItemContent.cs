using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class ProductItemContent : BaseEntity
    {
        public ProductItemContent()
        {

        }
        public ProductItemContent(int? id, int childProductId, int quantity, int parentProductId, int createdBy, int? modifiedBy, string createdDate, string modifiedDate, bool isFreeProduct)
        {
            Id = id;
            SubProductId = childProductId;
            Quantity = quantity;
            MainProductId = parentProductId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedDate = modifiedDate;
            CreatedDate = createdDate;
            IsFreeProduct = isFreeProduct;
        }

        public int SubProductId { get; set; }
        public int Quantity { get; set; }
        public int MainProductId { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsFreeProduct { get; set; }
    }
}

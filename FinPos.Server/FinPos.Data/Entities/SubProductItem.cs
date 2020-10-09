using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class SubProductItem : BaseEntity
    {
        public SubProductItem() { }

        public SubProductItem(int?id,int parentProductId,int quantity,decimal retail,int createdBy,int? modifiedBy,string createdDate,string modifiedDate)
        {
            Id = id.Value;
            ParentProductId = parentProductId;
            Quantity = quantity;
            RetailPrice = retail;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }
        public int ParentProductId { get; set; }
        public int Quantity { get; set; }
        public decimal RetailPrice { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}

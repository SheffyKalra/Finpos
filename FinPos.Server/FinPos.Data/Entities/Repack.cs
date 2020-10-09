using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Repack : BaseEntity
    {
        public Repack() { }
        public Repack(int? id, int bulkCode, int productCode, int quantity, int? createdBy, string modifiedDate, int? modifiedBy, string createdDate)
        {
            Id = id.Value;
            BulkCode = bulkCode;
            ProductCode = productCode;
            Quantity = quantity;
            CreatedBy = createdBy;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            CreatedDate = createdDate;
        }
        //// public int RepackCode { get; set; }
        public int BulkCode { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public int? CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

    }
}

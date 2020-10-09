using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
  public  class RepackModel
    {
        public RepackModel(int? id, int bulkCode, int productCode, int quantity, int? createdBy, string modifiedDate, int? modifiedBy, string createdDate)
        {
            RepackCode = id.Value;
            BulkCode = bulkCode;
            ProductCode = productCode;
            Quantity = quantity;
            CreatedBy = createdBy;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            CreatedDate = createdDate;
        }
        [DataMember]
         public int RepackCode { get; set; }
        [DataMember]
        public int BulkCode { get; set; }
        [DataMember]
        public int ProductCode { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}

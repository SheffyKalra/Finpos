using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public  class SubProductItemModel
    {
        public SubProductItemModel(int? id, int parentProductId, int quantity, decimal retail, int createdBy, int? modifiedBy, string createdDate, string modifiedDate)
        {
            Id = id.Value;
            ParentProductId = parentProductId;
            Quantity = quantity;
            Retail = retail;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ParentProductId { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal Retail { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
    }
}

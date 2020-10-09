using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class ProductItemContentModel
    {
        public ProductItemContentModel(int? id, int childProductId, int quantity, int parentProductId, int createdBy, int? modifiedBy, string createdDate, string modifiedDate, bool isFreeProduct)
        {
            Id = id;
            ChildProductId = childProductId;
            Quantity = quantity;
            ParentProductId = parentProductId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedDate = modifiedDate;
            CreatedDate = createdDate;
            IsFreeProduct = isFreeProduct;
        }
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int ChildProductId { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int ParentProductId { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
        [DataMember]
        public bool IsFreeProduct { get; set; }
    }
}

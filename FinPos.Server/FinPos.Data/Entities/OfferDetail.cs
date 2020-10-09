using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    [DataContract]
    public class OfferDetail : BaseEntity
    {
        public OfferDetail()
        {

        }
        public OfferDetail(int? id, int offerCode, int? productCode, decimal discount, string fromDate, string toDate, string discountType)
        {
            this.Id = id;
            this.OfferCode = offerCode;
            this.ProductCode = productCode;
            this.Discount = discount;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.DiscountType = discountType;
        }

        [DataMember]
        public int OfferCode { get; set; }
        [DataMember]
        public int? ProductCode { get; set; }
        [DataMember]
        public decimal Discount { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string DiscountType { get; set; }
    }
}

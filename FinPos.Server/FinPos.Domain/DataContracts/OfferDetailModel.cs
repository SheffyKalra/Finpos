using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class OfferdetailModel
    {
        public OfferdetailModel()
        {

        }
        public OfferdetailModel(int? id, int offerCode, int? productCode, decimal discount, string fromDate, string toDate,string discountType,string productName)
        {
            this.Id = id;
            this.OfferId = offerCode;
            this.ProductCode = productCode;
            this.Discount = discount;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.DiscountType = discountType;
            this.ProductName = productName;
        }
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int OfferId { get; set; }
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
        [DataMember]
        public string ProductName { get; set; }

    }
}
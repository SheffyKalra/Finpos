using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class CouponDetailsModel
    {
        public CouponDetailsModel(int? id, int couponCode, string couponNo, bool isRedeem,string redeemDate)
        {
            this.Id = id;
            this.CouponCode = couponCode;
            this.CouponNo = couponNo;
            this.IsRedeem = isRedeem;
            this.RedeemDate = redeemDate;
        }
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int CouponCode { get; set; }
        [DataMember]
        public string CouponNo { get; set; }
        [DataMember]
        public bool IsRedeem { get; set; }
        [DataMember]
        public string RedeemDate { get; set; }

    }
}

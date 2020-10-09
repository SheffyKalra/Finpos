using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class CouponDetail : BaseEntity
    {
        public CouponDetail()
        {

        }
        public int CouponCode { get; set; }
        public string CouponNo { get; set; }
        public bool IsRedeem { get; set; }
        public string RedeemDate { get; set; }
    }
}

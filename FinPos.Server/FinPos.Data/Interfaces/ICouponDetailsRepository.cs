using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPos.Data.Entities;

namespace FinPos.Data.Interfaces
{
    public interface ICouponDetailsRepository
    {
        bool SaveCoupons(List<CouponDetail> listCouponDetails);
        List<CouponDetail> GetCouponDetails(int couponId);
        List<OfferDetail> GetItemSpecificOfferDetails(int? id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPos.Data.Entities;
using FinPos.DomainContracts.DataContracts;

namespace FinPos.Data.Interfaces
{
    public interface ICouponManagmentRepository
    {

        int SaveCouponDetails(Coupon objCoupon);
        List<Coupon> GetCoupons(int companyId, int? branchId);
        int SaveOfferDetails(Offer objOffer);
        List<Offer> GetOffers(int companyId, int? branchId);
        void SaveItemSpecificOfferDetails(List<OfferDetail> lstItemSpecificCoupons);
        OfferDetail GetOfferDetail(int? id);
    }
}

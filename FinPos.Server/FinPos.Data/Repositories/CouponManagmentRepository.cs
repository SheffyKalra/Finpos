using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    /// <summary>
    /// Coupon and Offer Managment Repository
    /// </summary>
    public class CouponManagmentRepository : ICouponManagmentRepository
    {
        private readonly IEntityProvider<Coupon> _CouponProvider;
        private readonly IEntityProvider<Offer> _OfferProvider;
        private readonly IEntityProvider<OfferDetail> _OfferDetailProvider;
        public CouponManagmentRepository(IEntityProvider<Coupon> couponProvider, IEntityProvider<Offer> offerProvider, IEntityProvider<OfferDetail> offerDetailProvider)
        {
            this._CouponProvider = couponProvider;
            this._OfferProvider = offerProvider;
            this._OfferDetailProvider = offerDetailProvider;
        }

        public int SaveCouponDetails(Coupon objCoupon)
        {
            try
            {
                int value = 0;
                if (objCoupon.Id > 0)
                {
                    this._CouponProvider.Update(objCoupon);
                }
                else
                {
                    value = this._CouponProvider.Insert(objCoupon);
                }
                return value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<Coupon> GetCoupons(int companyId, int? branchId)
        {
            return this._CouponProvider.Get().Where(coupon => coupon.CompanyCode == companyId && coupon.BranchCode == branchId).ToList();
        }
        public List<Offer> GetOffers(int companyId, int? branchId)
        {
            return this._OfferProvider.Get().Where(coupon => coupon.CompanyCode == companyId && coupon.BranchCode == branchId).ToList();
        }
        public int SaveOfferDetails(Offer objOffer)
        {
            if (objOffer.Id > 0)
                return this._OfferProvider.Update(objOffer);
            else
                return this._OfferProvider.Insert(objOffer);
        }
        public void SaveItemSpecificOfferDetails(List<OfferDetail> lstItemSpecificCoupons)
        {
            try
            {
                if (lstItemSpecificCoupons.Count > 0 && lstItemSpecificCoupons.FirstOrDefault().Id > 0)
                    this._OfferDetailProvider.UpdateAll(lstItemSpecificCoupons);
                else
                    this._OfferDetailProvider.InsertAll(lstItemSpecificCoupons);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public OfferDetail GetOfferDetail(int? id)
        {
            return _OfferDetailProvider.Get().Where(item => item.Id == id).FirstOrDefault();
        }
    }
}

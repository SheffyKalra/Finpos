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
    public class CouponDetailRepository : ICouponDetailsRepository
    {
        private readonly IEntityProvider<CouponDetail> _couponDetailProvider;
        private readonly IEntityProvider<OfferDetail> _offerDetailProvider;
        public CouponDetailRepository(IEntityProvider<CouponDetail> couponDetailProvider, IEntityProvider<OfferDetail> offerDetailProvides)
        {
            this._couponDetailProvider = couponDetailProvider;
            this._offerDetailProvider = offerDetailProvides;
        }
        public bool SaveCoupons(List<CouponDetail> listCouponDetails)
        {
            try
            {
                this._couponDetailProvider.InsertAll(listCouponDetails);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<CouponDetail> GetCouponDetails(int couponId)
        {
            return this._couponDetailProvider.Get().Where(item => item.CouponCode == couponId).ToList();
        }

        public List<OfferDetail> GetItemSpecificOfferDetails(int? id)
        {
            try
            {
                return this._offerDetailProvider.Get().Where(item => item.OfferCode == id).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

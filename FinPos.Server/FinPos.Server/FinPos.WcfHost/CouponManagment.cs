using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost
{
    [ServiceBehavior(Name = "CouponManagment", Namespace = "http://locahost:8080/CouponManagment", InstanceContextMode = InstanceContextMode.Single)]
    public class CouponManagment : ICouponManagment
    {
        private readonly ICouponManagmentRepository _couponManagmentRepository;
        private readonly ICouponDetailsRepository _couponDetailsRepository;
        FaultData fault = new FaultData();
        public CouponManagment(ICouponManagmentRepository couponManagmentRepository, ICouponDetailsRepository couponDetailsRepository)
        {
            _couponManagmentRepository = couponManagmentRepository;
            _couponDetailsRepository = couponDetailsRepository;
        }
        #region CouponManagment
        /// <summary>
        /// This method is used to Insert update Coupon Details for Coupon Management : Created by Nishant
        /// </summary>
        /// <param name="couponDetails">it will consist of bulk coupon details with the list of no of coupons created Insert and 
        /// Update will happern on two tables Coupon and Coupon Detail</param>
        /// <returns>Returns the bool as Saved as true and not saved as false</returns>
        public bool SaveCoupon(CouponModel couponDetails)
        {
            bool isSaved = false;
            try
            {
                if (couponDetails.Id > 0)
                {
                    var obj = _couponManagmentRepository.GetCoupons(couponDetails.CompanyCode, couponDetails.BranchCode).FirstOrDefault(x => x.Id == couponDetails.Id);
                    SetCouponValuesBeforeSave(couponDetails, obj);
                    _couponManagmentRepository.SaveCouponDetails(obj);
                    isSaved = true;
                    ///Code for EDIT COUPONS
                }
                else
                {
                    ///Code for Add Coupon 
                    Coupon objCoupon = new Coupon();
                    SetCouponValuesBeforeSave(couponDetails, objCoupon);
                    int CouponId = _couponManagmentRepository.SaveCouponDetails(objCoupon);
                    isSaved = SaveCouponDetails(couponDetails, CouponId);
                }
                return isSaved;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveCouponDetails method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        /// <summary>
        /// This method is used to save coupon details on the basis of coupon Id in CouponDetail Table
        /// </summary>
        /// <param name="couponDetails"></param>
        /// <param name="CouponId"></param>
        /// <returns></returns>
        private bool SaveCouponDetails(CouponModel couponDetails, int CouponId)
        {
            bool isSaved;
            List<CouponDetail> listCouponDetails = new List<CouponDetail>();
            CouponDetail objCouponDetails;
            foreach (CouponDetailsModel items in couponDetails.CouponDetails)
            {
                ///Updating Coupon Id to all new Coupons
                objCouponDetails = new CouponDetail();
                objCouponDetails.CouponCode = CouponId;
                objCouponDetails.CouponNo = items.CouponNo;
                objCouponDetails.IsRedeem = items.IsRedeem;
                objCouponDetails.RedeemDate = items.RedeemDate;
                listCouponDetails.Add(objCouponDetails);
            }
            ///Insett in Coupon Detail Table
            isSaved = _couponDetailsRepository.SaveCoupons(listCouponDetails);
            return isSaved;
        }

        /// <summary>
        /// this method is used to set coupon object to save the details in coupon table
        /// </summary>
        /// <param name="couponDetails">Source</param>
        /// <param name="objCoupon">object to save</param>
        private static void SetCouponValuesBeforeSave(CouponModel couponDetails, Coupon objCoupon)
        {
            if (couponDetails.Id > 0)
            {
                objCoupon.Id = couponDetails.Id;
            }
            objCoupon.BranchCode = couponDetails.BranchCode;
            objCoupon.CompanyCode = couponDetails.CompanyCode;
            objCoupon.CreatedBy = couponDetails.CreatedBy;
            objCoupon.CreatedDate = couponDetails.CreatedDate;
            objCoupon.Name = couponDetails.Name;
            objCoupon.DiscountType = Convert.ToInt32(couponDetails.DiscountType);
            objCoupon.FromDate = couponDetails.FromDate;
            objCoupon.IsActive = couponDetails.IsActive;
            objCoupon.IsDelete = couponDetails.IsDeleted;
            objCoupon.ModifiedBy = null;
            objCoupon.NoOfCoupons = couponDetails.NoOfCoupons;
            objCoupon.ToDate = couponDetails.ToDate;
            objCoupon.ModifiedDate = couponDetails.UpdatedDate;
            objCoupon.CValue = couponDetails.Value;
        }

        /// <summary>
        /// This Method is used to get list of generated coupons
        /// </summary>
        /// <returns> list of generated coupons with no of coupons details</returns>
        public IList<CouponModel> GetCoupons(int companyId, int? branchId)
        {
            try
            {
                IList<CouponModel> lstCoupons = new List<CouponModel>();
                List<Coupon> coupons = _couponManagmentRepository.GetCoupons(companyId, branchId);
                coupons.ToList().ForEach(x =>
                {
                    lstCoupons.Add(new CouponModel(x.Id, Convert.ToString(x.DiscountType), x.FromDate, x.ToDate, x.NoOfCoupons,
                        x.CValue, x.CompanyCode, x.BranchCode, x.Name, x.IsActive, x.IsDelete, x.CreatedBy, x.CreatedDate));
                });
                return lstCoupons;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetWastage method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public IList<CouponDetailsModel> GetCouponDetails(int couponId)
        {
            IList<CouponDetailsModel> lstCoupons = new List<CouponDetailsModel>();
            List<CouponDetail> couponsDetails = _couponDetailsRepository.GetCouponDetails(couponId);
            couponsDetails.ToList().ForEach(x =>
            {
                lstCoupons.Add(new CouponDetailsModel(x.Id, x.CouponCode, x.CouponNo, x.IsRedeem, x.RedeemDate));
            });
            return lstCoupons;
        }
        #endregion
        #region Offer Managment
        public IList<OfferModel> GetOffers(int companyId, int? branchId)
        {

            try
            {
                IList<OfferModel> lstOffers = new List<OfferModel>();
                List<Offer> offers = _couponManagmentRepository.GetOffers(companyId, branchId);
                offers.ToList().ForEach(x =>
                {
                    lstOffers.Add(new OfferModel(x.Id, Convert.ToString(x.OfferType), x.FromDate, x.ToDate, x.Discount,
                        x.MinimumValue, x.CompanyCode, x.BranchCode, x.Name, x.IsActive, x.IsDelete, x.CreatedBy, x.CreatedDate));
                });
                return lstOffers;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetWastage method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }


        /// <summary>
        /// This Method is used for Saving and Updation Offer Details
        /// </summary>
        /// <param name="objSave"></param>
        /// <returns></returns>
        public bool SaveOfferDetails(OfferModel objSave)
        {
            bool isSaved = false;
            try
            {
                if (objSave.Id > 0)
                {
                    var obj = _couponManagmentRepository.GetOffers(objSave.CompanyCode, objSave.BranchCode).FirstOrDefault(x => x.Id == objSave.Id);
                    Offer objOffer = new Offer();
                    SetOfferValuesBeforeSave(objSave, obj);
                    _couponManagmentRepository.SaveOfferDetails(obj);
                    isSaved = true;
                }
                else
                {

                    Offer objOffer = new Offer();
                    SetOfferValuesBeforeSave(objSave, objOffer);
                    _couponManagmentRepository.SaveOfferDetails(objOffer);
                    isSaved = true;
                }
                #region Commented Code
                //isSaved = SaveCouponDetails(couponDetails, CouponId);
                //if (objSave.Id > 0)
                //{
                //    Offer objOffer = new Offer();
                //    SetOfferValuesBeforeSave(objSave, objOffer);
                //    _couponManagmentRepository.SaveOfferDetails(objOffer);
                //    isSaved = true;
                //    ///Code for EDIT Offer
                //}
                //else
                //{
                //    ///Code for Add Offer 
                //    Offer objOffer = new Offer();
                //    SetOfferValuesBeforeSave(objSave, objOffer);
                //    _couponManagmentRepository.SaveOfferDetails(objOffer);
                //    //isSaved = SaveCouponDetails(couponDetails, CouponId);
                //}
                #endregion
                return isSaved;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveOfferDetails method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        /// <summary>
        /// this method is used to set coupon object to save the details in coupon table
        /// </summary>
        /// <param name="OfferDetails">Source</param>
        /// <param name="objCoupon">object to save</param>
        private static void SetOfferValuesBeforeSave(OfferModel offerDetails, Offer objOffer)
        {
            if (offerDetails.Id > 0)
            {
                objOffer.Id = offerDetails.Id;
            }
            objOffer.BranchCode = offerDetails.BranchCode;
            objOffer.CompanyCode = offerDetails.CompanyCode;
            objOffer.CreatedBy = offerDetails.CreatedBy;
            objOffer.CreatedDate = offerDetails.CreatedDate;
            objOffer.Name = offerDetails.Name;
            objOffer.OfferType = Convert.ToInt32(offerDetails.OfferType);
            objOffer.FromDate = offerDetails.FromDate;
            objOffer.IsActive = offerDetails.IsActive;
            objOffer.IsDelete = offerDetails.IsDeleted;
            objOffer.ModifiedBy = null;
            objOffer.Discount = offerDetails.Discount;
            objOffer.ToDate = offerDetails.ToDate;
            objOffer.ModifiedDate = offerDetails.UpdatedDate;
            objOffer.MinimumValue = offerDetails.MinimumValue;
        }
        #endregion
    }
}

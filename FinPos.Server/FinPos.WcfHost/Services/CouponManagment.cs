using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FinPos.WcfHost.Interface;
using FinPos.DAL.Interfaces;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "CouponManagmentService", Namespace = "http://locahost:8080/CouponManagmentService", InstanceContextMode = InstanceContextMode.Single)]
    public class CouponManagmentService : ICouponManagmentService
    {
        private readonly ICouponManagmentRepository _couponManagmentRepository;
        private readonly ICouponDetailsRepository _couponDetailsRepository;
        private readonly IProductRepository _productRepository;
        FaultData fault = new FaultData();
        public CouponManagmentService(ICouponManagmentRepository couponManagmentRepository, ICouponDetailsRepository couponDetailsRepository, IProductRepository productRepository)
        {
            _couponManagmentRepository = couponManagmentRepository;
            _couponDetailsRepository = couponDetailsRepository;
            _productRepository = productRepository;
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
                    var coupons = _couponManagmentRepository.GetCoupons(couponDetails.CompanyCode, couponDetails.BranchCode).FirstOrDefault(x => x.Id == couponDetails.Id);
                    SetCouponValuesBeforeSave(couponDetails, coupons);
                    _couponManagmentRepository.SaveCouponDetails(coupons);
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
            couponsDetails.ForEach(x =>
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
                offers.ForEach(x =>
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
        public bool SaveOfferDetails(OfferModel offerDetails)
        {
            bool isSaved = false;
            try
            {
                if (offerDetails.Id > 0)
                {
                    var offer = _couponManagmentRepository.GetOffers(offerDetails.CompanyCode, offerDetails.BranchCode).FirstOrDefault(x => x.Id == offerDetails.Id);
                    Offer objOffer = new Offer();
                    SetOfferValuesBeforeSave(offerDetails, offer);
                    _couponManagmentRepository.SaveOfferDetails(offer);
                    isSaved = true;
                }
                else
                {

                    Offer objOffer = new Offer();
                    SetOfferValuesBeforeSave(offerDetails, objOffer);
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
        private void SetOfferValuesBeforeSave(OfferModel offerDetails, Offer objOffer)
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

        public bool SaveItemSpecificOfferDetails(OfferModel offerDetails)
        {
            try
            {
                if (offerDetails.Id > 0)
                {
                    var offer = _couponManagmentRepository.GetOffers(offerDetails.CompanyCode, offerDetails.BranchCode).FirstOrDefault(x => x.Id == offerDetails.Id);
                    Offer objOffer = new Offer();
                    List<OfferDetail> lstItemSpecificCoupons = new List<OfferDetail>();
                    SetOfferValuesBeforeSave(offerDetails, offer);
                    int OfferId = _couponManagmentRepository.SaveOfferDetails(offer);
                    //OfferDetail objOfferDetail = new OfferDetail();
                    //SetItemSpecificOfferDetails(offerDetails, objOfferDetail, OfferId, ref lstItemSpecificCoupons);
                    //_couponManagmentRepository.SaveItemSpecificOfferDetails(lstItemSpecificCoupons);
                    List<OfferDetail> updateOfferDetail = new List<OfferDetail>();
                    offerDetails.ItemSpecficOfferDetails?.ForEach(x =>
                    {
                        if (x.Id > 0)
                        {
                            OfferDetail offerDetail = _couponManagmentRepository.GetOfferDetail(x.Id);
                            offerDetail.ProductCode = x.ProductCode;
                            offerDetail.Discount = x.Discount; offerDetail.FromDate = x.FromDate; offerDetail.ToDate = x.ToDate; offerDetail.DiscountType = x.DiscountType;
                            updateOfferDetail.Add(offerDetail);
                        }
                    });
                    _couponManagmentRepository.SaveItemSpecificOfferDetails(updateOfferDetail);
                }
                else
                {
                    Offer objOffer = new Offer();
                    SetOfferValuesBeforeSave(offerDetails, objOffer);
                    List<OfferDetail> lstItemSpecificCoupons = new List<OfferDetail>();
                    OfferDetail objOfferDetail = new OfferDetail();
                    int OfferId = _couponManagmentRepository.SaveOfferDetails(objOffer);
                    SetItemSpecificOfferDetails(offerDetails, objOfferDetail, OfferId, ref lstItemSpecificCoupons);
                    _couponManagmentRepository.SaveItemSpecificOfferDetails(lstItemSpecificCoupons);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetItemSpecificOfferDetails(OfferModel offerDetails, OfferDetail objOfferDetail, int OfferId, ref List<OfferDetail> lstItemSpecificCoupons)
        {
            foreach (OfferdetailModel item in offerDetails.ItemSpecficOfferDetails)
            {
                objOfferDetail = new OfferDetail();
                if (item.Id > 0)
                {
                    objOfferDetail.Id = item.Id;
                }
                objOfferDetail.CreatedDate = offerDetails.CreatedDate;
                objOfferDetail.Discount = item.Discount;
                objOfferDetail.DiscountType = item.DiscountType;
                objOfferDetail.FromDate = item.FromDate;
                objOfferDetail.OfferCode = OfferId;
                objOfferDetail.ProductCode = item.ProductCode;
                objOfferDetail.ToDate = item.ToDate;
                lstItemSpecificCoupons.Add(objOfferDetail);
            }
        }

        public IList<OfferdetailModel> GetItemSpecificOfferDetails(int? id, int companyId, int? branchId)
        {
            IList<OfferdetailModel> lstOffer = new List<OfferdetailModel>();
            List<Product> products = _productRepository.GetProductByComponyIdAndBranchId(companyId, branchId);
            List<OfferDetail> offers = _couponDetailsRepository.GetItemSpecificOfferDetails(id);
            offers.ForEach(x =>
            {
                lstOffer.Add(new OfferdetailModel(x.Id, x.OfferCode, x.ProductCode, x.Discount, x.FromDate, x.ToDate, x.DiscountType, x.ProductCode != null ? products.ToList().FirstOrDefault(z => z.Id == x.ProductCode).ItemName : string.Empty));
            });
            return lstOffer;

        }
        #endregion
    }
}

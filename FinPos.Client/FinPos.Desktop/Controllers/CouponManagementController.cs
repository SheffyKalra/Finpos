using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class CouponManagementController
    {
        IServiceEndpoints objService = new ServiceEndPoints.ServiceEndPoints();

        #region CRUD Operation
        public bool SaveCouponDetails(CouponModel couponDetails)
        {
            bool result = false;
            try
            {
                return result = objService.CouponManagmentServiceInstance().SaveCoupon(couponDetails);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }

        public bool SaveOfferDetails(OfferModel offerDetails)
        {
            try
            {
                return objService.CouponManagmentServiceInstance().SaveOfferDetails(offerDetails);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }

        public bool SaveItemSpecificOfferDetails(OfferModel offerDetails)
        {
            try
            {
                return objService.CouponManagmentServiceInstance().SaveItemSpecificOfferDetails(offerDetails);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }
        #endregion

        #region Getter Methods
        public ResponseVm GetCoupons()
        {
            try
            {
                IList<CouponModel> lstCoupons = objService.CouponManagmentServiceInstance().GetCoupons(UserModelVm.CompanyId, UserModelVm.BranchId).Select(x =>
                   new CouponModel(x.Id, Enum.GetName(typeof(FinPos.Utility.CommonEnums.CommonEnum.DiscountType), Convert.ToInt32(x.DiscountType)), x.FromDate, x.ToDate, x.NoOfCoupons,
                      x.Value, x.CompanyCode, x.BranchCode, x.Name, x.IsActive, x.IsDeleted, x.CreatedBy, x.CreatedDate)).ToList();
                return new ResponseVm(null, new List<object>(lstCoupons));
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }

        public IList<CouponDetailsModel> GetCouponDetails(int couponId)
        {       
            try
            {                              
                return objService.CouponManagmentServiceInstance().GetCouponDetails(couponId);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }

        public ResponseVm GetOffers()
        {
          
            try
            {              
               
                IList<OfferModel> lstOffers = objService.CouponManagmentServiceInstance().GetOffers(UserModelVm.CompanyId, UserModelVm.BranchId).Select(x =>
                    new OfferModel(x.Id, FinPos.Utility.CommonMethods.CommonFunctions.GetDescriptionFromEnumValue(
                        (Utility.CommonEnums.CommonEnum.OfferType)Enum.Parse(typeof(Utility.CommonEnums.CommonEnum.OfferType), x.OfferType)), x.FromDate, x.ToDate, x.Discount,
                       x.MinimumValue, x.CompanyCode, x.BranchCode, x.Name, x.IsActive, x.IsDeleted, x.CreatedBy, x.CreatedDate)).ToList();
                return new ResponseVm(null, new List<object>(lstOffers));
            }
            //.ToList()  Remove Tolist() responce
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }

       

        public IList<OfferdetailModel> GetItemSpecificOfferDetails(int? id)
        { 
            try
            {              
                return objService.CouponManagmentServiceInstance().GetItemSpecificOfferDetails(id, UserModelVm.CompanyId, UserModelVm.BranchId);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objService.CouponManegmentServiceInstanceClosed();
            }
        }
        #endregion
    }
}

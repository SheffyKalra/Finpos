using FinPos.DomainContracts.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;
namespace FinPos.WcfHost.Interface
{
    
    [ServiceContract]
    public interface ICouponManagmentService
    {
        [OperationContract]
        bool SaveCoupon(CouponModel couponDetails);
        //[OperationContract]
        //bool SaveCouponDetails(CouponModel couponDetails, int CouponId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<CouponModel> GetCoupons(int companyId, int? branchId);
        [OperationContract]
        IList<CouponDetailsModel> GetCouponDetails(int couponId);
        [OperationContract]
        IList<OfferModel> GetOffers(int companyId, int? branchId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool SaveOfferDetails(OfferModel offerDetails);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool SaveItemSpecificOfferDetails(OfferModel offerDetails);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<OfferdetailModel> GetItemSpecificOfferDetails(int? id, int companyId, int? branchId);
    }
}

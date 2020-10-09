using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost
{
    //[ServiceContract(Name = "CouponManagment", Namespace = "http://locahost:8080/")]
    [ServiceContract(Name = "CouponManagment", Namespace = "http://locahost:8080/CouponManagment")]
    public interface ICouponManagment
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
    }
}

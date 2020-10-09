using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Interface
{
    [ServiceContract]
    public interface IPurchaseService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int SaveUpdatePurchase(PurchaseOrderModel model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int SaveUpdateDirectPurchase(PurchaseModel model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveUpdateStocks(List<StockModel> model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseOrderModel> GetPurchase();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseOrderModel> GetPurchaseByCompanyAndBranchId(int companyId, int? branchId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseOrderModel> GetPurchaseBySupplierId(int companyId, int? branchId, int supplierId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<StockModelVM> GetPurchaseById(int productId, int year);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseModel> GetDirectPurchase();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseModel> GetDirectPurchaseByCompanyAndBranchId(int companyId, int? branchId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseModel> GetDirectPurchaseBySupplierId(int companyId, int? branchId, int supplierId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeletePurchase(int purchaseId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool UpdateStatus(PurchaseOrderModel purchaseId, int purchaseStatus);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<StockModel> GetStocksByPurchaseId(int purchaseId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<StockModel> GetDirectStocks(int purchaseId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeleteDirectPurchase(int purchaseId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<PurchaseReturnModel> GetPurchaseReturns(int purchaseId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool SaveUpdatePurchaseReturns(List<PurchaseReturnModel> purchaseReturnModel);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdateStocks(List<StockModel> stocks, List<StockModel> deleteStocks);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdatePurchaseRetrun(List<PurchaseReturnModel> purchaseRetrun);
    }
}

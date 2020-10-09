using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface IPurchaseRepository
    {
        bool IsProductExist(int id);
        int SaveUpdatepurchase(PurchaseOrder model);

        int SaveUpdateDirectPurchase(Purchase model);

        List<PurchaseOrder> GetPurchases();
        List<PurchaseOrder> GetPurchaseByCompanyAndBranchId(int companyId, int? branchId);
        List<PurchaseOrder> GetPurchaseBySupplierId(int companyId, int? branchId,int supplierId);
        List<Stock> GetPurchasesById(int productId, int year);

        List<Purchase> GetDirectPurchases();
        List<Purchase> GetDirectPurchasesByCompanyAndBranchId(int companyId, int? branchId);
        List<Purchase> GetDirectPurchasesBySupplierId(int companyId, int? branchId,int supplierId);
        void SaveUpdateStocks(List<Stock> stocks);

        void DeletePurchase(PurchaseOrder purchase);

        bool UpdateStatus(PurchaseOrder purchase);
        List<Stock> GetStocks();
        List<Stock> GetStocksById(int purchaseId);
        List<Stock> GetStocksByDirectPurchaseId(int directPurchaseId);
        List<PurchaseReturn> GetPurchaseReturns();
        bool SaveUpdatePurchaseReturns(List<PurchaseReturn> purchaseReturns);

        void UpdateStocks(List<Stock> updateStocks, List<Stock> insertStocks, List<Stock> deleteStocks);
        void UpdatePurchaseRetrun(List<PurchaseReturn> updatePurchaseReturn, List<PurchaseReturn> insertPurchaseReturn);
        void DeleteDirectPurchase(int purchaseId);
      
    }
}

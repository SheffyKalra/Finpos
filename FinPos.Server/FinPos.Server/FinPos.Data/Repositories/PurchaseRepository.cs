using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IEntityProvider<PurchaseOrder> _purchaseProvider;
        private readonly IEntityProvider<Purchase> _directPurchaseProvider;
        private readonly IEntityProvider<Stock> _stockProvider;
        private readonly IEntityProvider<PurchaseReturn> _purchaseReturnProvider;
        private readonly IEntityProvider<OpeningStock> _OpeningStock;
        public PurchaseRepository(IEntityProvider<PurchaseOrder> purchaseProvider, IEntityProvider<Stock> stockProvider, IEntityProvider<PurchaseReturn> purchaseReturnProvider, IEntityProvider<Purchase> directPurchaseProvider, IEntityProvider<OpeningStock> openingstock)
        {
            this._purchaseProvider = purchaseProvider;
            this._stockProvider = stockProvider;
            this._purchaseReturnProvider = purchaseReturnProvider;
            this._directPurchaseProvider = directPurchaseProvider;
            this._OpeningStock = openingstock;
        }
        /// <summary>
        /// check the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsProductExist(int id)
        {
            bool isExist = false;
            if (this._stockProvider.Any(x => x.ProductCode == id) || this._OpeningStock.Any(x => x.ProductCode == id))
            {
                isExist = true;
            }
            return isExist;
        }

        public int SaveUpdatepurchase(PurchaseOrder model)
        {
            int purchaseId = 0;
            if (model.Id > 0)
                purchaseId = this._purchaseProvider.Update(model);
            else
            {
                purchaseId = this._purchaseProvider.Insert(model);
            }
            return purchaseId;
        }

        public int SaveUpdateDirectPurchase(Purchase model)
        {
            int purchaseId = 0;
            if (model.Id > 0)
                purchaseId = this._directPurchaseProvider.Update(model);
            else
            {
                purchaseId = this._directPurchaseProvider.Insert(model);
            }
            return purchaseId;
        }

        public void SaveUpdateStocks(List<Stock> stocks)
        {

            this._stockProvider.InsertAll(stocks);

        }

        public bool SaveUpdatePurchaseReturns(List<PurchaseReturn> purchaseReturns)
        {
            if (purchaseReturns.FirstOrDefault().Id > 0)
                this._purchaseReturnProvider.UpdateAll(purchaseReturns);
            else
            {
                this._purchaseReturnProvider.InsertAll(purchaseReturns);
            }
            return true;
        }
        public void UpdateStocks(List<Stock> updateStocks, List<Stock> insertStocks, List<Stock> deleteStocks)
        {
            if (updateStocks.Any())
            {
                updateStocks.ForEach(x =>
                {
                    Stock obj;
                    obj = this._stockProvider.GetSingle(z => z.Id == x.Id);
                    obj.Id = obj.Id;
                    obj.PurchaseId = obj.PurchaseId;
                    obj.Quantity = x.Quantity;
                    obj.CostPrice = x.CostPrice;
                    obj.SellingPrice = x.SellingPrice;
                    obj.MRP = x.MRP;
                    obj.ItemTaxPercentage = x.ItemTaxPercentage;
                    obj.BatchNo = x.BatchNo;
                    obj.ProductCode = obj.ProductCode;
                    obj.CreatedDate = obj.CreatedDate;
                    this._stockProvider.Update(obj);
                });
            }
            // this._stockProvider.UpdateAll(updateStocks);
            if (deleteStocks.Any())
            {
                deleteStocks.ForEach(z =>
                {
                    var stock = this._stockProvider.GetSingle(x => x.Id == z.Id);
                    this._stockProvider.Delete(stock.Id);
                });
            }
            // if (deleteStocks.Any())
            //  this._stockProvider.DeleteAll(insertStocks);
            if (insertStocks.Any())
                this._stockProvider.InsertAll(insertStocks);
        }

        public void UpdatePurchaseRetrun(List<PurchaseReturn> updatePurchaseReturn, List<PurchaseReturn> insertPurchaseReturn)
        {
            if (updatePurchaseReturn.Any())
            {
                updatePurchaseReturn.ForEach(x =>
                {
                    PurchaseReturn obj;
                    obj = this._purchaseReturnProvider.GetSingle(z => z.Id == x.Id);
                    obj.Id = obj.Id;
                    obj.PurchaseId = obj.PurchaseId;
                    obj.ProductCode = obj.ProductCode;
                    obj.Quantity = x.Quantity;
                    obj.ReturnBy = x.ReturnBy;
                    obj.CreatedDate = obj.CreatedDate;
                    obj.BatchNo = x.BatchNo;
                    obj.ReturnDate = x.ReturnDate;
                    this._purchaseReturnProvider.Update(obj);
                });
            }
            // this._stockProvider.UpdateAll(updateStocks);
            // if (deleteStocks.Any())
            //  this._stockProvider.DeleteAll(insertStocks);
            if (insertPurchaseReturn.Any())
                this._purchaseReturnProvider.InsertAll(insertPurchaseReturn);
        }

        public void DeletePurchase(PurchaseOrder purchase)
        {
            this._stockProvider.Delete(_stockProvider.Get().ToList().Where(item => item.PurchaseOrderId == purchase.Id).FirstOrDefault());
            this._purchaseProvider.Delete(purchase);
        }

        public void DeleteDirectPurchase(int purchaseId)
        {
            this._stockProvider.Delete(_stockProvider.Get().ToList().Where(item => item.PurchaseId == purchaseId).FirstOrDefault());
            this._directPurchaseProvider.Delete(this._directPurchaseProvider.Get().ToList().Where(item => item.Id == purchaseId).FirstOrDefault());
        }
        public bool UpdateStatus(PurchaseOrder purchase)
        {
            this._purchaseProvider.Update(purchase);
            return true;
        }
        public List<PurchaseOrder> GetPurchases()
        {
            return _purchaseProvider.Get().ToList();
        }
        public List<PurchaseOrder> GetPurchaseByCompanyAndBranchId(int companyId, int? branchId)
        {
            return _purchaseProvider.Get().Where(x => x.CompanyCode == companyId && x.BranchCode == branchId).ToList();
        }
        public List<PurchaseOrder> GetPurchaseBySupplierId(int companyId, int? branchId, int supplierId)
        {
            return _purchaseProvider.Get().Where(x => x.CompanyCode == companyId && x.BranchCode == branchId && x.SuplierCode == supplierId).ToList();
        }
        //Convert.ToDateTime(x.CreatedDate) >= Convert.ToDateTime(Settings.FinalYearStartDate) && Convert.ToDateTime(x.CreatedDate) <= Convert.ToDateTime(Settings.FinalYearEndDate) &&
        public List<Stock> GetPurchasesById(int productId, int year)
        {
            return _stockProvider.Get().ToArray().Where(x => x.ProductCode == productId && CommonFunctions.ParseDateToFinclave(x.CreatedDate).Year == year).ToList();
        }
        public List<Purchase> GetDirectPurchases()
        {
            return _directPurchaseProvider.Get().ToList();
        }
        public List<Purchase> GetDirectPurchasesByCompanyAndBranchId(int companyId, int? branchId)
        {
            return _directPurchaseProvider.Get().Where(x => x.SuplierCode != null && x.CompanyCode == companyId && x.BranchCode == branchId).ToList();
        }
        public List<Purchase> GetDirectPurchasesBySupplierId(int companyId, int? branchId, int supplierId)
        {
            return _directPurchaseProvider.Get().Where(x => x.SuplierCode != null && x.CompanyCode == companyId && x.BranchCode == branchId && x.SuplierCode == supplierId).ToList();
        }
        //&& Convert.ToDateTime(x.CreatedDate) >= Convert.ToDateTime(Settings.FinalYearStartDate) && Convert.ToDateTime(x.CreatedDate) <= Convert.ToDateTime(Settings.FinalYearEndDate) &&
        public List<Stock> GetStocks()
        {
            return _stockProvider.Get().ToList();
        }
        public List<Stock> GetStocksById(int purchaseId)
        {
            return _stockProvider.Get().Where(x => x.PurchaseOrderId == purchaseId).ToList();
        }
        public List<Stock> GetStocksByDirectPurchaseId(int purchaseId)
        {
            return _stockProvider.Get().Where(x => x.PurchaseId == purchaseId).ToList();
        }
        public List<PurchaseReturn> GetPurchaseReturns()
        {
            return _purchaseReturnProvider.Get().ToList();
        }

    }
}

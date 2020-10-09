using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "OpeningStockService", Namespace = "http://locahost:8080/OpeningStockService", InstanceContextMode = InstanceContextMode.Single)]
    public class OpeningStockService :IOpeningStockService
    {
        #region Properties
        private readonly IOpeningStockRepository _openingStockRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;

        #endregion

        #region Constructor
        public OpeningStockService(IOpeningStockRepository openingStockRepository, IPurchaseRepository purchaseRepository, IProductRepository productRepository, IStockAdjustmentRepository stockAdjustmentRepository)
        {
            this._openingStockRepository = openingStockRepository;
            this._purchaseRepository = purchaseRepository;
            this._productRepository = productRepository;
            this._stockAdjustmentRepository = stockAdjustmentRepository;
        }
        #endregion

        #region CRUD Oeprations
        public void SaveOpeningStocks(List<OpeningStockModel> model)
        {
            List<OpeningStock> openingStock = new List<OpeningStock>();
            openingStock = model.Select(x => new OpeningStock()
            {
                ProductCode = x.ProductCode,
                ExpiryDate = x.ExpiryDate,
                BatchNo = x.BatchNo,
                CompanyCode = x.CompanyCode,
                Branchcode = x.BranchCode,
                Quantity = x.Quantity,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate
            }).ToList();
            _openingStockRepository.SaveOpeningStocks(openingStock);
        }

        #endregion
       
        #region Getter Methods
        public int GetCurrentStockByProductAndBatchCode(long? productCode, string batchNo)
        {
            int currentStock = 0;
            int totalStock = 0;
            if (string.IsNullOrEmpty(batchNo))
            {
                List<Purchase> _directpurchases = _purchaseRepository.GetDirectPurchases().ToList();
                int?[] _directpurchasesIDs = _directpurchases.Select(x => x.Id).ToArray();
                int?[] _purchaseFromSupplier = _purchaseRepository.GetPurchases().Where(x => x.Status != 2).Select(x => x.Id).ToArray(); // temprary code(int)CommonEnum.PurchaseStatus.WaitingForApproval
                List<OpeningStock> _openingStock = _openingStockRepository.GetOpeningStockByProductCode().Where(x => x.ProductCode == productCode).ToList();
                List<Wastage> _wastage = _productRepository.GetWastage().Where(x => x.ItemCode == productCode).ToList();
                List<Stock> _stock = _purchaseRepository.GetStocks().Where(x => (x.ProductCode == productCode && x.PurchaseId != null && _directpurchasesIDs.Contains(x.PurchaseId)) || ((x.ProductCode == productCode && _purchaseFromSupplier.Contains(x.PurchaseOrderId)))).ToList();
                List<StockAdjustment> _stockAdjustment = _stockAdjustmentRepository.GetStockAdjustments().Where(x => x.ProductCode == productCode).ToList();
                currentStock = CommonFunctions.CalcCurrentStock(_stock == null ? 0 : _stock.Sum(x => x.Quantity), _openingStock == null ? 0 : _openingStock.Sum(x => x.Quantity), _wastage == null ? 0 : _wastage.Sum(x => x.Quantity), _stockAdjustment == null ? 0 : _stockAdjustment.Sum(x => x.Quantity));
            }
            else
            {
                List<OpeningStock> _openingStock = _openingStockRepository.GetOpeningStockByProductCode().Where(x => x.BatchNo == batchNo && x.ProductCode == productCode).ToList();
                List<Stock> _stock = _purchaseRepository.GetStocks().Where(x => x.BatchNo == batchNo && x.ProductCode == productCode).ToList();
                List<StockAdjustment> _stockAdjustment = _stockAdjustmentRepository.GetStockAdjustments().Where(x => x.BatchNo == batchNo && x.ProductCode == productCode).ToList();
                //Batch not included
                List<Wastage> _wastage = _productRepository.GetWastage().Where(x => x.ItemCode == productCode).ToList();
                currentStock = CommonFunctions.CalcCurrentStock(_stock == null ? 0 : _stock.Sum(x => x.Quantity), _openingStock == null ? 0 : _openingStock.Sum(x => x.Quantity), _wastage == null ? 0 : _stock.Sum(x => x.Quantity), _stockAdjustment == null ? 0 : _stockAdjustment.Sum(x => x.Quantity));
            }
            List<PurchaseReturn> stocksRetrun = _purchaseRepository.GetPurchaseReturns().Where(x => (x.ProductCode == productCode)).ToList();
            totalStock = currentStock - (stocksRetrun == null ? 0 : stocksRetrun.Sum(x => x.Quantity));
            return totalStock;
        }
        public bool CheckProductOpningStock(long productCode)
        {
            bool exists = false;
            List<Purchase> _directpurchases = _purchaseRepository.GetDirectPurchases().ToList();
            int?[] _directpurchasesIDs = _directpurchases.Select(x => x.Id).ToArray();
            int?[] _purchaseFromSupplier = _purchaseRepository.GetPurchases().Where(x => x.Status != 2).Select(x => x.Id).ToArray(); /*temprary code 2 in this line (int)CommonEnum.PurchaseStatus.WaitingForApproval*/
            List<OpeningStock> _openingStock = _openingStockRepository.GetOpeningStockByProductCode().Where(x => x.ProductCode == productCode).ToList();
            List<Stock> _stock = _purchaseRepository.GetStocks().Where(x => (x.ProductCode == productCode && x.PurchaseId != null && _directpurchasesIDs.Contains(x.PurchaseId)) || ((x.ProductCode == productCode && _purchaseFromSupplier.Contains(x.PurchaseOrderId)))).ToList();
            if (_openingStock.Count > 0 && _stock.Count > 0)
            {
                exists = true;
            }
            return exists;
        }
        #endregion
    }
}

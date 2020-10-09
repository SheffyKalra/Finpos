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
    [ServiceBehavior(Name = "StockAdjustmentService", Namespace = "http://locahost:8080/StockAdjustmentService", InstanceContextMode = InstanceContextMode.Single)]
    public class StockAdjustmentService : IStockAdjustmentService
    {
        #region Properties
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IOpeningStockRepository _openingStockRepository;
        private readonly IProductRepository _productRepository;
        FaultData fault = new FaultData();

        #endregion

        #region Constructor
        public StockAdjustmentService(IStockAdjustmentRepository stockAdjustmentRepository, IPurchaseRepository purchaseRepository, IOpeningStockRepository openingStockRepository, IProductRepository productRepository)
        {
            this._stockAdjustmentRepository = stockAdjustmentRepository;
            this._purchaseRepository = purchaseRepository;
            this._openingStockRepository = openingStockRepository;
            this._productRepository = productRepository;
        }
        #endregion

        #region Getter Methods
        public List<StockModel> GetAllStocks()
        {
            try
            {


                List<Stock> stocks = _purchaseRepository.GetStocks().ToList();
                return stocks.Select(x => new StockModel(x.Id, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, x.PurchaseOrderId)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetAllStocks method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool CheckStockByProductCode(long productCode)
        {
            try
            {
                List<Purchase> _directpurchases = _purchaseRepository.GetDirectPurchases().ToList();
                int?[] _directpurchasesIDs = _directpurchases.Select(x => x.Id).ToArray();
                bool isStockExists = false;
                int?[] _purchaseFromSupplier = _purchaseRepository.GetPurchases().Where(x => x.Status != 2).Select(x => x.Id).ToArray();/* Temprary code 2 in this line (int)CommonEnum.PurchaseStatus.WaitingForApproval*/
                List<OpeningStock> _openingStock = _openingStockRepository.GetOpeningStockByProductCode().Where(x => x.ProductCode == productCode).ToList();
                List<Wastage> _wastage = _productRepository.GetWastage().Where(x => x.ItemCode == productCode).ToList();
                List<Stock> _stock = _purchaseRepository.GetStocks().Where(x => (x.ProductCode == productCode && x.PurchaseId != null && _directpurchasesIDs.Contains(x.PurchaseId)) || ((x.ProductCode == productCode && _purchaseFromSupplier.Contains(x.PurchaseOrderId)))).ToList();
                List<StockAdjustment> _stockAdjustment = _stockAdjustmentRepository.GetStockAdjustments().Where(x => x.ProductCode == productCode).ToList();
                if (_openingStock.Count > 0 || _stock.Count > 0 || _wastage.Count > 0 || _stockAdjustment.Count > 0)
                {
                    isStockExists = true;
                }
                return isStockExists;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in CheckStockByProductCode method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion

        #region CRUD Operations
        public void SaveStockAdjustment(List<StockAdjustmentModel> model)
        {
            try
            {
                List<StockAdjustment> stockAdjustment = new List<StockAdjustment>();
                stockAdjustment = model.Select(x => new StockAdjustment()
                {
                    ProductCode = x.productCode,
                    ExpiryDate = x.ExpiryDate,
                    BatchNo = x.BatchNo,
                    Reason = x.Reason,
                    CompanyCode = x.CompanyCode,
                    Branchcode = x.BranchCode,
                    Quantity = x.Quantity,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate
                }).ToList();
                _stockAdjustmentRepository.SaveStockAdjustment(stockAdjustment);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveStockAdjustment method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        #endregion
    }
}

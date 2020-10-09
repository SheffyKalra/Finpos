using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "PurchaseService", Namespace = "http://locahost:8080/PurchaseService", InstanceContextMode = InstanceContextMode.Single)]
    public class PurchaseService : IPurchaseService
    {
        #region Properties
        private readonly IPurchaseRepository _purchaseRepository;
        FaultData fault = new FaultData();
        private readonly ISupplierRepository _supplierRepository;
        #endregion

        #region Constructor
        public PurchaseService(IPurchaseRepository purchaseRepository, ISupplierRepository supplierRepository)
        {
            this._purchaseRepository = purchaseRepository;
            this._supplierRepository = supplierRepository;
        }
        #endregion

        #region CRUD Operations
        public int SaveUpdatePurchase(PurchaseOrderModel model)
        {
            try
            {
                PurchaseOrder purchase;
                if (model.PurchaseId > 0)
                {
                    purchase = _purchaseRepository.GetPurchases().FirstOrDefault(x => x.Id == model.PurchaseId);
                    purchase.Id = model.PurchaseId;
                    purchase.PurchaseDate = model.PurchaseDate;
                    purchase.SuplierCode = model.SuplierCode;
                    // purchase.PurchaseType = model.PurchaseType;
                    purchase.DiscountPercentage = model.DiscountPercentage;
                    purchase.DiscountAmount = model.DiscountAmount;
                    purchase.DeliveryDate = model.DeliveryDate;
                    purchase.ExpiryDate = model.ExpiryDate;
                    purchase.SurChargeAmount = model.SurChargeAmount;
                    purchase.TaxPercentage = model.TaxPercentage;
                    purchase.CreatedDate = model.CreatedDate;
                    purchase.CreatedBy = model.CreatedBy;
                    purchase.Status = model.Status;
                    purchase.ApprovalDate = model.ApprovalDate;
                    purchase.ApprovedBy = model.ApprovedBy;
                    purchase.CompanyCode = model.CompanyCode;
                    purchase.BranchCode = model.BranchCode;
                    purchase.InvoiceNo = model.InvoiceNo;
                    purchase.InvoiceDate = model.InvoiceDate;
                }
                else
                {
                    purchase = new PurchaseOrder(model.PurchaseId, model.PurchaseDate, model.SuplierCode, model.DiscountPercentage, model.DiscountAmount
                        , model.DeliveryDate, model.ExpiryDate, model.SurChargeAmount, model.TaxPercentage, model.CreatedBy, model.CreatedDate
                        , model.Status, model.ApprovalDate, model.ApprovedBy, model.CompanyCode, model.BranchCode, model.InvoiceNo, model.InvoiceDate);
                }
                return _purchaseRepository.SaveUpdatepurchase(purchase);

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Purchase";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public int SaveUpdateDirectPurchase(PurchaseModel model)
        {
            try
            {
                Purchase purchase;
                if (model.PurchaseId > 0)
                {
                    purchase = _purchaseRepository.GetDirectPurchases().FirstOrDefault(x => x.Id == model.PurchaseId);
                    purchase.Id = model.PurchaseId;
                    purchase.PurchaseDate = model.PurchaseDate;
                    purchase.SuplierCode = model.SuplierCode;
                    // purchase.PurchaseType = model.PurchaseType;
                    purchase.DiscountPercentage = model.DiscountPercentage;
                    purchase.DiscountAmount = model.DiscountAmount;
                    purchase.DeliveryDate = model.DeliveryDate;
                    purchase.ExpiryDate = model.ExpiryDate;
                    purchase.SurChargeAmount = model.SurChargeAmount;
                    purchase.TaxPercentage = model.TaxPercentage;
                    purchase.CreatedDate = model.CreatedDate;
                    purchase.CreatedBy = model.CreatedBy;
                    purchase.CompanyCode = model.CompanyCode;
                    purchase.BranchCode = model.BranchCode;
                }
                else
                {
                    purchase = new Purchase(model.PurchaseId, model.PurchaseDate, model.SuplierCode, model.DiscountPercentage, model.DiscountAmount
                        , model.DeliveryDate, model.ExpiryDate, model.SurChargeAmount, model.TaxPercentage, model.CreatedBy, model.CreatedDate
                        , model.CompanyCode, model.BranchCode);
                }
                return _purchaseRepository.SaveUpdateDirectPurchase(purchase);

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update DirectPurchase";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveUpdateStocks(List<StockModel> model)
        {
            try
            {
                if (model.Any())
                {
                    List<Stock> stocks = new List<Stock>();
                    stocks.AddRange(model?.Select(x =>
                    {
                        return new Stock(x.StockId, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.PurchaseOrderId);
                    }).ToList());
                    _purchaseRepository.SaveUpdateStocks(stocks);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Stocks";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool DeletePurchase(int purchaseId)
        {
            try
            {
                PurchaseOrder purchase = _purchaseRepository.GetPurchases().FirstOrDefault(x => x.Id == purchaseId);
                _purchaseRepository.DeletePurchase(purchase);
                return true;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeletePurchase method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
        public bool UpdateStatus(PurchaseOrderModel model, int purchaseStatus)
        {
            try
            {
                PurchaseOrder purchase;
                purchase = _purchaseRepository.GetPurchases().FirstOrDefault(x => x.Id == model.PurchaseId);
                purchase.Id = purchase.Id;
                purchase.PurchaseDate = purchase.PurchaseDate;
                purchase.SuplierCode = model.SuplierCode;
                //  purchase.PurchaseType = model.PurchaseType;
                purchase.DiscountPercentage = model.DiscountPercentage;
                purchase.DiscountAmount = model.DiscountAmount;
                purchase.DeliveryDate = model.DeliveryDate;
                purchase.ExpiryDate = model.ExpiryDate;
                purchase.SurChargeAmount = model.SurChargeAmount;
                purchase.TaxPercentage = model.TaxPercentage;
                purchase.CreatedDate = purchase.CreatedDate;
                purchase.CreatedBy = model.CreatedBy;
                purchase.Status = purchaseStatus;
                purchase.ApprovalDate = model.ApprovalDate;
                purchase.ApprovedBy = model.ApprovedBy;
                purchase.CompanyCode = purchase.CompanyCode;
                purchase.BranchCode = purchase.BranchCode;
                purchase.InvoiceNo = model.InvoiceNo;
                purchase.InvoiceDate = model.InvoiceDate;
                //Purchase newPurchase = new Purchase(purchase.Id, purchase.PurchaseDate, purchase.SuplierCode, purchase.PurchaseType, purchase.DiscountPercentage, purchase.DiscountAmount
                //    , purchase.ReferenceNo, purchase.DeliveryDate, purchase.ExpiryDate, purchase.SurChargeAmount, purchase.TaxPercentage, purchase.CreatedBy, purchase.CreatedDate, DateTime.Now, UserModelVm.UserId, purchaseStatus,
                //    purchase.ApprovalDate, purchase.POReturnDate, purchase.ApprovedBy, purchase.POReturnedBy, purchase.CompanyId, purchase.BranchId);
                _purchaseRepository.UpdateStatus(purchase);
                return true;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool DeleteDirectPurchase(int purchaseId)
        {
            try
            {
                _purchaseRepository.DeleteDirectPurchase(purchaseId);
                return true;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteDirectPurchase method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool SaveUpdatePurchaseReturns(List<PurchaseReturnModel> purchaseReturnModel)
        {
            List<PurchaseReturn> purchaseReturns = purchaseReturnModel.Select(x => new PurchaseReturn(x.PurchaseReturnId, x.PurchaseId, x.ProductCode, x.Quantity, x.ReturnBy, x.BatchNo, x.ReturnDate, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.Reason)).ToList();
            return _purchaseRepository.SaveUpdatePurchaseReturns(purchaseReturns);
        }
        public void UpdateStocks(List<StockModel> stocks, List<StockModel> deleteStocksModel)
        {
            List<Stock> updateStocks = new List<Stock>();
            List<Stock> insertStocks = new List<Stock>();
            List<Stock> deleteStocks = new List<Stock>();
            stocks?.ForEach(x =>
            {
                if (x.StockId > 0)
                {
                    Stock updateStock = new Stock(x.StockId, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.PurchaseOrderId);
                    updateStocks.Add(updateStock);
                }
                else
                {
                    Stock insertStock = new Stock(0, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.PurchaseOrderId);
                    insertStocks.Add(insertStock);
                }

            });
            deleteStocks.AddRange(deleteStocksModel?.Select(x => new Stock(x.StockId, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.PurchaseOrderId)));
            _purchaseRepository.UpdateStocks(updateStocks, insertStocks, deleteStocks);
        }
        public void UpdatePurchaseRetrun(List<PurchaseReturnModel> purchaseRetrun)
        {
            List<PurchaseReturn> updateStocks = new List<PurchaseReturn>();
            List<PurchaseReturn> insertStocks = new List<PurchaseReturn>();
            purchaseRetrun?.ForEach(x =>
            {
                PurchaseReturn updateStock = new PurchaseReturn(x.PurchaseReturnId > 0 ? x.PurchaseReturnId : 0, x.PurchaseId, x.ProductCode, x.Quantity, x.ReturnBy, x.BatchNo, x.ReturnDate, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.Reason);
                if (x.PurchaseReturnId > 0)
                    updateStocks.Add(updateStock);
                else
                    insertStocks.Add(updateStock);


            });
            _purchaseRepository.UpdatePurchaseRetrun(updateStocks, insertStocks);
        }
        #endregion

        #region Getter Methods
        public List<PurchaseReturnModel> GetPurchaseReturns(int purchaseId)
        {
            List<PurchaseReturn> stocks = _purchaseRepository.GetPurchaseReturns().Where(x => x.PurchaseId == purchaseId).ToList();
            return stocks?.Select(x => new PurchaseReturnModel(x.Id, x.PurchaseId, x.ProductCode, x.BatchNo, x.Quantity, x.ReturnBy, x.ReturnDate, x.Reason)).ToList();
        }
        public List<StockModel> GetStocksByPurchaseId(int purchaseId)
        {
            try
            {
                return _purchaseRepository.GetStocksById(purchaseId).Select(x => new StockModel(x.Id, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, x.PurchaseOrderId)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetStocks method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<StockModel> GetDirectStocks(int purchaseId)
        {
            List<Stock> stocks = _purchaseRepository.GetStocksByDirectPurchaseId(purchaseId);
            return stocks?.Select(x => new StockModel(x.Id, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, x.PurchaseOrderId)).ToList();
        }
        public List<PurchaseOrderModel> GetPurchase()
        {
            try
            {
                List<PurchaseOrderModel> lstNewPurchases = new List<PurchaseOrderModel>();
                List<PurchaseOrder> purchases = _purchaseRepository.GetPurchases().ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    var suplierName = _supplierRepository.GetSupplier().FirstOrDefault(z => z.Id == x.SuplierCode)?.SupplierName;
                    return new PurchaseOrderModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, suplierName ?? string.Empty
                    , x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPurchase method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<PurchaseOrderModel> GetPurchaseByCompanyAndBranchId(int companyId, int? branchId)
        {
            try
            {
                List<PurchaseOrderModel> lstNewPurchases = new List<PurchaseOrderModel>();
                List<PurchaseOrder> purchases = _purchaseRepository.GetPurchaseByCompanyAndBranchId(companyId, branchId).ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    var suplierName = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId).FirstOrDefault(z => z.Id == x.SuplierCode)?.SupplierName;
                    return new PurchaseOrderModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, suplierName ?? string.Empty
                    , x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<PurchaseOrderModel> GetPurchaseBySupplierId(int companyId, int? branchId, int supplierId)
        {
            try
            {
                List<PurchaseOrderModel> lstNewPurchases = new List<PurchaseOrderModel>();
                List<PurchaseOrder> purchases = _purchaseRepository.GetPurchaseBySupplierId(companyId, branchId, supplierId).ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    Supplier suplier = _supplierRepository.GetSuppliersBySupplierId(companyId, branchId, supplierId);
                    return new PurchaseOrderModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, suplier.SupplierName ?? string.Empty
                    , x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<StockModelVM> GetPurchaseById(int productId, int year)
        {
            try
            {
                List<Stock> stocks = _purchaseRepository.GetPurchasesById(productId, year);
                List<StockModelVM> stockmodelVm = new List<StockModelVM>();
                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int x = 1; x <= 12; x++)
                {
                    var stocksLst = stocks?.Where(k => CommonFunctions.ParseDateToFinclave(k.CreatedDate).Month == Convert.ToInt32((CommonEnums.MonuthStatus)x)).ToArray().Select(c => new StockModelVM(null, c.Quantity, c.Quantity * c.CostPrice, months[x - 1].ToString())).ToList();
                    //var sumOfStocks = stocksLst?;Convert.ToInt32((CommonEnum.MonuthStatus)x)
                    stockmodelVm.Add(new StockModelVM(x, stocksLst.Any() ? stocksLst.Sum(z => z.Quantity) : 0, stocksLst.Any() ? stocksLst.Sum(m => m.CostPrice) : 0, months[x - 1].ToString()));
                }
                return stockmodelVm;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPurchaseById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<PurchaseModel> GetDirectPurchase()
        {
            try
            {
                List<PurchaseModel> lstNewPurchases = new List<PurchaseModel>();
                List<Purchase> purchases = _purchaseRepository.GetDirectPurchases().ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    string suplierName = _supplierRepository.GetSupplier().FirstOrDefault(z => z.Id == x.SuplierCode)?.SupplierName;
                    return new PurchaseModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.CompanyCode, x.BranchCode, suplierName ?? string.Empty
                    );
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<PurchaseModel> GetDirectPurchaseByCompanyAndBranchId(int companyId, int? branchId)
        {
            try
            {
                List<PurchaseModel> lstNewPurchases = new List<PurchaseModel>();
                List<Purchase> purchases = _purchaseRepository.GetDirectPurchasesByCompanyAndBranchId(companyId, branchId).ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    string suplierName = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId).FirstOrDefault(z => z.Id == x.SuplierCode)?.SupplierName;
                    return new PurchaseModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.CompanyCode, x.BranchCode, suplierName ?? string.Empty
                    );
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetDirectPurchaseByCompanyAndBranchId method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<PurchaseModel> GetDirectPurchaseBySupplierId(int companyId, int? branchId, int supplierId)
        {
            try
            {
                List<PurchaseModel> lstNewPurchases = new List<PurchaseModel>();
                List<Purchase> purchases = _purchaseRepository.GetDirectPurchasesBySupplierId(companyId, branchId, supplierId).ToList();
                lstNewPurchases.AddRange(purchases.Select(x =>
                {
                    Supplier suplier = _supplierRepository.GetSuppliersBySupplierId(companyId, branchId, supplierId);
                    return new PurchaseModel(
                    x.Id,
                    x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                    x.CreatedBy, x.CreatedDate, x.CompanyCode, x.BranchCode, suplier.SupplierName ?? string.Empty
                    );
                }).ToList());
                return lstNewPurchases;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetDirectPurchaseBySupplierId method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion
    }
}

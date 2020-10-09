using FinPos.Client.ServiceEndPoints.Interface;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Controllers
{
    public class PurchaseController
    {
        IServiceEndpoints objPurchaseService = new ServiceEndPoints.ServiceEndPoints();
        public int SaveUpdatePurchase(PurchaseOrderModel model)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().SaveUpdatePurchase(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public int SaveUpdateDirectPurchase(PurchaseModel model)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().SaveUpdateDirectPurchase(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public void SaveUpdateStocks(List<StockModel> model)
        {
            try
            {
                objPurchaseService.PurchaseServiceInstance().SaveUpdateStocks(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public List<PurchaseOrderModel> GetPurchase()
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetPurchase().Select(x =>
                  {
                      return new PurchaseOrderModel(
                        x.PurchaseId,
                        x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                        x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, x.SuplierName, x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                  }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<PurchaseOrderModel> GetPurchaseByCompanyAndBranchId(int companyId, int? branchId)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetPurchaseByCompanyAndBranchId(companyId, branchId).Select(x =>
                    {
                        return new PurchaseOrderModel(
                          x.PurchaseId,
                          x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                          x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, x.SuplierName, x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public List<PurchaseOrderModel> GetPurchaseBySupplierId(int companyId, int? branchId, int supplierId)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetPurchaseBySupplierId(companyId, branchId, supplierId).Select(x =>
                  {
                      return new PurchaseOrderModel(
                        x.PurchaseId,
                        x.PurchaseDate, x.SuplierCode, x.DiscountPercentage, x.DiscountAmount, x.DeliveryDate, x.ExpiryDate, x.SurChargeAmount, x.TaxPercentage,
                        x.CreatedBy, x.CreatedDate, x.Status, x.ApprovalDate, x.ApprovedBy, x.CompanyCode, x.BranchCode, x.SuplierName, x.Status.ToString(), x.InvoiceNo, x.InvoiceDate);
                  }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<StockModelVM> GetPurchaseById(int productId, int year)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetPurchaseById(productId, year);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<PurchaseModel> GetDirectPurchase()
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetDirectPurchase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<PurchaseModel> GetDirectPurchaseByCompanyAndBranchId()
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetDirectPurchaseByCompanyAndBranchId(UserModelVm.CompanyId, UserModelVm.BranchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<PurchaseModel> GetDirectPurchaseBySupplierId(int companyId, int? branchId, int supplierId)
        {
            try
            {


                return objPurchaseService.PurchaseServiceInstance().GetDirectPurchaseBySupplierId(companyId, branchId, supplierId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public ResponseVm DeletePurchase(int purchaseId)
        {
            try
            {

                var result = objPurchaseService.PurchaseServiceInstance().DeletePurchase(purchaseId);

                return new ResponseVm(null, new List<object>(1));

            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public bool UpdateStatus(PurchaseOrderModel purchase, int purchaseStatus)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().UpdateStatus(purchase, purchaseStatus);
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public List<StockModel> GetStocksByPurchaseId(int purchaseId)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetStocksByPurchaseId(purchaseId);
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        //public List<StockModel> GetStocksById(int purchaseId,int companyId,int? branchId)
        //{
        //    ChannelFactory<IFinPosService> myChannelFactory = new ChannelFactory<IFinPosService>(tcpBindings, myEndpoint);
        //    IFinPosService instance = myChannelFactory.CreateChannel();
        //    List<StockModel> stocks = instance.GetStocksById(purchaseId,companyId,branchId);
        //    myChannelFactory.Close();
        //    return stocks;
        //}

        public List<StockModel> GetDirectStocks(int purchaseId)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetDirectStocks(purchaseId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public ResponseVm DeleteDirectPurchase(int purchaseId)
        {
            try
            {
                var result = objPurchaseService.PurchaseServiceInstance().DeleteDirectPurchase(purchaseId);
                return new ResponseVm(null, new List<object>(1));
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public List<PurchaseReturnModel> GetPurchaseReturns(int purchaseId)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().GetPurchaseReturns(purchaseId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public bool SaveUpdatePurchaseReturns(List<PurchaseReturnModel> model)
        {
            try
            {
                return objPurchaseService.PurchaseServiceInstance().SaveUpdatePurchaseReturns(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
        public void UpdateStocks(List<StockModel> model, List<StockModel> deleteStocks)
        {
            try
            {
                objPurchaseService.PurchaseServiceInstance().UpdateStocks(model, deleteStocks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }

        public void UpdatePurchaseRetrun(List<PurchaseReturnModel> model)
        {
            try
            {
                objPurchaseService.PurchaseServiceInstance().UpdatePurchaseRetrun(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objPurchaseService.PurchaseServiceInstanceClosed();
            }
        }
    }
}

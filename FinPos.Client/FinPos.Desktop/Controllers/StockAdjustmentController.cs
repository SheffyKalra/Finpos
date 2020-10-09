using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class StockAdjustmentController
    {

        IServiceEndpoints objStockAdjustmentService = new ServiceEndPoints.ServiceEndPoints();
        public ResponseVm SaveStockAdjustment(List<StockAdjustmentModel> stocks)
        {
            try
            {
                objStockAdjustmentService.StockAdjustmentServiceInstance().SaveStockAdjustment(stocks);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }finally
            {
                objStockAdjustmentService.StockAdjustmentServiceInstanceClosed();
            }
        }
        public int GetCurrentStockByProductCode(long productCode)
        {
            IServiceEndpoints objOpeningStockService = new ServiceEndPoints.ServiceEndPoints();
            try
            {

                return objOpeningStockService.OpeningStockServiceInstance().GetCurrentStockByProductAndBatchCode(productCode, null);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objOpeningStockService.OpeningStockServiceInstanceClosed();
            }
        }
        public int GetItemCurrentStockByBatchNo(string batchNo, long? productCode)
        {
            IServiceEndpoints objOpeningStockService = new ServiceEndPoints.ServiceEndPoints();
            try
            {
                return objOpeningStockService.OpeningStockServiceInstance().GetCurrentStockByProductAndBatchCode(productCode, batchNo);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objOpeningStockService.OpeningStockServiceInstanceClosed();
            }
        }
        public List<StockModel> GetStocks()
        {
            try
            {
                return objStockAdjustmentService.StockAdjustmentServiceInstance().GetAllStocks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objStockAdjustmentService.StockAdjustmentServiceInstanceClosed();
            }
        }

        public bool CheckStockByProductCode(long productCode)
        {
            try
            {
                return objStockAdjustmentService.StockAdjustmentServiceInstance().CheckStockByProductCode(productCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objStockAdjustmentService.StockAdjustmentServiceInstanceClosed();
            }
        }
    }
}

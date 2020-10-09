using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class OpeningStockController
    {
        IServiceEndpoints objOpeningStockService = new ServiceEndPoints.ServiceEndPoints();
        public ResponseVm SaveUpdateOpeningStocks(List<OpeningStockModel> stocks)
        {
            try
            {
                objOpeningStockService.OpeningStockServiceInstance().SaveOpeningStocks(stocks);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objOpeningStockService.OpeningStockServiceInstanceClosed();
            }
        }
        public int GetCurrentStockByProductCode(long productCode)
        {
            try
            {
                return objOpeningStockService.OpeningStockServiceInstance().GetCurrentStockByProductAndBatchCode(productCode, null);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objOpeningStockService.OpeningStockServiceInstanceClosed();
            }
        }

        public bool CheckProductOpningStock(long productCode)
        {
            try
            {
                return objOpeningStockService.OpeningStockServiceInstance().CheckProductOpningStock(productCode);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objOpeningStockService.OpeningStockServiceInstanceClosed();
            }
        }
    }
}

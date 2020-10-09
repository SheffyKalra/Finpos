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
    public interface IStockAdjustmentService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveStockAdjustment(List<StockAdjustmentModel> model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<StockModel> GetAllStocks();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool CheckStockByProductCode(long productCode);
    }
}

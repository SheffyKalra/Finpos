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
    public interface IOpeningStockService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveOpeningStocks(List<OpeningStockModel> model);
        [OperationContract]
        int GetCurrentStockByProductAndBatchCode(long? productCode, string batchNo);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool CheckProductOpningStock(long productCode);
    }
}

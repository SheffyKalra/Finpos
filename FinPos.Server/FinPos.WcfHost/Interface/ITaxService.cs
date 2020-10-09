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
    public interface ITaxService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<TaxModel> GetTax();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveUpdateTax(TaxModel model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeleteTax(int id);
    }
}

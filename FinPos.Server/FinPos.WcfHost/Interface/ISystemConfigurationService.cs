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
    public interface ISystemConfigurationService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<SystemConfigurationModel> GetSystemConfiguration();
    }
}

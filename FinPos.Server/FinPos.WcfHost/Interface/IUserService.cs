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
    public interface IUserService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserModel GetUser(string email, string password);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveUpdateUser(UserModel user);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    public class ResponseVm
    {
        public ResponseVm(FaultException<FaultData> faultData, List<dynamic> response)
        {
            this.FaultData = faultData;
            this.Response = response;
        }
        public FaultException<FaultData> FaultData { get; set; }
        public List<dynamic> Response { get; set; }
    }
}

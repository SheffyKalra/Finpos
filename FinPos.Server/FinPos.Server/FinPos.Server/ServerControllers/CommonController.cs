//using FinPos.Domain.ServiceContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Server.ServerControllers
{
  public  class CommonController
    {
        private NetTcpBinding tcpBindings = new NetTcpBinding();
        private EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:8090/FinPosManager/");
        public bool IsExist()
        {///commented due to unused code
        //    ChannelFactory<IFinPosService> myChannelFactory = new ChannelFactory<IFinPosService>(tcpBindings, myEndpoint);
        //    IFinPosService instance = myChannelFactory.CreateChannel();
        //   // bool isExist = instance.IsExist();
        //    myChannelFactory.Close();

            return false;
        }
    }
}

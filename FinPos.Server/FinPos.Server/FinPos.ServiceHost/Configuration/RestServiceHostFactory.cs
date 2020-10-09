using Ninject.Extensions.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WebHost.Configuration
{
    public class RestServiceHostFactory<TServiceContract> : NinjectServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = base.CreateServiceHost(serviceType, baseAddresses);
            var webBehavior = new WebHttpBehavior
            {
                AutomaticFormatSelectionEnabled = true,
                HelpEnabled = true,
                FaultExceptionEnabled = true
            };
            var endpoint = host.AddServiceEndpoint(typeof(TServiceContract), new WebHttpBinding(), "Rest");
            endpoint.Name = "rest";
            endpoint.Behaviors.Add(webBehavior);
            return host;
        }
    }
}

using Ninject.Extensions.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WebHost.Configuration
{
    public class NinjectServiceHelper<TServiceContract, TServiceType> : IDisposable
    {
        bool _disposed;

        public NinjectServiceHelper(IServiceBehavior serviceBehavior, string address, Binding binding)
        {
            // Create Ninject service host
            if (binding.GetType() == typeof(WebHttpBinding))
                _serviceHost = new NinjectWebServiceHost(serviceBehavior, typeof(TServiceType));
            else
                _serviceHost = new NinjectServiceHost(serviceBehavior, typeof(TServiceType));

            // Add endpoint
            _serviceHost.AddServiceEndpoint(typeof(TServiceContract),binding, address);

            // Add web behavior
            if (binding.GetType() == typeof(WebHttpBinding))
            {
                var webBehavior = new WebHttpBehavior
                {
                    AutomaticFormatSelectionEnabled = true,
                    HelpEnabled = true,
                    FaultExceptionEnabled = true
                };
                _serviceHost.Description.Endpoints[0].Behaviors.Add(webBehavior);
            }

            // Add service metadata
            var metadataBehavior = new ServiceMetadataBehavior();
            if (binding.GetType() == typeof(BasicHttpBinding))
            {
                metadataBehavior.HttpGetEnabled = true;
                metadataBehavior.HttpGetUrl = new Uri(address);
            }
            _serviceHost.Description.Behaviors.Add(metadataBehavior);

            // Open service host
            _serviceHost.Open();

            // Init client
            var factory = new ChannelFactory<TServiceContract>(binding);
            _client = factory.CreateChannel(new EndpointAddress(address));
        }

        private readonly ServiceHost _serviceHost;
        public ServiceHost ServiceHost
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException("NinjectServiceHelper");
                return _serviceHost;
            }
        }

        private readonly TServiceContract _client;
        public TServiceContract Client
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException("NinjectServiceHelper");
                return _client;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ((IDisposable)_serviceHost).Dispose();
                ((IDisposable)_client).Dispose();
                _disposed = true;
            }
        }
    }
}

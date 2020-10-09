
using FinPos.Server.ServiceEndPoints.Interface;
using System.ServiceModel;

namespace FinPos.Server.ServiceEndPoints
{
    public class ServiceEndPoints : IServiceEndPoints
    {
        #region EndPoint Properties
        public NetTcpBinding tcpBindings = new NetTcpBinding();
        private EndpointAddress companyManegmentEndPoint = new EndpointAddress((string)System.Windows.Application.Current.Resources["CompanyService"]);
        private EndpointAddress userEndPoint = new EndpointAddress((string)System.Windows.Application.Current.Resources["UserService"]);
        ChannelFactory<WcfHost.Interface.ICompanyService> _channelFactoryCompany;
        ChannelFactory<WcfHost.Interface.IUserService> _channelFactoryUser;
        #endregion

        #region Service Instances      
        public WcfHost.Interface.ICompanyService CompanyServiceInstance()
        {
            _channelFactoryCompany = new ChannelFactory<WcfHost.Interface.ICompanyService>(tcpBindings, companyManegmentEndPoint);
            WcfHost.Interface.ICompanyService instance;
            return instance = _channelFactoryCompany.CreateChannel();
        }
        public WcfHost.Interface.IUserService UserServiceInstance()
        {
            _channelFactoryUser = new ChannelFactory<WcfHost.Interface.IUserService>(tcpBindings, userEndPoint);
            WcfHost.Interface.IUserService instance;
            return instance = _channelFactoryUser.CreateChannel();
        }
        #endregion
        #region Instance Closed
        /// <summary>
        /// Close the instance for Company Factory
        /// </summary>
        public void CompanyServiceInstanceClosed()
        {
            _channelFactoryCompany.Close();
        }
        /// <summary>
        /// Close the instance for User Factory
        /// </summary>
        public void UserServiceInstanceClosed()
        {
            _channelFactoryUser.Close();
        }
        #endregion
    }
}

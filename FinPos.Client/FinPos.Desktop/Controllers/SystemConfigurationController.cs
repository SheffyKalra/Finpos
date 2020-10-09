using FinPos.Client.ServiceEndPoints.Interface;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Controllers
{
    public class SystemConfigurationController
    {
        IServiceEndpoints objSystemConfigurationService = new ServiceEndPoints.ServiceEndPoints();
        public IList<SystemConfigurationModel> GetSystemConfiguration()
        {
            try
            {
                return objSystemConfigurationService.SystemConfigurationServiceInstance().GetSystemConfiguration();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSystemConfigurationService.SystemConfigurationServiceInstanceClosed();
            }
        }
    }
}

using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "SystemConfigurationService", Namespace = "http://locahost:8080/SystemConfigurationService", InstanceContextMode = InstanceContextMode.Single)]
    public class SystemConfigurationService : ISystemConfigurationService
    {
        #region Properties
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        #endregion

        #region Constructor
        public SystemConfigurationService(ISystemConfigurationRepository systemConfigurationRepository)
        {
            this._systemConfigurationRepository = systemConfigurationRepository;
        }
        #endregion
        
        #region Getter Methods 
        public List<SystemConfigurationModel> GetSystemConfiguration()
        {
            List<SystemConfiguration> configuration = _systemConfigurationRepository.GetSystemConfiguration();
            return new List<SystemConfigurationModel>() { new SystemConfigurationModel(1, "7/01/2017", "7/01/2018", "7/01/2017") }; // configuration.Select(x => new SystemConfigurationModel(x.Id, x.FInalYearStartDate, x.FinalYearEndDate, x.CreatedDate)).ToList();
        }
        #endregion
    }
}

using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly IEntityProvider<SystemConfiguration> _systemConfigurationProvider;

        public SystemConfigurationRepository(IEntityProvider<SystemConfiguration> systemConfigurationProvider)
        {
            this._systemConfigurationProvider = systemConfigurationProvider;
        }
        public List<SystemConfiguration> GetSystemConfiguration()
        {
            return _systemConfigurationProvider.Get().ToList();
        }
    }
}

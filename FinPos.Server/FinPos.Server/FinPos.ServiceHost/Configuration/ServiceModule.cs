using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using FinPos.DAL.Providers;
using FinPos.DAL.Repositories;
using FinPos.Data;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Data.Repositories;
//using FinPos.Domain.ServiceContracts;
//using FinPos.WcfHost;
using FinPos.Service;
using MySql.Data.MySqlClient;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WebHost.Configuration
{
    public class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEntityProvider<CompanyData>>().To<MySQLProvider<CompanyData>>();
            Bind<IEntityProvider<BranchData>>().To<MySQLProvider<BranchData>>();
            Bind<IEntityProvider<UserData>>().To<MySQLProvider<UserData>>();
            Bind<IEntityProvider<Product>>().To<MySQLProvider<Product >>();
            Bind<IInventoryRepository>().To<InventoryRepository>();
            Bind<ICompanyRepository>().To<CompanyRepository>();
            Bind<IBranchRepository>().To<BranchRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IProductRepository>().To<ProductRepository>();
           // Bind<IFinPosService>().To<Service.FinPosService>();
           //Bind<IFinPosService>().To<FinPosService>();
        }
    }
}

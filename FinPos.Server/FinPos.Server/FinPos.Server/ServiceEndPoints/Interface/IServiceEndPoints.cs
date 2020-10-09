using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Server.ServiceEndPoints.Interface
{
    public interface IServiceEndPoints
    {
        WcfHost.Interface.ICompanyService CompanyServiceInstance();
        WcfHost.Interface.IUserService UserServiceInstance();
        void CompanyServiceInstanceClosed();
        void UserServiceInstanceClosed();
    }
}

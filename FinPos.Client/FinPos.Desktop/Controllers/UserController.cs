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
    class UserController
    {
        IServiceEndpoints objUserService = new ServiceEndPoints.ServiceEndPoints();
        public UserModel GetUser(string email, string password)
        {
            try
            {
                return objUserService.UserServiceInstance().GetUser(email, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objUserService.UserServiceInstanceClosed();
            }
        }
    }
}

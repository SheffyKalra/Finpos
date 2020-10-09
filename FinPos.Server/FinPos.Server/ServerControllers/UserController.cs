//using FinPos.Domain.ServiceContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.Server.ServiceEndPoints.Interface;
using FinPos.WcfHost;
using System.Collections.Generic;
using System.ServiceModel;

namespace FinPos.Server.ServerControllers
{
    public class UserController
    {
        #region Properties
        IServiceEndPoints objUserService = new ServiceEndPoints.ServiceEndPoints();
        #endregion
        public IList<UserModel> GetUsers()///Unuser Code
        {
            //IList<UserModel> lstCustomers;
            //ChannelFactory<IFinPosService> myChannelFactory = new ChannelFactory<IFinPosService>(tcpBindings, myEndpoint);
            //IFinPosService instance = myChannelFactory.CreateChannel();
            ////lstCustomers = instance.GetUsers();
            ////myChannelFactory.Close();

            return null;
        }

        public UserModel GetUser(string email, string password)
        {
            try
            {
                return objUserService.UserServiceInstance().GetUser(email, password);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }finally
            {
                objUserService.UserServiceInstanceClosed();
            }
        }

        public void SaveUpdateUser(UserModel model)
        {
            try
            {
                objUserService.UserServiceInstance().SaveUpdateUser(model);
            }
            catch (System.Exception ex)
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

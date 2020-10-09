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
    [ServiceBehavior(Name = "UserService", Namespace = "http://locahost:8080/UserService", InstanceContextMode = InstanceContextMode.Single)]
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUserRepository _userRepository;
        FaultData fault = new FaultData();
        #endregion
        #region Constructor
        public UserService(IUserRepository userRepository)
        {
            this._userRepository=userRepository;
        }
        #endregion
        #region Getter Methods
        public UserModel GetUser(string email, string password)
        {
            try
            {
                User user = _userRepository.GetUser(email, password);
                if (user != null)
                {
                    return new UserModel(user.Id, user.UserCode, user.CreatedDate, user.FirstName, user.LastName, user.IsAdmin, user.Email, user.Password, user.IsActive, user.ModifiedDate, user.ModifiedBy, user.CreatedBy, user.RoleId);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetUser method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
        #endregion
        #region CRUD Operations
        public void SaveUpdateUser(UserModel model)
        {
            User obj = new User(model.Id, model.UserCode, model.CreatedDate, model.FirstName, model.LastName, model.IsAdmin, model.Email, model.Password, model.IsActive, null, model.FirstName, model.CreatedBy, model.RoleId);
            _userRepository.SaveUpdateUser(obj);
        }
        #endregion
    }
}

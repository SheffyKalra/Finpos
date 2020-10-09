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
   public class UserRepository : IUserRepository
    {
        private readonly IEntityProvider<User> _userProvider;
        public UserRepository(IEntityProvider<User> userProvider)
        {
            this._userProvider = userProvider;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            users = this._userProvider.Get().ToList();
            return users;
        }
        //public int SaveCUstomer(UserData company)
        //{
        //    return 0;
        //    // this.customerProvider.Insert(new CompanyData(company.Name, company.Address));
        //}
        public void SaveUpdateUser(User user)
        {
            if (user.Id > 0)
                this._userProvider.Update(user);
            else
                this._userProvider.Insert(user);
        }

        public User GetUser(string email,string password)
        {
            return this._userProvider.Get().ToList().FirstOrDefault(x => x.Email == email && x.Password == password);
        }
    }
}

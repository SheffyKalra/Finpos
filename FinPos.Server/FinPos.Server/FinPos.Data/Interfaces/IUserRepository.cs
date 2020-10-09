using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        void SaveUpdateUser(User customer);

        User GetUser(string email, string password);
    }
}

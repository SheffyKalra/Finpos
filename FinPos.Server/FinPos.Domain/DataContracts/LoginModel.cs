using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPOS.DomainContracts.Model
{
    public class LoginModel
    {
        public LoginModel()
        {

        }
        public LoginModel(string email, string password, bool isRememberMe)
        {
            this.Email = email;
            this.Password = password;
            this.IsRememberMe = isRememberMe;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}

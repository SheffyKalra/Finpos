using FinPos.Utility.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinPos.Server.ServerModel
{
    class LoginModel : IDataErrorInfo
    {
        private bool _nameChanged = false;
        private string _userName;
        private string _password;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                _nameChanged = true;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                _nameChanged = true;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (_nameChanged && columnName == "UserName")
                {
                    if (string.IsNullOrEmpty(UserName))
                        result = "Please enter a user Name or Email";
                    if (!Regex.IsMatch(UserName, CommonConstants._emailValidation))
                        result = "Please enter the valid email";
                }
                if (_nameChanged && columnName == "Password")
                {
                    if (string.IsNullOrEmpty(Password))
                        result = "Please enter Password";
                }
                return result;
            }
        }
    }
}

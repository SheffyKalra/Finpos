using FinPos.Utility.Constants;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FinPos.Client.Model
{
    public class LoginModel : IDataErrorInfo
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
        public string Password {
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
                        result = "User Name or Email is required";
                    if (!Regex.IsMatch(UserName, CommonConstants._emailValidation))
                        result = "Please enter the valid email";
                }
                if (_nameChanged && columnName == "Password")
                {
                    if (string.IsNullOrEmpty(Password))
                        result = "Password is required";
                }
                return result;
            }
        }
    }
}

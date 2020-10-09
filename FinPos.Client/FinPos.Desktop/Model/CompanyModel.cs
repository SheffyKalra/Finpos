using System;
using System.ComponentModel;

namespace FinPos.Client.Model
{
    class CompanyModel : IDataErrorInfo

    {
        private bool _nameChanged = false;
        private string _companyName;
        private string _mobile;
        public string Name
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                _nameChanged = true;
            }
        }
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
                _nameChanged = true;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string result = null;
                if (_nameChanged && columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        result = "Company name is required";

                }
                if (_nameChanged && columnName == "Mobile")
                {
                    if (Mobile.Length < 10)
                        result = "Enter minimum 10 digits phone number";

                }
                return result;
            }
        }
    }
}
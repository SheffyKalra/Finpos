using System;
using System.ComponentModel;

namespace FinPos.Client.Model
{
    class LicenseModel: IDataErrorInfo

    {
        private bool _nameChanged = false;
        private string _licenseKey;
        public string LicenseKey
        {
            get { return _licenseKey; }
            set
            {
                _licenseKey = value;
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
                if (_nameChanged && columnName == "LicenseKey")
                {
                    if (string.IsNullOrEmpty(LicenseKey))
                        result = "LicenseKey is required";
                    
                }
                return result;
            }
        }
    }
}

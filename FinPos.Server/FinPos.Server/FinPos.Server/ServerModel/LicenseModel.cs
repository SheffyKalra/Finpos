using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Server.ServerModel
{
    class LicenseModel : IDataErrorInfo

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
                        result = "Please enter LicneseKey";

                }
                return result;
            }
        }
    }
}

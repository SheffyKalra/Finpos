using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Model
{
    public class TaxRequiredModel : IDataErrorInfo
    {
        private bool _nameChanged = false;
        private string _taxDetail;
        private string _taxRate;
        public string TaxDetail
        {
            get { return _taxDetail; }
            set
            {
                _taxDetail = value;
                _nameChanged = true;
            }
        }

        public string TaxRate
        {
            get { return _taxRate; }
            set
            {
                _taxRate = value;
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
                if (columnName == "TaxDetail")
                {
                    if (string.IsNullOrEmpty(TaxDetail))
                        result = "TaxDetail is required";

                }
                if (columnName == "TaxRate")
                {
                    if (string.IsNullOrEmpty(TaxRate))
                        result = "TaxRate is required";

                }
                return result;
            }
        }
    }
}


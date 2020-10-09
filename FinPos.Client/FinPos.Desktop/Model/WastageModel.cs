using System;
using System.ComponentModel;

namespace FinPos.Client.Model
{
    class WastageModel : IDataErrorInfo

    {
        private bool _nameChanged = false;
        private string _companyName;
        private string _code;
        private string _quantity;
        private string _date;
        private string _reason;
        public string Name
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                _nameChanged = true;
            }
        }
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                _nameChanged = true;
            }
        }
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                _nameChanged = true;
            }
        }

        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                _nameChanged = true;
            }
        }
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
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
                        result = "Product name is required";

                }
                if (_nameChanged && columnName == "Code")
                {
                    if (string.IsNullOrEmpty(Code))
                        result = "Product code is required";

                }
                if (_nameChanged && columnName == "Quantity")
                {
                    if (string.IsNullOrEmpty(Quantity))
                        result = "Quantity is required";

                }
                if (_nameChanged && columnName == "Date")
                {
                    if (string.IsNullOrEmpty(Date))
                        result = "Date is required";

                }
                if (_nameChanged && columnName == "Reason")
                {
                    if (string.IsNullOrEmpty(Reason))
                        result = "Reason is required";

                }
                return result;
            }
        }
    }
}

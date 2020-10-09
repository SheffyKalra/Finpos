using System;
using System.ComponentModel;

namespace FinPos.Client.Model
{
    class BranchModel : IDataErrorInfo

    {
        private bool _nameChanged = false;
        private string _branchyName;
        private string _address;
        public string Name
        {
            get { return _branchyName; }
            set
            {
                _branchyName = value;
                _nameChanged = true;
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
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
                if (_nameChanged && columnName == "Address")
                {
                    if (string.IsNullOrEmpty(Address))
                        result = "Address is required";

                }
                return result;
            }
        }
    }
}
  
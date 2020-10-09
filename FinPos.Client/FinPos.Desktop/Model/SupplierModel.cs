using FinPos.Utility.Constants;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FinPos.Client.Model
{
    class SupplierModel : IDataErrorInfo
    {
        private string _supplierTelephone;
        private bool _nameChanged = false;
        private string _supplierName;
        private string _supplierEmail;
        private string _supplierContactName;
        private string _supplierMobile;
        private string _supplierWebsiteUrl;
        private string _supplierAddress;
        // private string _address;
        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                _nameChanged = true;
            }
        }

        public string SupplierEmail
        {
            get { return _supplierEmail; }
            set
            {
                _supplierEmail = value;
                _nameChanged = true;
            }
        }
        public string SupplierTelephone
        {
            get { return _supplierTelephone; }
            set
            {
                _supplierTelephone = value;
                _nameChanged = true;
            }
        }
        public string SupplierContactName
        {
            get { return _supplierContactName; }
            set
            {
                _supplierContactName = value;
                _nameChanged = true;
            }
        }

        public string SupplierMobile
        {
            get { return _supplierMobile; }
            set
            {
                _supplierMobile = value;
                _nameChanged = true;
            }
        }

        
        public string SupplierWebsiteUrl
        {
            get { return _supplierWebsiteUrl; }
            set
            {
                _supplierWebsiteUrl = value;
                _nameChanged = true;
            }
        }
        public string SupplierAddress
        {
            get { return _supplierAddress; }
            set
            {
                _supplierAddress = value;
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
                if (columnName == "SupplierName")
                {
                    if (string.IsNullOrEmpty(SupplierName))
                        result = "Supplier name is required";

                }
                if (columnName == "SupplierEmail")
                {
                    if (string.IsNullOrEmpty(SupplierEmail))
                        result = "Email is required";
                    if (SupplierEmail != null && !Regex.IsMatch(SupplierEmail, CommonConstants._emailValidation))
                        result = "Please enter the valid email";

                }
                if (columnName == "SupplierContactName")
                {
                    if (string.IsNullOrEmpty(SupplierContactName))
                        result = "Contact name is required";

                }
                if (columnName == "SupplierMobile")
                {
                    if (string.IsNullOrEmpty(SupplierMobile))
                        result = "Mobile number is required";
                    if (SupplierMobile != null && !Regex.IsMatch(SupplierMobile, @"^\d{10,}$"))
                        result = "Minimum 10 digits required";

                }
                if (columnName == "SupplierTelephone")
                {
                    if (string.IsNullOrEmpty(SupplierTelephone))
                        result = "Telephone number is required";
                    if (SupplierTelephone != null && !Regex.IsMatch(SupplierTelephone, @"^\d{10,}$"))
                        result = "Minimum 16 digits required";

                }
                if (columnName == "SupplierWebsiteUrl")
                {
                    if (string.IsNullOrEmpty(SupplierWebsiteUrl))
                        result = "Url is required";
                    if (!Regex.IsMatch(SupplierWebsiteUrl, CommonConstants._urlValidation))
                        result = "please enter the valid url";

                }
                if (columnName == "SupplierAddress")
                {
                    if (string.IsNullOrEmpty(SupplierAddress))
                        result = "Address is required";

                }

                return result;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Model
{
   public class PaymentModelRequirements : IDataErrorInfo
    {
        private bool _valueChanged = false;
        private string _invoiceNo, _supplierName;
        private string _amount,_accountNo,_bank;
        private string _quantity, _discountType, _fromDate, _toDate;


        public string InvoiceNo
        {
            get { return _invoiceNo; }
            set
            {
                _invoiceNo = value;
                _valueChanged = true;
            }
        }
        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                _valueChanged = true;
            }
        }
        public string Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                _valueChanged = true;
            }
        }
        public string AccountNo
        {
            get { return _accountNo; }
            set
            {
                _accountNo = value;
                _valueChanged = true;
            }
        }
        public string BankName
        {
            get { return _bank; }
            set
            {
                _bank = value;
                _valueChanged = true;
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
                ///refracted Code 
                string result = null;
                if (_valueChanged)
                {
                    switch (columnName)
                    {
                        case "BankName":
                            if (string.IsNullOrEmpty(BankName))
                                result = columnName + " is required";
                            break;
                        case "AccountNo":
                            if (string.IsNullOrEmpty(AccountNo))
                                result = columnName + " is required";
                            break;
                      
                        case "Amount":
                            if (string.IsNullOrEmpty(Amount))
                                result = columnName + " is required";
                            break;
                        case "SupplierName":
                            if (string.IsNullOrEmpty(SupplierName))
                                result = columnName + " is required";
                            break;
                        case "InvoiceNo":
                            if (string.IsNullOrEmpty(InvoiceNo))
                                result = columnName + " is required";
                            break;
                            //case "FromDate":
                            //    if (Convert.ToDateTime(_fromDate) > Convert.ToDateTime(_toDate))
                            //        result = columnName + " can not be greater than to date";
                            //    break;
                            //case "ToDate":
                            //    if (Convert.ToDateTime(_fromDate) > Convert.ToDateTime(_toDate))
                            //        result = columnName + " can not be greater than to date";
                            //    break;
                    }
                }
                //else
                //{
                //    result = columnName;
                //}
                return result;
            }
        }
    }
}

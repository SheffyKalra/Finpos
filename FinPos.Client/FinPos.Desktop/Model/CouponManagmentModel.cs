using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Model
{
    public class CouponManagmentModel : IDataErrorInfo
    {

        private bool _valueChanged = false;
        private string _name,_value;
        private string _decription;
        private string _quantity, _discountType, _fromDate, _toDate;


        public string FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                _valueChanged = true;
            }
        }
        public string ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                _valueChanged = true;
            }
        }
        public string DiscountType
        {
            get { return _discountType; }
            set
            {
                _discountType = value;
                _valueChanged = true;
            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _valueChanged = true;
            }
        }
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                _valueChanged = true;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
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
                        case "Value":
                            if (string.IsNullOrEmpty(Value))
                                result = columnName + " is required";
                            break;
                        case "Quantity":
                            if (string.IsNullOrEmpty(Quantity))
                                result = columnName + " is required";
                            break;
                        case "Description":
                            if (string.IsNullOrEmpty(Name))
                                result = columnName + " is required";
                            break;
                        case "DiscountType":
                            if (DiscountType == "Select")
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

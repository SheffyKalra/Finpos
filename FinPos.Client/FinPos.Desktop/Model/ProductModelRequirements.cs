using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Model
{
    public class ProductModelRequirements : IDataErrorInfo
    {
        private bool _nameChanged = false;
        private string _itemName;
        private string _retailPrice;
        private string _tradePrice;
        private string _itemType;
        private string _barcode;
        private bool _requirePinNumber;
        private string _taxPercentage;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                _nameChanged = true;
            }
        }
        public string RetailPrice
        {
            get { return _retailPrice; }
            set
            {
                _retailPrice = value;
                _nameChanged = true;
            }
        }
        public string TradePrice
        {
            get { return _tradePrice; }
            set
            {
                _tradePrice = value;
                _nameChanged = true;
            }
        }
        public int SelectedIndex { get; set; }
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                _nameChanged = true;
            }
        }

        //public bool RequirePinNumber
        //{
        //    get
        //    {
        //        return this._requirePinNumber;
        //    }
        //    set
        //    {
        //        this._requirePinNumber = value;
                
        //    }
        //}
        public string Barcode
        {
            get { return _barcode; }
            set
            {
                _barcode = value;
                _nameChanged = true;
            }
        }

        public string TaxPercentage
        {
            get { return _taxPercentage; }
            set
            {
                _taxPercentage = value;
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
                if ( columnName == "ItemName")
                {
                    if (string.IsNullOrEmpty(ItemName))
                        result = "Product name is required";

                }
                if (columnName == "RetailPrice")
                {
                    if (string.IsNullOrEmpty(RetailPrice))
                        result = "Retail price  is required";
                   //else if ( !string.IsNullOrEmpty(RetailPrice) && !string.IsNullOrEmpty(TradePrice) && Convert.ToInt64(RetailPrice) < Convert.ToInt64(TradePrice))
                   //     result = "Retail Price not less than Trade Price";

                }
                if (columnName == "TradePrice")
                {
                    if (string.IsNullOrEmpty(TradePrice))
                        result = "Trade price is required";
                   //else if (!string.IsNullOrEmpty(RetailPrice) && !string.IsNullOrEmpty(TradePrice) && Convert.ToInt64(RetailPrice) < Convert.ToInt64(TradePrice))
                   //     result = "Trade Price not greater than Retail Price";

                }
                if (columnName == "TaxPercentage")
                {
                    if (string.IsNullOrEmpty(TaxPercentage))
                        result = "Tax Percentage is required";

                }
                if (columnName == "ItemType")
                {
                    if (string.IsNullOrEmpty(ItemType))
                    {
                        //  if (SelectedIndex == 0)
                        // {
                        result = "Please Select One";
                        //}
                    }
                }
                //if (_nameChanged && columnName == "RequirePinNumber")
                //{
                //   if (this.PropertyChanged != null)
                //{
                //    this.PropertyChanged(this, new PropertyChangedEventArgs("RequirePinNumber"));
                //    this.PropertyChanged(this, new PropertyChangedEventArgs("PinNumber"));
                //        result = "Please Checked Istax";
                //    }

                //}
                if (columnName == "Barcode")
                {
                    if (string.IsNullOrEmpty(Barcode))
                        result = "Barcode is required";

                }
                return result;
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}


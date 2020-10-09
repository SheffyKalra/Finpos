using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FinPos.DomainContracts.DataContracts
{
   public class PrintMasterSettingModel
    {
        private string _barcode;
        public string Barcode
        {
            get { return this._barcode; }
            set { this._barcode = value; }
        }
        private string _productPrice;
        public string Price
        {
            get { return this._productPrice; }
            set { this._productPrice = value; }
        }
        private string _productName;
        public string ProductName
        {
            get { return this._productName; }
            set { this._productName = value; }
        }
        private string _productCode;
        public string ProductCode
        {
            get { return this._productCode; }
            set { this._productCode = value; }
        }

        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }
    }
}

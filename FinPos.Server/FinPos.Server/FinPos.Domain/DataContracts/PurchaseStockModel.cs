using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class PurchaseStockModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public string _reason;
        public PurchaseStockModel() { }

        public PurchaseStockModel(long productCode, string productName)
        {
            ProductCode = productCode;
            ProductName = productName;
        }

        public PurchaseStockModel(long productCode, string productName, int quantity, decimal costPrice, decimal sellingPrice, decimal mrp, int discount, string batchNo, int? stockId)
        {
            this.ProductCode = productCode;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.SellingPrice = sellingPrice;
            this.MRP = mrp;
            this.Discount = discount;
            this.BatchNo = batchNo;
            this.StockId = stockId;
        }

        public PurchaseStockModel(long productCode, string productName, int quantity, decimal costPrice, int discount, string batchNo,int quantityForRetruned,int retrunedQuantity,int purchaseReturnId,int sellingPrice,string reasonForRetruned)
        {
            this.ProductCode = productCode;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.Discount = discount;
            this.BatchNo = batchNo;
            this.QuantityForRetrun = quantityForRetruned;
            this.ReturnedQuantity = retrunedQuantity;
            this.PurchaseRetrunId = purchaseReturnId;
            this.SellingPrice = sellingPrice;
            this.ReasonForRetrun = reasonForRetruned;
        }

        [DataMember]
        public int? StockId { get; set; }
        [DataMember]
        public long ProductCode { get; set; }
        public long productCode
        {
            get { return ProductCode; }
            set
            {

                ProductCode = value;
                NotifyPropertyChanged("ProductCode");
            }
        }

        [DataMember]
        public string ProductName { get; set; }

        public string productName
        {
            get { return ProductName; }
            set
            {
                ProductName = value;
                NotifyPropertyChanged("ProductName");
            }
        }

        [DataMember]
        public int Quantity { get; set; }

        public int quantity
        {
            get { return Quantity; }
            set
            {
                Quantity = value;
                NotifyPropertyChanged("Quantity");
            }
        }

        public int QuantityForRetrun { get; set; }

        public int PurchaseRetrunId { get; set; }
        public int purchasedQuantity
        {
            get { return QuantityForRetrun; }
            set
            {
                QuantityForRetrun = value;
                NotifyPropertyChanged("QuantityForRetrun");
            }
        }

        public int ReturnedQuantity { get; set; }

        public int retrunedQuantity
        {
            get { return ReturnedQuantity; }
            set
            {
                ReturnedQuantity = value;
                NotifyPropertyChanged("ReturnedQuantity");
            }
        }

        [DataMember]
        public decimal CostPrice { get; set; }

        public decimal costPrice
        {
            get { return CostPrice; }
            set
            {

                NotifyPropertyChanged("CostPrice");
            }
        }

        [DataMember]
        public decimal SellingPrice { get; set; }

        public decimal sellingPrice
        {
            get { return SellingPrice; }
            set
            {

                SellingPrice = value;
                NotifyPropertyChanged("SellingPrice");
            }
        }

        [DataMember]
        public decimal MRP { get; set; }

        public decimal mrp
        {
            get { return MRP; }
            set
            {
                MRP = value;
                NotifyPropertyChanged("MRP");
            }
        }

        [DataMember]
        public int Discount { get; set; }

        public int discount
        {
            get { return Discount; }
            set
            {
                Discount = value;
                NotifyPropertyChanged("Discount");
            }
        }

        [DataMember]
        public string BatchNo { get; set; }

        public string batchNo
        {
            get { return BatchNo; }
            set
            {
                BatchNo = value;
                NotifyPropertyChanged("BatchNo");
            }
        }

        //public string ReasonForRetrun
        //{
        //    get { return _reason; }
        //    set
        //    {
        //        _reason = value;
        //    }
        //}

        [DataMember]
        public string ReasonForRetrun { get; set; }

        public string reason
        {
            get { return ReasonForRetrun; }
            set
            {
                ReasonForRetrun = value;
                _reason = value;
                NotifyPropertyChanged("ReasonForRetrun");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            Quantity = 0;
            SellingPrice = 0;
            CostPrice = 0;
            MRP = 0;
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
                if (columnName == "CostPrice")
                {
                    if (CostPrice > SellingPrice && CostPrice != 0 && SellingPrice != 0)
                    {
                        result = "Selling Price not less than Cost Price";
                    }
                    if (CostPrice > MRP && CostPrice != 0 && MRP != 0)
                        result = "MRP Price not less than Cost Price";
                }
                if (columnName == "SellingPrice")
                {
                    if (SellingPrice < CostPrice && CostPrice != 0 && SellingPrice != 0)
                        result = "Selling Price not less than Cost Price";
                    if (SellingPrice > MRP && MRP != 0 && SellingPrice != 0)
                        result = "MRP Price not less than Selling Price";
                }
                if (columnName == "MRP")
                {
                    if (MRP < CostPrice && CostPrice != 0 && MRP != 0)
                        result = "MRP not less than Cost price";
                    if (MRP < SellingPrice && SellingPrice != 0 && MRP != 0)
                        result = "MRP not less than Selling price";
                }
                if (columnName == "Discount")
                {
                    if (Discount > 100)
                        result = "Invalid discount value";
                }
                if (columnName == "QuantityForRetrun")
                {
                    if (ReturnedQuantity >= Quantity)
                        result = "Quantity limit is exceed";
                }
                if (columnName == "ReasonForRetrun")
                {
                    if (string.IsNullOrEmpty(ReasonForRetrun))
                        result = "Reason is required";
                }

                return result;
            }

        }
    }
}

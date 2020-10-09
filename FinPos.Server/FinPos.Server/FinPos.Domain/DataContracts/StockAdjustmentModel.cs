using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class StockAdjustmentModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private bool isReason = false;

        public StockAdjustmentModel()
        {
           
        }
        public StockAdjustmentModel(long productCode, string productName)
        {
            ProductCode = productCode;
            ProductName = productName;
        }
        [DataMember]
        public long? Id { get; set; }

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
        public int CurrentStock { get; set; }
        public int currentStock
        {
            get { return CurrentStock; }
            set
            {
                CurrentStock = value;
                NotifyPropertyChanged("CurrentStock");
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
        [DataMember]
        public string Reason { get; set; }
        public string reason
        {
            get { return Reason; }
            set
            {
                Reason = value;
                NotifyPropertyChanged("Reason");
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
        [DataMember]
        public int? CompanyCode { get; set; }

        [DataMember]
        public int? BranchCode { get; set; }

        [DataMember]
        public string ExpiryDate { get; set; }

        [DataMember]
        public String CreatedDate { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
                if (columnName == "Quantity")
                {
                    if (Quantity < 0)
                    {
                        isReason = true;
                        NotifyPropertyChanged("Reason");
                    }
                    else if(Quantity==0)
                    {
                        isReason = false;
                       // NotifyPropertyChanged("Reason");

                        //no
                    }
                    else
                    {
                        isReason = false;
                        NotifyPropertyChanged("Reason");
                    }

                }
                if (columnName == "Reason")
                {
                    if (string.IsNullOrEmpty(this.Reason) && this.isReason)
                        result = "Reason is required";

                }
                return result;
            }

        }
    }
}

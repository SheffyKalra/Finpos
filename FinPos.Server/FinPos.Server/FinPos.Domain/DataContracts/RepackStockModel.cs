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
   public class RepackStockModel : INotifyPropertyChanged
    {
        public RepackStockModel()
        {

        }
        public RepackStockModel(long productCode, string productName)
        {
            ProductCode = productCode;
            ProductName = productName;
        }
        public decimal? RetailPrice { get; set; }
         public decimal? TotalQuantityStock { get; set; }
        [DataMember]
        public decimal? Weight { get; set; }

        [DataMember]
        public string BatchNo { get; set; }

        public decimal? weight
        {
            get { return Weight; }
            set
            {
                Weight = value;
                NotifyPropertyChanged("Weight");
            }
        }
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
        public int? CompanyCode { get; set; }

        [DataMember]
        public int? BranchCode { get; set; }

        [DataMember]
        public string ExpiryDate { get; set; }

        [DataMember]
        public String CreatedDate { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

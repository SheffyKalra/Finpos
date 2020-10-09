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
    public class ChildProductItemsVm : INotifyPropertyChanged
    {
        public ChildProductItemsVm()
        {

        }
        public ChildProductItemsVm(int quantity, decimal retailPrice)
        {
            Quantity = quantity;
            RetailPrice = retailPrice;
        }
        public int Quantity { get; set; }

        public decimal RetailPrice { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

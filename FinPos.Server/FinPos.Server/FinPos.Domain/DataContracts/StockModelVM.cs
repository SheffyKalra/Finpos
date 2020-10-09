using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class StockModelVM
    {
        public StockModelVM(int? id, int quantity, decimal costPrice, string period)
        {
            this.StockId = id;
            this.Quantity = quantity;
            this.CostPrice = costPrice;
            this.Period = period;
        }

        [DataMember]
        public int? StockId { get; set; }
        [DataMember]
        public string Period { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal CostPrice { get; set; }
    }
}

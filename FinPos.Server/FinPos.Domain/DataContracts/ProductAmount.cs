using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
   public class ProductAmount
    {
        public ProductAmount(string netTotal,string totalAmount)
        {
            this.NetTotal = netTotal;
            this.TotalAmount = totalAmount;
        }
        public string NetTotal { get; set; }

        public string TotalAmount { get; set; }
    }
}

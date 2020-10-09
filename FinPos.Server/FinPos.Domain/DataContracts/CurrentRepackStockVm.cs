using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
   public class CurrentRepackStockVm
    {
        public CurrentRepackStockVm(decimal? currentStock,decimal? packedStock,decimal? availStock)
        {
            this.CurrentStock = currentStock;
            this.PackedStock = packedStock;
            this.AvailStock = availStock;
        }

        public decimal? CurrentStock { get; set; }

        public decimal? PackedStock { get; set; }

        public decimal? AvailStock { get; set; }
    }
}

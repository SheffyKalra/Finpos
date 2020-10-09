using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface IOpeningStockRepository
    {
        void SaveOpeningStocks(List<OpeningStock> Stocks);
        List<OpeningStock> GetOpeningStockByProductCode();    
    }
}

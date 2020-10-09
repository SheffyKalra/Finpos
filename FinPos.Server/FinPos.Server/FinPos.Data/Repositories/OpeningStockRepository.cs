using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class OpeningStockRepository : IOpeningStockRepository
    {

        private readonly IEntityProvider<OpeningStock> _openingAndStockAdjustmentProvider;
        public OpeningStockRepository(IEntityProvider<OpeningStock> openingAndStockAdjustmentProvider)
        {
            _openingAndStockAdjustmentProvider = openingAndStockAdjustmentProvider;
        }
        public void SaveOpeningStocks(List<OpeningStock> Stocks)
        {
            var ss = _openingAndStockAdjustmentProvider.Get();
            foreach (OpeningStock stock in Stocks)
            {
                if (stock.ProductCode != null)
                {
                    _openingAndStockAdjustmentProvider.Insert(stock);
                }
            }
        }

        public List<OpeningStock> GetOpeningStockByProductCode()
        {
            return _openingAndStockAdjustmentProvider.Get().ToList();
        }
    }
}

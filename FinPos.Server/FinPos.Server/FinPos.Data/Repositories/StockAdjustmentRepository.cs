using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FinPos.Data.Repositories
{
    public class StockAdjustmentRepository : IStockAdjustmentRepository
    {
        private readonly IEntityProvider<StockAdjustment> _stockAdjustmentProvider;

        public StockAdjustmentRepository(IEntityProvider<StockAdjustment> stockAdjustmentProvider)
        {
            _stockAdjustmentProvider = stockAdjustmentProvider;
        }
        public void SaveStockAdjustment(List<StockAdjustment> stockAdjustments)
        {
            foreach (StockAdjustment stock in stockAdjustments)
            {
                _stockAdjustmentProvider.Insert(stock);
            }
        }
        public List<StockAdjustment> GetStockAdjustments()
        {
          return _stockAdjustmentProvider.Get().ToList();               
        }
    }
}

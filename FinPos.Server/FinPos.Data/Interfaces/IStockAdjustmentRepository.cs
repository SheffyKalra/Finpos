using FinPos.Data.Entities;
using System.Collections.Generic;

namespace FinPos.Data.Interfaces
{
    public interface IStockAdjustmentRepository
    {
        void SaveStockAdjustment(List<StockAdjustment> stockAdjustments);
        List<StockAdjustment> GetStockAdjustments();
    }
}

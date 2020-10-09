using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        private readonly IEntityProvider<Tax> _taxProvider;
        public TaxRepository(IEntityProvider<Tax> taxProvider)
        {
            _taxProvider = taxProvider;
        }
        public List<Tax> GetTax()
        {
             List<Tax> taxs = new List<Tax>();
            taxs= _taxProvider.Get().ToList();
            return taxs;
        }
        public void SaveUpdateTax(Tax model)
        {
            if (model.Id > 0)
                this._taxProvider.Update(model);
            else
                this._taxProvider.Insert(model);
        }
        public void DeleteTax(Tax tax)
        {
            this._taxProvider.Delete(tax);
        }
    }
}

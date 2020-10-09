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
    public class OfferDetailRepository : IOfferDetailRepository
    {
        private readonly IEntityProvider<OfferDetail> _offerDetailProvider;
        public OfferDetailRepository(IEntityProvider<OfferDetail> offerDetailProvides)
        {
            this._offerDetailProvider = offerDetailProvides;
        }
    }
}

using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
   public class Tax: BaseEntity
    {
        public Tax()
        {

        }
        public Tax(int? id, string taxDetail, double rate, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.Id = id.Value;
            this.TaxDetail = taxDetail;
            //  this.Code = code;
            this.Rate = rate;
            this.CreatedDate = createdDate;
            this.ModifiedDate = updatedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;

        }


        public string TaxDetail { get; set; }

        public double Rate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

    }
}


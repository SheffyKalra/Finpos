using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class SystemConfiguration : BaseEntity
    {
        public SystemConfiguration() { }
        public SystemConfiguration(int? id,string finalYearStartDate,string finalYearEndDate,string createdDate)
        {
            this.Id = id;
            this.FInalYearStartDate = finalYearStartDate;
            this.FinalYearEndDate = finalYearEndDate;
            this.CreatedDate = createdDate;
        }
        public string FInalYearStartDate { get; set ; }
        public string FinalYearEndDate { get; set; }

    }
}

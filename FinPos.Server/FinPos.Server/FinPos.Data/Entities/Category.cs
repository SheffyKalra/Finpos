using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Category : BaseEntity
    {

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public long? BranchCode { get; set; }

        public bool? IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public string ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public long CompanyCode { get; set; }
    }
}

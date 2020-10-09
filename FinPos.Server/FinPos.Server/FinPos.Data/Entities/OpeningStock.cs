using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class OpeningStock : BaseEntity
    {
        public long ProductCode { get; set; }
        public int Quantity { get; set; }
        public string ExpiryDate { get; set; }
        public string BatchNo { get; set; }
        public int? Branchcode { get; set; }
        public int? CompanyCode { get; set; }
        public int? CreatedBy { get; set; }
    }
}

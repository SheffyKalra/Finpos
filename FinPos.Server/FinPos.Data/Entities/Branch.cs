using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Branch : BaseEntity
    {
        public Branch()
        {

        }
        public Branch(int? id, int companyId, string name, string description, string address, bool isDefault, bool isActive, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.CompanyCode = companyId;
            this.Id = id.Value;
            // this.Code = code;
            this.Name = name;
            this.Description = description;
            this.Address = address;
            this.CreatedDate = createdDate;
            this.ModifiedDate = updatedDate;
            this.IsDefault = isDefault;
            this.IsActive = isActive;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;
        }



        public int CompanyCode { get; set; }
        //  public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        //[ForeignKey("CompanyId")]
        //public CompanyData Company { get; set; }

    }
}

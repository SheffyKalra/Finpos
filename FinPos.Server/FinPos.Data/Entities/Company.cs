using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DAL.Entities
{
    public class Company : BaseEntity
    {
        public Company()
        {

        }
        //public CompanyData(int? id, int code, string name, string description, string phoneNo, string logo, bool isDefault, bool isActive, DateTime? createdDate, DateTime? updatedDate, string modifiedBy, string createdBy)
        //{
        public Company(int? id, string name, string description, string phoneNo, string logo,  bool isDefault, bool isActive, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.PhoneNo = phoneNo;
            this.Logo = logo;
            this.CreatedDate = createdDate;
            this.ModifiedDate = updatedDate;
            this.IsDefault = isDefault;
            this.IsActive = isActive;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;
        }



        // public int? Id { get; set; }
      
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNo { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        //public ICollection<BranchData> Branch { get; set; }
    }
}

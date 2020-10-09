using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class BranchModel
    {
        public BranchModel()
        {

        }
        public BranchModel(string name, int? code, string description)
        {
            this.Name = name;
            this.Code = code;
            this.Description = description;
        }
        public BranchModel(int? id, int companyId, string name, string description, string address, bool isDefault, bool isActive, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.Id = id;
            this.CompanyId = companyId;
            this.Name = name;
          //  this.Code = code;
            this.Description = description;
            this.Address = address;
            this.IsDefault = isDefault;
            this.IsActive = isActive;
            this.CreatedDate = createdDate;
            this.UpdatedDate = updatedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;

        }
        public BranchModel(string name,int companyId,int? id)
        {
            this.Name = name;
            this.CompanyId = companyId;
            this.Id = id.Value;
        }

        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string UpdatedDate { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }


    }
}

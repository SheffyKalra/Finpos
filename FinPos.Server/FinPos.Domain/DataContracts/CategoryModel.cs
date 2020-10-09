using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class CategoryModel
    {
        public CategoryModel(int? id, string categoryName, string description, long? branchcode, bool? isDeleted, string createdDate, int createdby, string modeifiedDate, int? modifiedBy, bool isActive, string activeStatus, long companyCode)
        {
            this.Id = id;
            this.CategoryName = categoryName;
            this.Description = description;
            this.BranchCode = branchcode;
            this.IsDeleted = isDeleted;
            this.CreatedDate = createdDate;
            this.Createdby = createdby;
            this.ModifiedDate = modeifiedDate;
            this.ModifiedBy = modifiedBy;
            this.IsActive = isActive;
            this.ActiveStatus = activeStatus;
            this.CompanyCode = companyCode;
        }
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public long? BranchCode { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public int Createdby { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }

        [DataMember]
        public int? ModifiedBy { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string ActiveStatus { get; set; }

        [DataMember]
        public long CompanyCode { get; set; }

    }
}

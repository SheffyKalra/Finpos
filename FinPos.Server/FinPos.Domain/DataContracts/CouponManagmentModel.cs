using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class CouponManagmentModel
    {
        public CouponManagmentModel()
        {

        }
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int DiscountType { get; set; }
        [DataMember]
        public int CouponCode { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public int? BranchCode { get; set; }
        [DataMember]
        public int NoOfCoupons { get; set; }
    }
}

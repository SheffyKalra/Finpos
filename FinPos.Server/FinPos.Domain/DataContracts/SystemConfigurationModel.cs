using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public  class SystemConfigurationModel
    {
        public SystemConfigurationModel(int? id, string finalYearStartDate, string finalYearEndDate, string createdDate)
        {
            this.SystemConfigurationId = id;
            this.FinalYearStartDate = finalYearStartDate;
            this.FinalYearEndDate = finalYearEndDate;
            this.CreatedDate = createdDate;
        }
        [DataMember]
        public int? SystemConfigurationId { get; set; }
        [DataMember]
        public string FinalYearStartDate { get; set; }
        [DataMember]
        public string FinalYearEndDate { get; set; }
        [DataMember]

        public string CreatedDate { get; set; }
    }
}

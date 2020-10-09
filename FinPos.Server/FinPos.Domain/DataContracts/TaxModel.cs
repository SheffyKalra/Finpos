using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class TaxModel
    {
        public TaxModel()
        {

        }
        public TaxModel(int? id, string taxDetail)
        {
            this.TaxCode = id;
            this.TaxDetail = taxDetail;
        }
        public TaxModel(int? id, string taxDetail, double rate, string createdDate, string updatedDate, string modifiedBy, string createdBy)
        {
            this.TaxCode = id;
            this.TaxDetail = taxDetail;
            //  this.Code = code;
            this.Rate = rate;
            this.CreatedDate = createdDate;
            this.ModifiedDate = updatedDate;
            this.ModifiedBy = modifiedBy;
            this.CreatedBy = createdBy;

        }
        [DataMember]
        public int? TaxCode { get; set; }

        [DataMember]
        public string TaxDetail { get; set; }

        [DataMember]
        public double Rate { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }


    }
}

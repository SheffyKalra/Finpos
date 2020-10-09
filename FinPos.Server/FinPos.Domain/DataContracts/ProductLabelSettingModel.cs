using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class ProductLabelSettingModel
    {
        public ProductLabelSettingModel() { }
        public ProductLabelSettingModel(int id, int masterLabelCode, int productCode,int quantity,string productName)
        {
            this.Id = id;
            this.MasterLabelCode = masterLabelCode;
            this.ProductCode = productCode;
            this.Quantity = quantity;
            this.ProductName = productName;
            // this.CreatedDate = createdDate;
        }
        [DataMember]
        public  int Id { get; set; }
        [DataMember]
        public int MasterLabelCode { get; set; }
        [DataMember]
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        [DataMember]
        public int Quantity { get; set; }
    }
}

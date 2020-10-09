using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class ProductLabelSetting : BaseEntity
    {
        public ProductLabelSetting() { }
        public ProductLabelSetting(int? id,int masterLabelCode,int productCode,int quantity)
        {
            this.Id = id;
            this.MasterLabelSettingCode = masterLabelCode;
            this.ProductCode = productCode;
            this.Quantity = quantity;
           // this.CreatedDate = createdDate;
        }
        public int MasterLabelSettingCode { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}

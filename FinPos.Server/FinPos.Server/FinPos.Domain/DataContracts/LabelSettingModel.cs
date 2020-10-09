using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class LabelSettingModel
    {
        public LabelSettingModel()
        {

        }
        public LabelSettingModel(int? id, int labelSettingCode, int itemId, bool printItemCode, bool printItemDetail, string printUnitMeasure, bool printItemPrice, bool printBarCode, string barCodeHeight,string labelSheet,string totalNoOfPrints,string startRow,string startColumn)
        {
            Id = id;

            LabelSettingCode = labelSettingCode;

            ItemId = itemId;

            PrintItemCode = printItemCode;

            PrintItemDetail = printItemDetail;

            PrintUnitMeasure = printUnitMeasure;

            PrintItemPrice = printItemPrice;

            PrintBarCode = printBarCode;

            BarCodeHeight = barCodeHeight;

            LabelSheet = labelSheet;

            TotalNoOfPrints = totalNoOfPrints;

            StartRow = startRow;

            StartColumn = startColumn;
        }

        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int LabelSettingCode { get; set; }
        [DataMember]
        public int ItemId { get; set; }
        [DataMember]
        public bool PrintItemCode { get; set; }
        [DataMember]
        public bool PrintItemDetail { get; set; }
        [DataMember]
        public string PrintUnitMeasure { get; set; }
        [DataMember]
        public bool PrintItemPrice { get; set; }
        [DataMember]
        public bool PrintBarCode { get; set; }
        [DataMember]
        public string BarCodeHeight { get; set; }

        [DataMember]
        public string LabelSheet { get; set; }

        [DataMember]
        public string TotalNoOfPrints { get; set; }

        [DataMember]
        public string StartRow { get; set; }

        [DataMember]
        public string StartColumn { get; set; }

    }
}

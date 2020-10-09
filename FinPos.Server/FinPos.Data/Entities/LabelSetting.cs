using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class LabelSetting : BaseEntity
    {
        public LabelSetting()
        {

        }
        public LabelSetting(int? id, int labelSettingCode, int itemId, bool printItemCode, bool printItemDetail, string printUnitMeasure, bool printItemPrice, bool printBarCode, string barCodeHeight,string labelSheet,string startRow,string startColumn)
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
            StartRow = startRow;
            StartColumn = startColumn;

        }
        
        public int LabelSettingCode { get; set; }
        public int ItemId { get; set; }
        public bool PrintItemCode { get; set; }
        public bool PrintItemDetail { get; set; }
        public string PrintUnitMeasure { get; set; }
        public bool PrintItemPrice { get; set; }
        public bool PrintBarCode { get; set; }
        public string BarCodeHeight { get; set; }
        public string LabelSheet { get; set; }
        public string TotalNoOfPrints { get; set; }
        public string StartRow { get; set; }
        public string StartColumn { get; set; }
    }
}

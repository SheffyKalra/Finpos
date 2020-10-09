using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    public class PrintPurchaseReturnModel
    {
        public PrintPurchaseReturnModel(long productCode, string productName, int quantity, decimal costPrice, decimal total, string reason, int returnQty)
        {
            this.PrintProductCode = productCode;
            this.PrintProductName = productName;
            this.PrintQuantity = quantity;
            this.PrintCostPrice = costPrice;
            this.PrintTotal = total;
            this.Reason = reason;
            this.ReturnQty = returnQty;
        }
        public string Reason { get; set; }

        public int ReturnQty { get; set; }

        public long PrintProductCode { get; set; }

        public string PrintProductName { get; set; }

        public int PrintQuantity { get; set; }

        public decimal PrintCostPrice { get; set; }

        public decimal PrintTotal { get; set; }
    }
}
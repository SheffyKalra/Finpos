using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost
{
    public class CommonFunctions
    {
        public static int CalcCurrentStock(int stockQuantity, int openingStockQuantity, int wastageQuantity, int stockAdjustQuantity)
        {
            return (stockQuantity + openingStockQuantity - wastageQuantity) + stockAdjustQuantity;
        }
        public static DateTime ParseDateToFinclave(string date)
        {
            DateTime _date;
            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateTime.TryParseExact(date, sysFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _date);
            return _date;
        }
        public static string ParseDateToFinclaveString(string date)
        {
            DateTime _date;
            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateTime.TryParseExact(date, sysFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _date);
            return _date.ToShortDateString();
        }
    }
}

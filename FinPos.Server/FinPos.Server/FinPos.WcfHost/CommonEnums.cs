using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost
{
    public class CommonEnums
    {
        public enum MonuthStatus
        {
            [Description("January")]
            January = 1,
            [Description("February")]
            February = 2,
            [Description("March")]
            March = 3,
            [Description("April")]
            April = 4,
            [Description("May")]
            FullyReturned = 5,
            [Description("June")]
            June = 6,
            [Description("July")]
            July = 7,
            [Description("August")]
            August = 8,
            [Description("September")]
            September = 9,
            [Description("October")]
            October = 10,
            [Description("November")]
            November = 11,
            [Description("December")]
            December = 12
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Utility.Constants
{
    public class CommonConstants
    {
        public const string _emailValidation = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
        public const string _urlValidation = @"[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
        public const string _randomNumber = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        #region application color code 
        public const string _redColorCode = "#eb5151";
        public const string _greenColorCode = "#0091EA";
        public const string _purpleColorCode = "#864D8F";
        public const string _pazeSize = "ISO";
        #endregion
    }
}

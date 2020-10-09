using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPOS.DomainContracts.Model
{
    public class LicenseModel
    {
        public LicenseModel()
        {

        }

        public LicenseModel(string licenseKey, string macAddress, string accessToken)
        {
            this.LicenseKey = licenseKey;
            this.MacAddress = macAddress;
            this.AccessToken = accessToken;
        }
        public LicenseModel(string licenseKey, string accessToken)
        {
            this.LicenseKey = licenseKey;
            this.AccessToken = accessToken;
        }
        public string LicenseKey { get; set; }

        public string MacAddress { get; set; }

        public string AccessToken { get; set; }
    }
}

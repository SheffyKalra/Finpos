using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System.Configuration;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.ServiceProcess;
using System.Windows.Controls;
using FinPos.DomainContracts.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FinPos.Utility.CommonMethods;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace FinPos.Utility.CommonMethods
{
    public class CommonFunctions
    {
        public dynamic _accessToken;
        public dynamic _licenseKey;
        public string _macAddress;
        public CommonFunctions()
        {

        }
        public CommonFunctions(dynamic license, dynamic accessToken, string macaddress)
        {
            this._licenseKey = license;
            this._accessToken = accessToken;
            this._macAddress = macaddress;
        }

        public bool createRegistry(string edition, int plantype, int industryType)
        {
            bool isRegistered;
            if (industryType == (int)CommonEnums.CommonEnum.IndustryTypes.RS)
            {
                // if (edition == Convert.ToString(CommonEnums.Edition.Client))
                // {
                //  if (plantype == (int)CommonEnums.PlanTypes.Trial)
                // {
                isRegistered = RegesteredKey(edition, plantype, Convert.ToString(CommonEnums.CommonEnum.IndustryTypes.RS));
                return isRegistered;
            }
            else if (industryType == (int)CommonEnums.CommonEnum.IndustryTypes.FB)
            {
                isRegistered = RegesteredKey(edition, plantype, Convert.ToString(CommonEnums.CommonEnum.IndustryTypes.FB));
                return isRegistered;
            }
            else if (industryType == (int)CommonEnums.CommonEnum.IndustryTypes.BS)
            {
                isRegistered = RegesteredKey(edition, plantype, Convert.ToString(CommonEnums.CommonEnum.IndustryTypes.BS));
                return isRegistered;
            }
            else if (industryType == (int)CommonEnums.CommonEnum.IndustryTypes.RB)
            {
                isRegistered = RegesteredKey(edition, plantype, Convert.ToString(CommonEnums.CommonEnum.IndustryTypes.RB));
                return isRegistered;
            }
            else
            {
                isRegistered = RegesteredKey(edition, plantype, Convert.ToString(CommonEnums.CommonEnum.IndustryTypes.PH));
                return isRegistered;
            }


        }
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static void IntegerValueChecker(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            char ch = e.Text[0];
            if ((char.IsNumber(ch)))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        public static void DecimalValueChecker(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            char ch = e.Text[0];
            if ((Char.IsDigit(ch) || ch == '.'))
            {
                if (ch == '.' && textBox.Text.Contains("."))
                    e.Handled = true;
            }
            else
                e.Handled = true;
        }

        public static ProductAmount RetrunProductAmount(List<PurchaseStockModel> stocks, string cashDisPer, string cashDicDoller, string taxAmount, string subcharge)
        {
            decimal total = 0;
            decimal netTotal = 0;
            stocks.ForEach(x =>
            {
                if (x.Quantity > 0 && x.CostPrice > 0)
                    total += x.CostPrice * Convert.ToDecimal(x.Quantity) - ((x.CostPrice * Convert.ToDecimal(x.Quantity) * x.Discount) / 100); //+( string.IsNullOrEmpty(purchase_cashdiscount.Text) ? 0 : int.Parse(purchase_cashdiscount.Text)) +( string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? 0 : int.Parse(purchase_cashdiscountDoller.Text));

            });
            var netAmount = total - (total * (string.IsNullOrEmpty(cashDisPer) ? 0 : decimal.Parse(cashDisPer)) / 100); //+ (total * (string.IsNullOrEmpty(taxAmount) ? 0 : int.Parse(taxAmount) / 100)) + (string.IsNullOrEmpty(subcharge) ? 0 : int.Parse(subcharge));
            netTotal = netAmount + (netAmount * (string.IsNullOrEmpty(taxAmount) ? 0 : decimal.Parse(taxAmount)) / 100) + (string.IsNullOrEmpty(subcharge) ? 0 : decimal.Parse(subcharge));
            return new ProductAmount(Convert.ToString(netTotal), Convert.ToString(total));
        }

        public static ProductAmount RetrunProductAmountWithReturnProduct(List<PurchaseStockModel> stocks, string cashDisPer, string cashDicDoller, string taxAmount, string subcharge)
        {
            decimal total = 0;
            decimal netTotal = 0;
            stocks.ForEach(x =>
            {
                if (x.Quantity > 0 && x.CostPrice > 0)
                    total += x.CostPrice * Convert.ToDecimal(x.Quantity - x.ReturnedQuantity) - ((x.CostPrice * Convert.ToDecimal(x.Quantity - x.ReturnedQuantity) * x.Discount) / 100); //+( string.IsNullOrEmpty(purchase_cashdiscount.Text) ? 0 : int.Parse(purchase_cashdiscount.Text)) +( string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? 0 : int.Parse(purchase_cashdiscountDoller.Text));

            });
            var netAmount = total - (total * (string.IsNullOrEmpty(cashDisPer) ? 0 : decimal.Parse(cashDisPer)) / 100); //+ (total * (string.IsNullOrEmpty(taxAmount) ? 0 : int.Parse(taxAmount) / 100)) + (string.IsNullOrEmpty(subcharge) ? 0 : int.Parse(subcharge));
            netTotal = netAmount + (netAmount * (string.IsNullOrEmpty(taxAmount) ? 0 : decimal.Parse(taxAmount)) / 100) + (string.IsNullOrEmpty(subcharge) ? 0 : decimal.Parse(subcharge));
            return new ProductAmount(Convert.ToString(netTotal), Convert.ToString(total));
        }
        public static bool ServiceStatus(string serviceName)
        {

            ServiceController sc = new ServiceController(serviceName);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return true;
                case ServiceControllerStatus.Stopped:
                    return false;
                case ServiceControllerStatus.Paused:
                    return false;
                case ServiceControllerStatus.StopPending:
                    return false;
                case ServiceControllerStatus.StartPending:
                    return false;
                default:
                    return false;
            }
        }
        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface macAddress in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (macAddress.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    macAddress.OperationalStatus == OperationalStatus.Up)
                {
                    return macAddress.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static bool IsLicenseActivate(string edition, string industryType)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Salt", true))
            {
                if (key != null)
                {
                    using (RegistryKey isIndustry = key.OpenSubKey(industryType, true))
                    {
                        if (isIndustry != null)
                        {
                            using (RegistryKey isServer = isIndustry.OpenSubKey(edition, true))
                            {
                                if (isServer != null)
                                {
                                    try
                                    {


                                        Object licenseKey = isServer.GetValue("LicenseKey");
                                        //string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licenseKey), true);
                                        //string[] getvalue = modifylicense.Split('-');
                                        //string expiryDate = getvalue[7].ToString();
                                        //string keyCreatedDate = getvalue[4].ToString();
                                        //string planType = getvalue[1];
                                        //string createdDate = isServer.GetValue("Salt").ToString();
                                        //string macAddress = isServer.GetValue("SaltAddress").ToString();
                                        //string currentDate = ParseDateToFinclaveString(DateTime.Now.ToString());



                                        //if (planType == "1")
                                        //{
                                        //    if (ParseDateToFinclave(keyCreatedDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(createdDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(currentDate).Date <= ParseDateToFinclave(expiryDate).Date)
                                        //    {
                                        //        int? dateDiff = (ParseDateToFinclave(expiryDate) - ParseDateToFinclave(currentDate)).Days + 1;
                                        //        if (ParseDateToFinclave(currentDate).Date < ParseDateToFinclave(currentDate).Date)
                                        //        {
                                        //            isServer.SetValue("SaltAL", 0);
                                        //        }
                                        //        isServer.SetValue("Salt", ParseDateToFinclave(DateTime.Now.ToShortDateString()));
                                        //        isServer.Close();
                                        //        return true;
                                        //    }
                                        //}

                                        //else if (planType == "2")
                                        //{
                                        //    int? dateDiff = ((ParseDateToFinclave(expiryDate) - ParseDateToFinclave(currentDate)).Days) + 1;
                                        //    isServer.SetValue("Salt", ParseDateToFinclave(DateTime.Now.ToShortDateString()));
                                        //    isServer.Close();
                                        //    return true;
                                        //}
                                        //else
                                        //{
                                        //    return false;
                                        //}
                                        return true;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static ProductAmount RetrunProductAmountOfReturnProduct(List<PurchaseStockModel> stocks, string cashDisPer, string cashDicDoller, string taxAmount, string subcharge)
        {
            decimal total = 0;
            decimal netTotal = 0;
            stocks.ForEach(x =>
            {
                if (x.Quantity > 0 && x.CostPrice > 0)
                    total += x.CostPrice * Convert.ToDecimal(x.ReturnedQuantity) - ((x.CostPrice * Convert.ToDecimal(x.ReturnedQuantity) * x.Discount) / 100); //+( string.IsNullOrEmpty(purchase_cashdiscount.Text) ? 0 : int.Parse(purchase_cashdiscount.Text)) +( string.IsNullOrEmpty(purchase_cashdiscountDoller.Text) ? 0 : int.Parse(purchase_cashdiscountDoller.Text));

            });
            var netAmount = total - (total * (string.IsNullOrEmpty(cashDisPer) ? 0 : int.Parse(cashDisPer)) / 100); //+ (total * (string.IsNullOrEmpty(taxAmount) ? 0 : int.Parse(taxAmount) / 100)) + (string.IsNullOrEmpty(subcharge) ? 0 : int.Parse(subcharge));
            netTotal = netAmount + (netAmount * (string.IsNullOrEmpty(taxAmount) ? 0 : int.Parse(taxAmount)) / 100) + (string.IsNullOrEmpty(subcharge) ? 0 : int.Parse(subcharge));
            return new ProductAmount(Convert.ToString(netTotal), Convert.ToString(total));
        }

        public static int CalcCurrentStock(int stockQuantity, int openingStockQuantity, int wastageQuantity, int stockAdjustQuantity)
        {
            return (stockQuantity + openingStockQuantity - wastageQuantity) + stockAdjustQuantity;
        }

        public static bool IsActivatedTrue(string edition, string industryType)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Salt", true))
            {
                if (key != null)
                {
                    using (RegistryKey isIndustry = key.OpenSubKey(industryType, true))
                    {
                        if (isIndustry != null)
                        {
                            using (RegistryKey isServer = isIndustry.OpenSubKey(edition, true))
                            {
                                if (isServer != null)
                                {
                                    Object licenseKey = isServer.GetValue("LicenseKey");
                                    //string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licenseKey), true);
                                    //string[] getvalue = modifylicense.Split('-');
                                    //string expiryDate = getvalue[7].ToString();
                                    //string keyCreatedDate = getvalue[4].ToString();
                                    //string planType = getvalue[1];
                                    //string createdDate = isServer.GetValue("Salt").ToString();
                                    //string macAddress = isServer.GetValue("SaltAddress").ToString();
                                    //string currentDate = ParseDateToFinclaveString(Convert.ToString(DateTime.Now));
                                    //if (planType == "1")
                                    //{
                                    //    return ParseDateToFinclave(keyCreatedDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(createdDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(currentDate).Date <= ParseDateToFinclave(expiryDate).Date ? true : false;
                                    //}

                                    //else if (planType == "2")
                                    //{
                                    //    return ParseDateToFinclave(keyCreatedDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(createdDate).Date <= ParseDateToFinclave(currentDate).Date && ParseDateToFinclave(currentDate).Date <= ParseDateToFinclave(expiryDate).Date ? true : false;
                                    //}

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static bool IsKeyExpired(string edition, string industryType)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Salt", true))
            {
                if (key != null)
                {
                    using (RegistryKey isIndustry = key.OpenSubKey(industryType, true))
                    {
                        if (isIndustry != null)
                        {
                            using (RegistryKey isServer = isIndustry.OpenSubKey(edition, true))
                            {
                                if (isServer != null)
                                {
                                    try
                                    {
                                        Object licenseKey = isServer.GetValue("LicenseKey");

                                        return true;                     //                   string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licenseKey), true);
                     //                   string[] getvalue = modifylicense.Split('-');
                     //                   string expiryDate = getvalue[7].ToString();
                     //                   string planType = getvalue[1];
                     //                   string createdDate = isServer.GetValue("Salt").ToString();
                     //                   int maxLimit = Convert.ToInt32(isServer.GetValue("SaltML"));
                     //                   int accessLimit = Convert.ToInt32(isServer.GetValue("SaltAL"));
                     //                   //  string macAddress = key.GetValue("SaltAddress").ToString();
                     //                   string currentDate = ParseDateToFinclaveString(Convert.ToString(DateTime.Now));
                     //                   if (planType == "1")
                     //                   {
                     //                       string dateString = expiryDate;  // your date.

                     //                       string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt","dd-MMM-yy",
                     //"MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                     //"M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                     //"M/d/yyyy h:mm", "M/d/yyyy h:mm",
                     //"MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                     //"MM/d/yyyy HH:mm:ss.ffffff" };

                     //                       //expiryDate = DateTime.ParseExact(dateString, formats, new CultureInfo("en-US"), DateTimeStyles.None).ToString();
                     //                       //string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                     //                       // string[] ssize = expiryDate.Split(null);
                     //                       // string strFormat = "yyyyMMdd";
                     //                       // string val = String.Format("{0:" + strFormat + "}", ssize[0]);
                     //                       //// DateTime date2 = Convert.ToDateTime(val);
                     //                       // DateTime date = DateTime.ParseExact(ssize[0], sysFormat, null);
                     //                       // DateTime date = DateTime.ParseExact(ssize[0], "MM/DD/YY", CultureInfo.InvariantCulture);

                     //                       if (ParseDateToFinclave(createdDate).Date > ParseDateToFinclave(expiryDate).Date || ParseDateToFinclave(createdDate).Date > ParseDateToFinclave(expiryDate).Date)
                     //                       {
                     //                           return true;
                     //                       }

                     //                   }
                     //                   else if (planType == "2")
                     //                   {
                     //                       if (ParseDateToFinclave(createdDate) > ParseDateToFinclave(expiryDate) || ParseDateToFinclave(currentDate).Date > ParseDateToFinclave(expiryDate).Date)
                     //                       {
                     //                           return true;
                     //                       }
                     //                       //if (Convert.ToDateTime(createdDate) > Convert.ToDateTime(expiryDate) || Convert.ToDateTime(currentDate).Date > Convert.ToDateTime(expiryDate).Date)
                     //                       //{
                     //                       //    return true;
                     //                       //}
                     //                   }
                     //                   else
                     //                   {
                     //                       return false;
                     //                   }
                                    }

                                    catch (Exception ex)
                                    {


                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        //// This extension method is broken out so you can use a similar pattern with 
        //// other MetaData elements in the future. This is your base method for each.
        //public static T GetAttribute<T>(this Enum value) where T : Attribute
        //{
        //    var type = value.GetType();
        //    var memberInfo = type.GetMember(value.ToString());
        //    var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        //    return (T)attributes[0];
        //}

        //// This method creates a specific call to the above method, requesting the
        //// Description MetaData attribute.
        //public static string ToName(this Enum value)
        //{
        //    var attribute = value.Gee<DescriptionAttribute>();
        //    return attribute == null ? value.ToString() : attribute.Description;
        //}

        public static byte[] ImageToByteArray(BitmapImage imageIn)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageIn));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        public static BitmapImage ByteToBitmapImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            return biImg;
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

        public static bool IsLimitExpired(string edition, string industryType)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Salt", true))
            {
                if (key != null)
                {
                    using (RegistryKey isIndustry = key.OpenSubKey(industryType, true))
                    {
                        if (isIndustry != null)
                        {
                            using (RegistryKey isServer = isIndustry.OpenSubKey(edition, true))
                            {
                                if (isServer != null)
                                {
                                    Object licenseKey = isServer.GetValue("LicenseKey");
                                    //string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licenseKey), true);
                                    //string[] getvalue = modifylicense.Split('-');
                                    //string expiryDate = getvalue[7].ToString();
                                    //string planType = getvalue[1];
                                    //string createdDate = isServer.GetValue("Salt").ToString();
                                    //int maxLimit = Convert.ToInt32(isServer.GetValue("SaltML"));
                                    //int accessLimit = Convert.ToInt32(isServer.GetValue("SaltAL"));
                                    ////  string macAddress = key.GetValue("SaltAddress").ToString();
                                    //string currentDate = ParseDateToFinclaveString(Convert.ToString(DateTime.Now));
                                    //if (planType == "1")
                                    //{
                                    //    if (ParseDateToFinclave(createdDate).Date == ParseDateToFinclave(DateTime.Now.ToShortDateString()).Date && accessLimit >= maxLimit)
                                    //    {
                                    //        return true;
                                    //    }
                                    //    else
                                    //    {
                                    //        isServer.SetValue("SaltAL", accessLimit + 1);
                                    //        return false;
                                    //    }

                                    //}

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsMonthlyTimeExpired(string edition, string industryType)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Salt", true))
            {
                if (key != null)
                {
                    using (RegistryKey isIndustry = key.OpenSubKey(industryType, true))
                    {
                        if (isIndustry != null)
                        {
                            using (RegistryKey isServer = isIndustry.OpenSubKey(edition, true))
                            {
                                if (isServer != null)
                                {
                                    Object licenseKey = isServer.GetValue("LicenseKey");
                                    return true;
                                    //string modifylicense = ExtensionMethods.Decrypt(Convert.ToString(licenseKey), true);
                                    //string[] getvalue = modifylicense.Split('-');
                                    //string expiryDate = getvalue[7].ToString();
                                    //string planType = getvalue[1];
                                    //string createdDate = isServer.GetValue("Salt").ToString();
                                    //string monthlyExpired = isServer.GetValue("SaltEx").ToString();
                                    ////  string macAddress = key.GetValue("SaltAddress").ToString();
                                    //string currentDate = ParseDateToFinclaveString(Convert.ToString(DateTime.Now));
                                    //if (planType == "1")
                                    //{
                                    //    if (ParseDateToFinclave(createdDate).Date > ParseDateToFinclave(monthlyExpired).Date)
                                    //    {
                                    //        return true;
                                    //    }

                                    //}
                                    //else if (planType == "2")
                                    //{
                                    //    if (ParseDateToFinclave(createdDate) > ParseDateToFinclave(monthlyExpired))
                                    //    {
                                    //        return true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    return false;
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool RegesteredKey(string edition, int plantype, string industryType)
        {
            RegistryKey exsist = Registry.CurrentUser.OpenSubKey("Salt", true);
            RegistryKey isIndustryExsist = exsist?.OpenSubKey(industryType, true);
            RegistryKey isEditionExsist = isIndustryExsist?.OpenSubKey(edition, true);
            try
            {
                if (exsist == null)
                {

                    RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Salt");
                    RegistryKey subRegistryKey = registryKey.CreateSubKey(Convert.ToString(industryType));
                    RegistryKey subEditionRegistryKey = subRegistryKey.CreateSubKey(edition);
                    subEditionRegistryKey.SetValue("AccessToken", this._accessToken);
                    subEditionRegistryKey.SetValue("LicenseKey", this._licenseKey);
                    subEditionRegistryKey.SetValue("Salt", ParseDateToFinclave(DateTime.Now.ToShortDateString()));
                    subEditionRegistryKey.SetValue("SaltEx", ParseDateToFinclave(DateTime.Now.ToShortDateString()).AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["DateLimit"])));
                    //if (edition == "Client")
                    //{
                    subEditionRegistryKey.SetValue("SaltML", 99);
                    subEditionRegistryKey.SetValue("SaltAL", 1);
                    // }
                    subEditionRegistryKey.SetValue("SaltAddress", this._macAddress);
                    subEditionRegistryKey.Close();
                    return true;
                }
                else if (isIndustryExsist == null)
                {
                    RegistryKey subRegistryKey = exsist.CreateSubKey(Convert.ToString(industryType));
                    RegistryKey subEditionRegistryKey = subRegistryKey.CreateSubKey(edition);
                    subEditionRegistryKey.SetValue("AccessToken", this._accessToken);
                    subEditionRegistryKey.SetValue("LicenseKey", this._licenseKey);
                    subEditionRegistryKey.SetValue("Salt", ParseDateToFinclave(DateTime.Now.ToShortDateString()));
                    subEditionRegistryKey.SetValue("SaltEx", ParseDateToFinclave(DateTime.Now.ToShortDateString()).AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["DateLimit"])));
                    //if (edition == "Client")
                    // {
                    subEditionRegistryKey.SetValue("SaltML", 3);
                    subEditionRegistryKey.SetValue("SaltAL", 1);
                    // }
                    subEditionRegistryKey.SetValue("SaltAddress", this._macAddress);
                    subEditionRegistryKey.Close();
                    return true;
                }
                // return false;

                else if (isEditionExsist == null)
                {
                    RegistryKey subEditionRegistryKey = isIndustryExsist.CreateSubKey(edition);
                    subEditionRegistryKey.SetValue("AccessToken", this._accessToken);
                    subEditionRegistryKey.SetValue("LicenseKey", this._licenseKey);
                    subEditionRegistryKey.SetValue("Salt", ParseDateToFinclave(DateTime.Now.ToShortDateString()));
                    subEditionRegistryKey.SetValue("SaltEx", ParseDateToFinclave(DateTime.Now.ToShortDateString()).AddMonths(Convert.ToInt32(ConfigurationManager.AppSettings["DateLimit"])));
                    // if (edition == "Client")
                    //  {
                    subEditionRegistryKey.SetValue("SaltML", 3);
                    subEditionRegistryKey.SetValue("SaltAL", 1);
                    // }
                    subEditionRegistryKey.SetValue("SaltAddress", this._macAddress);
                    subEditionRegistryKey.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                return false;
            }
        }



    }

    public class Sort : Adorner
    {
        private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public Sort(UIElement element, ListSortDirection dir)
                : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                    (
                            AdornedElement.RenderSize.Width - 15,
                            (AdornedElement.RenderSize.Height - 5) / 2
                    );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(System.Windows.Media.Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }


    public static class LogError
    {
        public static void LogErrorMessage(string ActionName, string ErrorMessage)
        {
            string filepath = @"c:\Exception\error.txt";  //Text File Path

            if (!Directory.Exists(@"c:\Exception"))
            {
                Directory.CreateDirectory(@"c:\Exception");
            }
            //Text File Name
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine(ActionName + ": " + ErrorMessage);
                sw.WriteLine("--------------------------------*End*------------------------------------------");

                sw.Flush();
                sw.Close();

            }
        }
    }
    public static class Validations
    {
        public static bool StringHasSpace(KeyEventArgs ke)
        {
            return (ke.Key == Key.Space) ? true : false;
        }
    }

}

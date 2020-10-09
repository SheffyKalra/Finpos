using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class ProductModel
    {
        public ProductModel(string name, decimal retailPrice, decimal tradePrice, int categoryCode, int itemType, string barCode, decimal taxPercentage)
        {
            // this.Id = id;
            this.ItemName = name;
            this.RetailPrice = retailPrice;
            this.TradePrice = tradePrice;
            this.CategoryCode = categoryCode;
            this.ItemType = itemType;
            this.BarCode = barCode;
            this.TaxPercentage = taxPercentage;
        }
        public ProductModel(int? id, int itemCode, string name, decimal retailPrice, decimal tradePrice, int categoryCode, int itemType, string barCode, decimal taxPercentage, string categoryName)
        {
            this.Id = id;
            this.ItemName = name;
            this.RetailPrice = retailPrice;
            this.TradePrice = tradePrice;
            this.CategoryCode = categoryCode;
            this.ItemType = itemType;
            this.BarCode = barCode;
            this.TaxPercentage = taxPercentage;
            this.CategoryName = categoryName;
            this.ItemCode = itemCode;
        }

        public ProductModel(int? id, string itemName, int? categoryCode, decimal? retailPrice, decimal? tradePrice, decimal? wholesellerPrice, decimal? resellerPrice, int itemType,
            decimal? weight, string barcode, decimal? taxPercentage, int? minimumLevel, int? reoderLevel, byte[] itemImage, bool isTaxInclusive, string shortName, string description, int? branchCode, string categoryName, string itemTypeName, ImageSource image, string imageText, int companyCode,int? bulkCode)
        {
            this.Id = id;
            this.ItemName = itemName;
            this.CategoryCode = categoryCode;
            this.RetailPrice = retailPrice;
            this.TradePrice = tradePrice;
            this.WholeSellerPrice = wholesellerPrice;
            this.ResellerPrice = resellerPrice;
            this.ItemType = itemType;
            this.Weight = weight;
            this.BarCode = barcode;
            this.TaxPercentage = taxPercentage;
            this.MinimumLevel = minimumLevel;
            this.ReOrderLevel = reoderLevel;
            this.ItemImage = itemImage;
            this.IsTaxInclusive = isTaxInclusive;
            this.ShortName = shortName;
            this.Description = description;
            this.BranchCode = branchCode;
            this.CategoryName = categoryName;
            this.ItemTypeName = itemTypeName;
            this.ImageSource = image;
            this.ImageText = imageText;
            this.CompanyCode = companyCode;
            this.BulkCode = bulkCode;
        }
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int ItemCode { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public decimal? RetailPrice { get; set; }
        [DataMember]
        public decimal? TradePrice { get; set; }

        [DataMember]
        public decimal? WholeSellerPrice { get; set; }

        [DataMember]
        public decimal? ResellerPrice { get; set; }

        [DataMember]
        public decimal? Weight { get; set; }

        [DataMember]
        public int? MinimumLevel { get; set; }

        [DataMember]
        public int? ReOrderLevel { get; set; }
        [DataMember]
        public byte[] ItemImage
        {
            get; set;
            //get { return ItemImage; }

            //    //return ItemImage = Convert.FromBase64String(ImageText);


            //set
            //{
            //    if (ImageText != null)
            //    {
            //        ItemImage = Convert.FromBase64String(ImageText);
            //    }
            //}
        }
        [DataMember]
        public int? CategoryCode { get; set; }
        [DataMember]
        public int ItemType { get; set; }
        [DataMember]
        public string BarCode { get; set; }

        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public decimal? TaxPercentage { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? BranchCode { get; set; }
        [DataMember]
        public bool IsTaxInclusive { get; set; }
        [DataMember]
        public string ItemTypeName { get; set; }

        [DataMember]
        public ImageSource ImageSource { get; set; }
        [DataMember]
        public string ImageText { get; set; }
        [DataMember]
        public int CurrentStock { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public int? BulkCode { get; set; }

    }
}

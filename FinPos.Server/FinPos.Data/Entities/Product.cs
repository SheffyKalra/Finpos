using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {

        }
        public Product(int? id, string name, decimal? retailPrice, decimal? tradePrice, decimal? wholesellerPrice, decimal? weight, bool isTaxInclusive, int? categoryCode, int itemType, string barCode, decimal? taxPercentage, int? minimumLevel, int? reOderLevel, byte[] itemImage, string shortName, string description, string categoryName, string imageText, int branchCode, int companyCode, int? bulkCode)
        {
            Id = id;
            ItemName = name;
            RetailPrice = retailPrice;
            TradePrice = tradePrice;
            WholesellerPrice = wholesellerPrice;
            ResellerPrice = ResellerPrice;
            Weight = weight;
            IsTaxInclusive = isTaxInclusive;
            CategoryCode = categoryCode;
            ItemType = itemType;
            BarCode = barCode;
            TaxPercentage = taxPercentage;
            MinimumLevel = minimumLevel;
            ReOrderLevel = reOderLevel;
            ItemImage = itemImage;
            ShortName = shortName;
            Description = description;
            ImageText = imageText;
            BranchCode = branchCode;
            CompanyCode = companyCode;
            BulkCode = bulkCode;
        }

        public string ItemName { get; set; }
        public int? CategoryCode { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? TradePrice { get; set; }
        public decimal? WholesellerPrice { get; set; }
        public decimal? ResellerPrice { get; set; }
        public int ItemType { get; set; }
        public decimal? Weight { get; set; }
        public string BarCode { get; set; }
        public bool IsTaxInclusive { get; set; }
        public decimal? TaxPercentage { get; set; }
        public int? MinimumLevel { get; set; }
        public int? ReOrderLevel { get; set; }
        public byte[] ItemImage { get; set; }
        public string ShortName { get; set; }

        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string Description { get; set; }

        public int? BranchCode { get; set; }
        public string ImageText { get; set; }
        public int CompanyCode { get; set; }
        public int? BulkCode { get; set; }
    }
}

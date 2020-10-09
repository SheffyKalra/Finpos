using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {

        }
        public Supplier(int? id, string supplierName, string shortName, string address, string contactName, string telephone, string mobile, string fax, string websiteUrl, string email, string notes, decimal? discountPercentage, decimal tax, bool taxInclusive, int companyCode, int? branchCode)
        {
            this.Id = id;
            this.SupplierName = supplierName;
            this.ShortName = shortName;
            this.Address = address;
            this.ContactName = contactName;
            this.Telephone = telephone;
            this.Mobile = mobile;
            this.Fax = fax;
            this.WebsiteUrl = websiteUrl;
            this.Email = email;
            this.Notes = notes;
            this.DiscountPercentage = discountPercentage;
            this.Tax = tax;
            this.TaxInclusive = taxInclusive;
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
        }
        public Supplier(string supplierName, string shortName, string address, string contactName, string telephone, string mobile, string fax, string websiteUrl, string email, string notes, decimal? discountPercentage, decimal tax, bool taxInclusive, int companyCode, int? branchCode)
        {
            this.SupplierName = supplierName;
            this.ShortName = shortName;
            this.Address = address;
            this.ContactName = contactName;
            this.Telephone = telephone;
            this.Mobile = mobile;
            this.Fax = fax;
            this.WebsiteUrl = websiteUrl;
            this.Email = email;
            this.Notes = notes;
            this.DiscountPercentage = discountPercentage;
            this.Tax = tax;
            this.TaxInclusive = taxInclusive;
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
        }
        public string SupplierName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal Tax { get; set; }
        public bool TaxInclusive { get; set; }
        public bool IsDeleted { get; set; }
        public int PurchaseType { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int CompanyCode { get; set; }
        public int? BranchCode { get; set; }
    }
}

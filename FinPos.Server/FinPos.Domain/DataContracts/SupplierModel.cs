using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
    [DataContract]
    public class SupplierModel
    {
        public SupplierModel()
        {

        }
        public SupplierModel(int?id,string supplierName)
        {
            this.Id = id;
            this.SupplierName = supplierName;
        }
        public SupplierModel(int? id, string supplierName, string shortName, string address, string contactName, string telephone, string mobile, string fax, string websiteUrl, string email, string notes, decimal? discountPercentage, int companyCode, int? branchCode)
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
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
        }
        public SupplierModel(string supplierName, string shortName, string address, string contactName, string telephone, string mobile, string fax, string websiteUrl, string email, string notes, decimal? discountPercentage, int companyCode, int? branchCode)
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
            this.CompanyCode = companyCode;
            this.BranchCode = branchCode;
        }

        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string Telephone { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string WebsiteUrl { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public decimal? DiscountPercentage { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public bool TaxInclusive { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public int? BranchCode { get; set; }
    }
}

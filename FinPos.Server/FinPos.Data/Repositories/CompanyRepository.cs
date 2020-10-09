using FinPos.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinPos.DAL.Entities;
using FinPos.Data.Entities;

namespace FinPos.DAL.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IEntityProvider<Company> _companyProvider;

        private readonly IEntityProvider<Branch> _branchProvider;
        public CompanyRepository(IEntityProvider<Company> companyProvider, IEntityProvider<Branch> branchProvider)
        {
            this._companyProvider = companyProvider;
            this._branchProvider = branchProvider;
        }

        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();
            companies = this._companyProvider.Get().ToList();
            return companies;
        }

        public List<Company> GetActiveCompanies()
        {
            List<Company> companies = new List<Company>();
            companies = this._companyProvider.Get().Where(item => item.IsActive == true).ToList();
            return companies;
        }

        public IList<Company> GetCompanyById(int companyId)
        {
            return this._companyProvider.Get().Where(item => item.Id == companyId).ToList();
        }
        //public int SaveCUstomer(CompanyData company)
        //{
        //    return 0;
        //    // this.customerProvider.Insert(new CompanyData(company.Name, company.Address));
        //}
        public int SaveUpdateCompany(Company company)
        {
            if (company.Id > 0)
            {
                return this._companyProvider.Update(company);
            }
            else
            {
                return this._companyProvider.Insert(company);
            }
        }

        public void SaveUpdateBranch(Branch branch)
        {
            if (branch.Id > 0)
                this._branchProvider.Update(branch);
            else
                this._branchProvider.Insert(branch);
        }
    }
}

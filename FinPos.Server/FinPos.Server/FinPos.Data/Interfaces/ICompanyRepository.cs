using System.Collections.Generic;
using FinPos.DAL.Entities;
using FinPos.Data.Entities;
using FinPos.Domain.DataContracts;

namespace FinPos.DAL.Interfaces
{
    public interface ICompanyRepository
    {
        List<Company> GetCompanies();
        int SaveUpdateCompany(Company customer);

        void SaveUpdateBranch(Branch branch);
        List<Company> GetActiveCompanies();
        IList<Company> GetCompanyById(int companyId);
    }
}

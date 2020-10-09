using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly IEntityProvider<Branch> _branchProvider;
        public BranchRepository(IEntityProvider<Branch> branchProvider)
        {
            this._branchProvider = branchProvider;
        }
        public List<Branch> GetCompanyBranches(int companyId)
        {
            List<Branch> branches = new List<Branch>();
            branches = this._branchProvider.Get().Where(x => x.CompanyCode == companyId).ToList();
            return branches;
        }
        public List<Branch> GetCompanyActiveBranches(int companyId)
        {
            List<Branch> branches = new List<Branch>();
            branches = this._branchProvider.Get().Where(x => x.CompanyCode == companyId && x.IsActive == true).ToList();
            return branches;
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


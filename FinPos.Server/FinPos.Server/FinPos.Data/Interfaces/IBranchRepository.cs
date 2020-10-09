using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
   public interface IBranchRepository
    {
        List<Branch> GetCompanyBranches(int companyId);

        void SaveUpdateBranch(Branch branch);
        List<Branch> GetCompanyActiveBranches(int companyId);
    }
}

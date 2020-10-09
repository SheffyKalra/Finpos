using FinPos.Domain.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DomainContracts.DataContracts
{
   public class CompanyBranchVm
    {
        //public CompanyBranchVm(CompanyModel  company,BranchModel branch)
        //{
        //    this.Company = company;
        //    this.Branch = branch;
        //}
        //public CompanyModel Company { get; set; }
        ////  public List<FinPos.DomainContracts.DataContracts.BranchModel> Branch { get; set; }
        //public BranchModel Branch { get; set; }

        public CompanyBranchVm(CompanyModel company, List<BranchModel> branch)
        {
            this.Company = company;
            this.Branch = branch;
        }
        public CompanyModel Company { get; set; }
        //  public List<FinPos.DomainContracts.DataContracts.BranchModel> Branch { get; set; }
        public List<BranchModel> Branch { get; set; }
    }
}

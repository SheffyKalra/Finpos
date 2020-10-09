using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;
namespace FinPos.WcfHost.Interface
{
    [ServiceContract]
    public interface ICompanyService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<CompanyModel> GetCompanies();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<CompanyModel> GetCompanyById(int companyId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int SaveUpdateCompany(CompanyModel model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<BranchModel> GetCompanyBranches(int companyId);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<CompanyModel> GetActiveCompanies();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveUpdateBranch(BranchModel model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<BranchModel> GetCompanyActiveBranches(int companyId);
    }
}

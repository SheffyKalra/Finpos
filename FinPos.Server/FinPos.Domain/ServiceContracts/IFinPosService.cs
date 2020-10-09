using System;
using System.Collections.Generic;
using System.ServiceModel;
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;

namespace FinPos.Domain.ServiceContracts
{
    [ServiceContract]
    public interface IFinPosService
    {
        [OperationContract]
        IList<CompanyModel> GetCompanies();
        [OperationContract]
        IList<ProductModel> GetProducts();
        [OperationContract]
        void SaveUpdateCompany(CompanyModel model);

        [OperationContract]
        void UpdateExistDefaultCompany(int? id);

        [OperationContract]
        IList<BranchModel> GetCompanyBranches(int companyId);

        [OperationContract]
        void SaveUpdateBranch(BranchModel model);

        [OperationContract]
        void UpdateExistDefaultBranch(int companyId, int branchId);

        [OperationContract]

        UserModel GetUser(string email, string password);

        [OperationContract]
        void SaveUpdateUser(UserModel user);

        //[OperationContract]
        ////IList<CompanyVM> GetCompanyWithBranches();
    }
}

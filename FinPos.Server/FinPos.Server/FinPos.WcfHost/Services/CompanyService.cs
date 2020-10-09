using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "CompanyService", Namespace = "http://locahost:8080/CompanyService", InstanceContextMode = InstanceContextMode.Single)]
    public class CompanyService : ICompanyService
    {
        #region Properties
        private readonly ICompanyRepository _companyRepository;
        private readonly IBranchRepository _branchRepository;
        FaultData fault = new FaultData();
        #endregion
             
        #region Constructor
        public CompanyService(ICompanyRepository companyRepository, IBranchRepository branchRepository)
        {
            this._companyRepository = companyRepository;
            this._branchRepository = branchRepository;
        }
        #endregion
     
        #region Getter Methods
        public IList<CompanyModel> GetCompanies()
        {
            try
            {
                return _companyRepository.GetCompanies().Select(x => new CompanyModel(x.Id, x.Name, x.Description, x.PhoneNo,
                   x.Logo, x.IsDefault, x.IsActive,
                   !string.IsNullOrEmpty(x.CreatedDate) ? CommonFunctions.ParseDateToFinclaveString(x.CreatedDate) : x.CreatedDate,
                   x.ModifiedDate, x.ModifiedBy, x.CreatedBy)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCompanies method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<CompanyModel> GetCompanyById(int companyId)
        {
            try
            {
                return _companyRepository.GetCompanyById(companyId).Select(x => new CompanyModel(x.Id, x.Name, x.Description, x.PhoneNo,
                   x.Logo, x.IsDefault, x.IsActive,
                   !string.IsNullOrEmpty(x.CreatedDate) ? CommonFunctions.ParseDateToFinclaveString(x.CreatedDate) : x.CreatedDate,
                   x.ModifiedDate, x.ModifiedBy, x.CreatedBy)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCompanyById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<BranchModel> GetCompanyBranches(int companyId)
        {
            try
            {
                return _branchRepository.GetCompanyBranches(companyId).Select(x => new BranchModel(x.Id, x.CompanyCode, x.Name, x.Description, x.Address, x.IsDefault, x.IsActive, CommonFunctions.ParseDateToFinclaveString(x.CreatedDate), "Modifieddate", x.ModifiedBy, x.CreatedBy)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCompanyBranches method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<CompanyModel> GetActiveCompanies()
        {
            try
            {
                return _companyRepository.GetActiveCompanies().Select(x => new CompanyModel(x.Id, x.Name, x.Description, x.PhoneNo, x.Logo, x.IsDefault, x.IsActive, !string.IsNullOrEmpty(x.CreatedDate) ? CommonFunctions.ParseDateToFinclaveString(x.CreatedDate): x.CreatedDate, x.ModifiedDate, x.ModifiedBy, x.CreatedBy)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCompanies method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
        public IList<BranchModel> GetCompanyActiveBranches(int companyId)
        {
            try
            {
                List<Branch> branches = _branchRepository.GetCompanyActiveBranches(companyId);
                return branches.ToList().Select(x => new BranchModel(x.Id, x.CompanyCode, x.Name, x.Description, x.Address, x.IsDefault, x.IsActive, CommonFunctions.ParseDateToFinclaveString(x.CreatedDate), "Modifieddate", x.ModifiedBy, x.CreatedBy)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCompanyBranches method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion

        #region CRUD Operations
        public int SaveUpdateCompany(CompanyModel model)
        {
            Company obj;
            try
            {
                if (model.IsDefault)
                {
                    UpdateExistDefaultCompany(model.Id);
                }

                if (model.Id > 0)
                {
                    obj = _companyRepository.GetCompanies().FirstOrDefault(x => x.Id == model.Id);
                    obj.IsActive = model.IsActive;
                    obj.Logo = model.Logo;
                    obj.Name = model.Name;
                    obj.PhoneNo = model.PhoneNo;
                    obj.Description = model.Description;
                    obj.IsDefault = model.IsDefault;
                    obj.ModifiedDate = model.UpdatedDate;
                    return _companyRepository.SaveUpdateCompany(obj);
                }
                else
                {
                    Company company = new Company(model.Id, model.Name, model.Description, model.PhoneNo, model.Logo, model.IsDefault, model.IsActive, model.CreatedDate, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                    return _companyRepository.SaveUpdateCompany(company);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateCompany method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }

        public void UpdateExistDefaultCompany(int? id)
        {
            try
            {
                Company IsDefaulltExist = new Company();
                if (id.Value > 0)
                {
                    IsDefaulltExist = _companyRepository.GetCompanies().FirstOrDefault(x => x.Id != id && x.IsDefault);
                }
                else
                {
                    IsDefaulltExist = _companyRepository.GetCompanies().FirstOrDefault(x => x.IsDefault);
                }
                if (IsDefaulltExist != null)
                {
                    IsDefaulltExist.IsDefault = false;
                    _companyRepository.SaveUpdateCompany(IsDefaulltExist);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateExistDefaultCompany method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }

      

        public void SaveUpdateBranch(BranchModel model)
        {
            Branch obj;
            try
            {
                if (model.IsDefault)
                {
                    UpdateExistDefaultBranch(model.CompanyId, model.Id.Value);
                }

                if (model.Id > 0)
                {
                    obj = _branchRepository.GetCompanyBranches(model.CompanyId).FirstOrDefault(x => x.Id == model.Id);
                    obj.IsActive = model.IsActive;
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                    obj.IsDefault = model.IsDefault;
                    obj.Address = model.Address;
                    _branchRepository.SaveUpdateBranch(obj);
                }
                else
                {
                    Branch branch = new Branch(model.Id, model.CompanyId, model.Name, model.Description, model.Address, model.IsDefault, true, model.CreatedDate, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                    _branchRepository.SaveUpdateBranch(branch);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateBranch method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public void UpdateExistDefaultBranch(int companyId, int banchId)
        {
            try
            {
                List<Branch> isDefaulltExist = new List<Branch>();
                if (companyId > 0 && banchId > 0)
                {
                    isDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.Id != banchId).ToList();

                }
                else
                {
                    isDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.IsDefault).ToList();

                }
                foreach (var obj in isDefaulltExist)
                {
                    obj.IsDefault = false;
                    _branchRepository.SaveUpdateBranch(obj);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateExistDefaultBranch method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion

    }
}

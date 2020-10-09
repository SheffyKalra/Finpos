using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FinPos.Domain.DataContracts;
using FinPos.DAL.Repositories;
using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.Data.Entities;
using FinPos.WcfHost;
//using FinPos.Domain.ServiceContracts;
//using FinPos.WcfHost;

namespace FinPos.Service
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FinPosService" in both code and config file together.
    [ServiceBehavior(Name = "FinPosService", Namespace = "http://locahost:8080/finposservice", InstanceContextMode = InstanceContextMode.Single)]
    public class FinPosService : IFinPosService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;

        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        private readonly IBranchRepository _branchRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ILabelSettingRepository _labelSettingRepository;

        public FinPosService(ICompanyRepository companyRepository, IBranchRepository branchRepository, IUserRepository userRepository, IProductRepository productRepository, IPurchaseRepository purchaseRepository, ILabelSettingRepository labelSettingRepository)//, IInventoryRepository inventoryRepository)
        {
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            this._userRepository = userRepository;
            this._productRepository = productRepository;
            this._purchaseRepository = purchaseRepository;
            _labelSettingRepository = labelSettingRepository;
        }
        public IList<CompanyModel> GetCompanies()
        {
            List<CompanyData> companies = _companyRepository.GetCompanies();
            return companies.ToList().Select(x => new CompanyModel(x.Id, x.Code, x.Name, x.Description, x.PhoneNo, x.Logo,  x.IsDefault, x.IsActive, x.CreatedDate, x.ModifiedDate, x.ModifiedBy, x.CreatedBy)).OrderByDescending(x => x.CreatedDate).ToList();
            // return companies.ToList().Select(x => new CompanyModel(x.Id, x.Code, x.Name, x.Description, x.PhoneNo, x.Logo, x.IsDefault, x.IsActive, x.CreatedDate, x.ModifiedDate, null, null)).ToList();
        }

        public void SaveUpdateCompany(CompanyModel model)
        {
            CompanyData obj;

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
                obj.ModifiedDate = DateTime.Now;
                _companyRepository.SaveUpdateCompany(obj);

            }
            else
            {
                CompanyData company = new CompanyData(model.Id, model.Code.Value, model.Name, model.Description, model.PhoneNo, model.Logo, model.IsDefault, true, DateTime.Now, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                _companyRepository.SaveUpdateCompany(company);
            }

        }
        public void UpdateExistDefaultCompany(int? id)
        {
            CompanyData companyData = new CompanyData();
            companyData = id.Value > 0 ? _companyRepository.GetCompanies().FirstOrDefault(x => x.Id != id && x.IsDefault) : _companyRepository.GetCompanies().FirstOrDefault(x => x.IsDefault);
            if (companyData != null)
            {
                companyData.IsDefault = false;
                _companyRepository.SaveUpdateCompany(companyData);
            }
        }

        public IList<BranchModel> GetCompanyBranches(int companyId)
        {
            List<BranchData> branches = _branchRepository.GetCompanyBranches(companyId);
            return branches.ToList().Select(x => new BranchModel(x.Id, x.CompanyId, x.Code, x.Name, x.Description, x.Address, x.IsDefault, x.IsActive, x.CreatedDate, x.ModifiedDate, x.ModifiedBy, x.CreatedBy)).OrderByDescending(x => x.CreatedDate).ToList();
        }
        public void SaveUpdateBranch(BranchModel model)
        {
            BranchData obj;

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
                obj.Address = model.Address;
                obj.IsDefault = model.IsDefault;
                _branchRepository.SaveUpdateBranch(obj);

            }
            else
            {
                BranchData company = new BranchData(model.Id, model.CompanyId, model.Code.Value, model.Name, model.Description, model.Address, model.IsDefault, true, DateTime.Now, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                _branchRepository.SaveUpdateBranch(company);
            }
        }
        public void UpdateExistDefaultBranch(int companyId, int banchId)
        {
            if (companyId > 0 && banchId > 0)
            {
                var IsDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.Id != banchId).ToList();

                foreach (var obj in IsDefaulltExist)
                {
                    obj.IsDefault = false;

                    _branchRepository.SaveUpdateBranch(obj);

                }
            }
            else
            {
                var IsDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.IsDefault).ToList();
                foreach (var obj in IsDefaulltExist)
                {
                    obj.IsDefault = false;

                    _branchRepository.SaveUpdateBranch(obj);

                }
            }
        }

        public IList<UserModel> GetUsers()
        {
            List<UserData> users = _userRepository.GetUsers();
            return users.ToList().Select(x => new UserModel(x.Id, x.UserCode, x.CreatedDate, x.FirstName, x.LastName, x.IsAdmin, x.Email, x.Password, x.IsActive, x.ModifiedDate, x.ModifiedBy, x.CreatedBy, x.RoleId)).ToList();
        }

        public UserModel GetUser(string email, string password)
        {
            UserData user = _userRepository.GetUser(email, password);
            return null;
            //return user != null ? new UserModel(user.Id, user.UserCode, user.CreatedDate, user.FirstName, user.LastName, user.IsAdmin, user.Email, user.Password, user.IsActive, user.ModifiedDate, user.ModifiedBy, user.CreatedBy, user.RoleId) : null;
        }
        public void SaveUpdateUser(UserModel model)
        {
            UserData obj = new UserData(model.Id, model.UserCode, model.CreatedDate, model.FirstName, model.LastName, model.IsAdmin, model.Email, model.Password, model.IsActive, null, model.FirstName, model.CreatedBy, model.RoleId);
            _userRepository.SaveUpdateUser(obj);
        }

        //public void SaveUpdateBranch(BranchModel model)
        //{
        //    BranchData obj = new BranchData(model.Id, model.BranchCode, model.BranchName, model.CompanyCode, model.Description, model.Address, model.IsDefault, model.IsActive, model.CreatedDate, model.ModifiedDate, model.ModifiedBy, model.CreatedBy);
        //    _companyRepository.SaveUpdateBranch(obj);
        //}

        //        public IList<CompanyVM> GetCompanyWithBranches()
        //        {
        //            var dd = _companyRepository.GetCompanies();
        //            dd.ToList().ForEach(x =>
        //            {
        //                new CompanyVM(new CompanyModel(x.Name, x.Code, x.PhoneNo, x.Description, x.Logo),

        //null);
        //            });
        //            return null;
        //        }
        public bool IsExist()
        {
            return true;
        }
        public IList<ProductModel> GetProducts()
        {
            List<Product> products = _productRepository.GetProducts();
            return null;
           // return products.ToList().Select(x => new ProductModel(x.ItemName)).ToList();
        }

        public bool DeleteProduct(int id)
        {
            var result = _purchaseRepository.IsProductExist(id);
            return true;
        }
        public LabelSettingModel GetLabelData(int ItemId)
        {
            LabelSetting labelData = _labelSettingRepository.GetLabelData(ItemId);
            return new LabelSettingModel(labelData.Id, labelData.LabelSettingCode, labelData.ItemId, labelData.PrintItemCode, labelData.PrintItemDetail, labelData.PrintUnitMeasure, labelData.PrintItemPrice, labelData.PrintBarCode, labelData.BarCodeHeight);
        }
        public void UpdateLabel(LabelSettingModel model)
        {
            LabelSetting labelData = new LabelSetting(model.Id, model.LabelSettingCode, model.ItemId, model.PrintItemCode, model.PrintItemDetail, model.PrintUnitMeasure, model.PrintItemPrice, model.PrintBarCode, model.BarCodeHeight);
            _labelSettingRepository.UpdateLabel(labelData);
        }
    }
}

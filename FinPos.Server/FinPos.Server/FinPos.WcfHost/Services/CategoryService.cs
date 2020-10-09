using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
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
    [ServiceBehavior(Name = "CategoryService", Namespace = "http://locahost:8080/CategoryService", InstanceContextMode = InstanceContextMode.Single)]
    public class CategoryService : ICategoryService
    {
        #region Properties
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        FaultData fault = new FaultData();
        #endregion
        
        #region Constructor
        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this._categoryRepository = categoryRepository;
            this._productRepository = productRepository;
        }
        #endregion

        #region Category Service Methods
        public IList<CategoryModel> GetCategories()
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategories();
                return categories.Select(x => new CategoryModel(
                    x.Id,
                    x.CategoryName,
                    x.Description,
                    x.BranchCode,
                    x.IsDeleted,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.ModifiedDate,
                    x.ModifiedBy, x.IsActive, x.IsActive ? "Active" : "In Active", x.CompanyCode
                    )).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public IList<CategoryModel> GetCategoriesByCompanyId(int companyId, int? branchId)
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategoriesByCompanyId(companyId, branchId);
                return categories.Select(x => new CategoryModel(
                    x.Id,
                    x.CategoryName,
                    x.Description,
                    x.BranchCode,
                    x.IsDeleted,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.ModifiedDate,
                    x.ModifiedBy, x.IsActive, x.IsActive ? "Active" : "In Active", x.CompanyCode
                    )).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }


        public void SaveUpdateCategory(CategoryModel model)
        {
            try
            {
                Category category = new Category();
                if (model.Id > 0)
                {
                    category = _categoryRepository.GetCategories().FirstOrDefault(x => x.Id == model.Id);
                    category.CreatedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                }
                else
                {
                    category.CreatedDate = model.CreatedDate;
                    category.ModifiedBy = model.ModifiedBy;
                    category.ModifiedDate = model.ModifiedDate;

                }
                category.IsDeleted = model.IsDeleted;
                category.IsActive = model.IsActive;
                category.BranchCode = model.BranchCode;
                category.CategoryName = model.CategoryName;
                category.Description = model.Description;
                category.CreatedBy = model.Createdby;
                category.CompanyCode = model.CompanyCode;
                _categoryRepository.SaveUpdateCategory(category);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Category";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                var result = _productRepository.IsCategoryExist(id);
                if (result == false)
                {
                    Category category = _categoryRepository.GetCategories().FirstOrDefault(x => x.Id.Value == id);
                    _categoryRepository.DeleteCategory(category);
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
        #endregion
    }
}

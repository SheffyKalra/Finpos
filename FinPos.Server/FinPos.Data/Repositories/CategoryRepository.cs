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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IEntityProvider<Category> _categoryProvider;
        public CategoryRepository(IEntityProvider<Category> categoryProvider)
        {
            _categoryProvider = categoryProvider;
        }
        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            categories = _categoryProvider.Get().ToList();
            return categories;
        }
        public List<Category> GetCategoriesByCompanyId(int companyid, int? branchId)
        {
            List<Category> categories = new List<Category>();
            categories = _categoryProvider.Get().Where(x => x.CompanyCode == companyid && x.BranchCode == branchId).ToList();
            return categories;
        }
        public void SaveUpdateCategory(Category category)
        {
            if (category.Id > 0)
                this._categoryProvider.Update(category);
            else
                this._categoryProvider.Insert(category);
        }

        public void DeleteCategory(Category category)
        {
            this._categoryProvider.Delete(category);
        }
    }
}

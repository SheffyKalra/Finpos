using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        List<Category> GetCategoriesByCompanyId(int companyid, int? branchId);
        void SaveUpdateCategory(Category category);

        void DeleteCategory(Category category);
    }
}

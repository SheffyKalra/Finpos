using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Interface
{
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        IList<CategoryModel> GetCategories();

        [OperationContract]
        IList<CategoryModel> GetCategoriesByCompanyId(int companyId, int? branchId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void SaveUpdateCategory(CategoryModel model);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeleteCategory(int id);
    }
}

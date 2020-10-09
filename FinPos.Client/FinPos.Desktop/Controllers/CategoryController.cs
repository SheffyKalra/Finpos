using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using System.Linq;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class CategoryController
    {
        IServiceEndpoints objServiceEndPoints = new ServiceEndPoints.ServiceEndPoints();

        #region Getter Methods
        public IList<CategoryModel> GetCategories()
        {
            try
            {
                return objServiceEndPoints.CategoryServiceInstance().GetCategories();
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objServiceEndPoints.CategoryServiceInstanceClosed();
            }
        }
        public IList<CategoryModel> GetCategoriesByCompanyId()
        {
            try
            {
                return objServiceEndPoints.CategoryServiceInstance().GetCategoriesByCompanyId(UserModelVm.CompanyId, UserModelVm.BranchId);
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objServiceEndPoints.CategoryServiceInstanceClosed();
            }
        }
        #endregion

        #region CRUD Operations
        /// <summary>
        /// Save or Update Category
        /// </summary>
        /// <param name="model">Category Details</param>
        /// <returns></returns>
        public ResponseVm SaveUpdateCategory(CategoryModel model)
        {
            try
            {
                objServiceEndPoints.CategoryServiceInstance().SaveUpdateCategory(model);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objServiceEndPoints.CategoryServiceInstanceClosed();
            }
        }
        public bool DeleteCategory(int id)
        {
            try
            {
                var result = objServiceEndPoints.CategoryServiceInstance().DeleteCategory(id);
                return result;//new ResponceVm(null, new List<object>(result == true ? 1 : 0));
            }
            catch (FaultException<FaultData> e)
            {
                return false;// new ResponceVm(e, null); ;
            }
            finally
            {
                objServiceEndPoints.CategoryServiceInstanceClosed();
            }
            #endregion

        }
    }
}

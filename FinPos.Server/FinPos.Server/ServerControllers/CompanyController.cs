using FinPos.Domain.DataContracts;
//using FinPos.Domain.ServiceContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.Server.ServiceEndPoints.Interface;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Server
{
    public class CompanyController
    {
        #region Properties
        IServiceEndPoints objCompanyService = new ServiceEndPoints.ServiceEndPoints();
        #endregion

        #region Getter Methods
        public ResponseVm GetCompanies()
        {
            try
            {
                IList<CompanyModel> lstCustomers = objCompanyService.CompanyServiceInstance().GetCompanies();
                return new ResponseVm(null, lstCustomers.Cast<object>().ToList());
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objCompanyService.CompanyServiceInstanceClosed();
            }
        }
        #endregion
        
        #region CRUD Operations
        public int SaveUpdateCompany(CompanyModel model)
        {
            try
            {
                return objCompanyService.CompanyServiceInstance().SaveUpdateCompany(model);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objCompanyService.CompanyServiceInstanceClosed();
            }
        }
        public void SaveUpdateBranch(BranchModel model)
        {
            try
            {
                objCompanyService.CompanyServiceInstance().SaveUpdateBranch(model);
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objCompanyService.CompanyServiceInstanceClosed();
            }
        }
        #endregion
    }
}


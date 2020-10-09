using System.Collections.Generic;
using System.Linq;
using FinPos.Domain.DataContracts;
using System.ServiceModel;
using FinPos.DomainContracts.DataContracts;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class CompanyController
    {
        #region Properties
        IServiceEndpoints objCouponService = new ServiceEndPoints.ServiceEndPoints();
        #endregion

        #region Getter Methods
        public IList<CompanyModel> GetCompanies()
        {
            try
            {
                return objCouponService.CompanyServiceInstance().GetCompanies();
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }
        }
        public IList<CompanyModel> GetCompanyById(int companyId)
        {
            return objCouponService.CompanyServiceInstance().GetCompanyById(companyId);
        }
        public ResponseVm GetCompanyBranches(int companyId)
        {
            try
            {
                IList<BranchModel> lstCustomers = objCouponService.CompanyServiceInstance().GetCompanyBranches(companyId).ToList();
                return new ResponseVm(null, new List<object>(lstCustomers).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null); ;
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }
        }

        public IList<CompanyModel> GetActiveCompanies()
        {
            try
            {
                return objCouponService.CompanyServiceInstance().GetActiveCompanies();
            }
            catch (FaultException<FaultData> e)
            {
                throw e;
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }
        }
        public ResponseVm GetCompanyActiveBranches(int companyId)
        {
            try
            {
                IList<BranchModel> lstCustomers = objCouponService.CompanyServiceInstance().GetCompanyActiveBranches(companyId).ToList();
                return new ResponseVm(null, new List<object>(lstCustomers).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }
        }
        #endregion

        #region CRUD Operation
        public ResponseVm SaveUpdateCompany(CompanyModel model)
        {
            try
            {
                objCouponService.CompanyServiceInstance().SaveUpdateCompany(model);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }
        }
        public ResponseVm SaveUpdateBranch(BranchModel model)
        {
            try
            {
                objCouponService.CompanyServiceInstance().SaveUpdateBranch(model);
                return new ResponseVm(null, null);
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objCouponService.CompanyServiceInstanceClosed();
            }

        }
        #endregion

        #region Commented Code
        //public IList<CompanyVM> GetCompanyWithBranches()
        //{
        //    IList<CompanyVM> lstCustomers;
        //    ChannelFactory<IFinPosService> myChannelFactory = new ChannelFactory<IFinPosService>(tcpBindings, myEndpoint);
        //    IFinPosService instance = myChannelFactory.CreateChannel();
        //   // lstCustomers = instance.GetCompanyWithBranches();
        //    return null;
        //}
        #endregion
    }
}

using FinPos.Client.ServiceEndPoints.Interface;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.Controllers
{
    public class SupplierController
    {
        IServiceEndpoints objSupplierService = new ServiceEndPoints.ServiceEndPoints();
        public IList<SupplierModel> GetSuppliers()
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetSuppliers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public void SaveUpdateSupplier(SupplierModel model)
        {
            try
            {
                objSupplierService.SupplierServiceInstance().SaveUpdateSupplier(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public void SaveUpdatePayment(PaymentToSupplierModel model)
        {
            try
            {
                objSupplierService.SupplierServiceInstance().SaveUpdatePayment(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }

        public void DeleteSupplier(int id)
        {
            try
            {
                objSupplierService.SupplierServiceInstance().DeleteSupplier(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public void DeletePayment(int id)
        {
            try
            {
                objSupplierService.SupplierServiceInstance().DeletePayment(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }


        public IList<SupplierModel> GetSuppliersByCompanyAndBrach(int companyId, int? branchId)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetSuppliersByCompanyAndBrach(companyId, branchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public SupplierModel GetSuppliersBySupplierCode(int companyId, int? branchId,int supplierCode)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetSuppliersBySupplierCode(companyId, branchId,supplierCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentsByPaymentTypeAndInvoiceNo(int companyId, int? branchId, int invoiceNo, int purchaseType)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetPaymentsByPaymentTypeAndInvoiceNo(companyId, branchId, invoiceNo, purchaseType);

            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }

        public ResponseVm GetPaymentsByCompanyIdAndBranchId(int companyId, int? branchId)
        {
            try
            {
                IList<PaymentToSupplierModel> payments = objSupplierService.SupplierServiceInstance().GetPaymentsByCompanyIdAndBranchId(companyId, branchId);
                return new ResponseVm(null, new List<dynamic>(payments).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentByDateFilter(int companyId, int? branchId, string fromDate, string toDate)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetPaymentByDateFilter(companyId, branchId, fromDate, toDate);
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentBySupplierCode(int companyId, int? branchId, int suplierCode)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetPaymentBySupplierCode(companyId, branchId, suplierCode);
                // new ResponseVm(null, new List<dynamic>(payments).ToList());
            }
            catch (FaultException<FaultData> ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }
        }

        public int? GetSupplierIdByName(string txtSupplierName)
        {
            try
            {
                return objSupplierService.SupplierServiceInstance().GetSupplierIdByName(txtSupplierName, UserModelVm.CompanyId, UserModelVm.BranchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSupplierService.SupplierServiceInstanceClosed();
            }

        }
    }
}

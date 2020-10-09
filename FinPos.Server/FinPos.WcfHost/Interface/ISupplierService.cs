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
    public interface ISupplierService
    {
        [OperationContract]
        IList<SupplierModel> GetSuppliers();
        [OperationContract]
        void SaveUpdateSupplier(SupplierModel model);
        [OperationContract]
        void SaveUpdatePayment(PaymentToSupplierModel model);
        [OperationContract]
        void DeleteSupplier(int id);
        [OperationContract]
        void DeletePayment(int id);
        [OperationContract]
        IList<SupplierModel> GetSuppliersByCompanyAndBrach(int companyId, int? branchId);
        [OperationContract]
        SupplierModel GetSuppliersBySupplierCode(int companyId, int? branchId,int supplierCode);
        [OperationContract]
        IList<PaymentToSupplierModel> GetPaymentsByPaymentTypeAndInvoiceNo(int companyId, int? branchId, int invoiceNo, int purchaseType);
        [OperationContract]
        IList<PaymentToSupplierModel> GetPaymentsByCompanyIdAndBranchId(int companyId, int? branchId);
        [OperationContract]
        IList<PaymentToSupplierModel> GetPaymentByDateFilter(int companyId, int? branchId, string fromDate, string toDate);
        [OperationContract]
        IList<PaymentToSupplierModel> GetPaymentBySupplierCode(int companyId, int? branchId, int supplierCode);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        int? GetSupplierIdByName(string txtSupplierName, int companyId, int? branchId);
    }
}

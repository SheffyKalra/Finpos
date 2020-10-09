using FinPos.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Interfaces
{
    public interface ISupplierRepository
    {
        void SaveUpdateSupplier(Supplier model);
        void SaveUpdatePayment(PaymentToSupplier model);
        List<Supplier> GetSupplier();
        Supplier GetSupplierData(int id);
        PaymentToSupplier GetPaymentById(int id);
        List<PaymentToSupplier> GetPaymentByInvoiceNo(int id);
        void DeleteSupplier(Supplier model);
        void DeletePayment(PaymentToSupplier payment);
        List<Supplier> GetSuppliersByCompanyAndBrach(int companyId, int? branchId);
        Supplier GetSuppliersBySupplierCode(int companyId, int? branchId,int supplierCode);
        List<PaymentToSupplier> GetPaymentByDateFilter(int companyId, int? branchId, string fromDate, string toDate);
        List<PaymentToSupplier> GetPaymentBySupplierCode(int companyId, int? branchId, int supplierCode);
        List<PaymentToSupplier> GetPaymentsByPaymentTypeAndInvoiceNo(int companyId, int? branchId, int invoiceNo, int purchaseType);
        Supplier GetSuppliersBySupplierId(int companyId, int? branchId, int supplierId);
        List<PaymentToSupplier> GetPaymentsByCompanyIdAndBranchId(int companyId, int? branchId);
        int? GetSupplierIdByName(string txtSupplierName,int companyId, int? branchId);
    }
}

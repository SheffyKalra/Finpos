using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IEntityProvider<Supplier> _supplierProvider;
        private readonly IEntityProvider<PaymentToSupplier> _paymentToSupplierProvider;
        public SupplierRepository(IEntityProvider<Supplier> supplierProvider, IEntityProvider<PaymentToSupplier> paymentToSupplierProvider)
        {
            this._supplierProvider = supplierProvider;
            this._paymentToSupplierProvider = paymentToSupplierProvider;
        }
        public List<Supplier> GetSuppliersByCompanyAndBrach(int companyId, int? branchId)
        {
            return _supplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId).ToList();
        }
        public Supplier GetSuppliersBySupplierCode(int companyId, int? branchId, int supplierCode)
        {
            return _supplierProvider.Get().FirstOrDefault(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && supplier.Id == supplierCode);
        }
        public List<PaymentToSupplier> GetPaymentsByPaymentTypeAndInvoiceNo(int companyId, int? branchId, int invoiceNo, int purchaseType)
        {
            return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && supplier.InvoiceNo == invoiceNo && supplier.PurchaseType == purchaseType).ToList();
        }
        public List<PaymentToSupplier> GetPaymentByDateFilter(int companyId, int? branchId, string fromDate, string toDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && CommonFunctions.ParseDateToFinclave(supplier.CreatedDate).Date <= CommonFunctions.ParseDateToFinclave(toDate).Date).ToList();
            }
            else if (string.IsNullOrWhiteSpace(toDate) && !string.IsNullOrWhiteSpace(fromDate))
            {
                return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && CommonFunctions.ParseDateToFinclave(supplier.CreatedDate).Date >= CommonFunctions.ParseDateToFinclave(fromDate).Date && CommonFunctions.ParseDateToFinclave(supplier.CreatedDate).Date <= CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()).Date).ToList();
            }
            else
                return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && CommonFunctions.ParseDateToFinclave(supplier.CreatedDate).Date >= CommonFunctions.ParseDateToFinclave(fromDate).Date && CommonFunctions.ParseDateToFinclave(supplier.CreatedDate).Date <= CommonFunctions.ParseDateToFinclave(toDate).Date).ToList();
        }
        public List<PaymentToSupplier> GetPaymentBySupplierCode(int companyId, int? branchId, int supplierCode)
        {
            return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && supplier.SupplierCode == supplierCode).ToList();
        }
        public Supplier GetSuppliersBySupplierId(int companyId, int? branchId, int supplierId)
        {
            return _supplierProvider.Get().FirstOrDefault(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId && supplier.Id == supplierId);
        }
        public List<PaymentToSupplier> GetPaymentsByCompanyIdAndBranchId(int companyId, int? branchId)
        {
            return _paymentToSupplierProvider.Get().Where(supplier => supplier.CompanyCode == companyId && supplier.BranchCode == branchId).ToList();
        }
        public int? GetSupplierIdByName(string txtSupplierName, int companyId, int? branchId)
        {
            if (this._supplierProvider.Any(item => item.SupplierName == txtSupplierName && item.CompanyCode == companyId && item.BranchCode == branchId))
            {
                return this._supplierProvider.Get().Where(item => item.SupplierName == txtSupplierName && item.CompanyCode == companyId && item.BranchCode == branchId).FirstOrDefault().Id;
            }
            else
            {
                return null;
            }
        }
        public List<Supplier> GetSupplier()
        {
            return _supplierProvider.Get().ToList();
        }
        public void SaveUpdateSupplier(Supplier model)
        {
            if (model.Id > 0)
                this._supplierProvider.Update(model);
            else
                this._supplierProvider.Insert(model);
        }
        public void SaveUpdatePayment(PaymentToSupplier model)
        {
            if (model.Id > 0)
                this._paymentToSupplierProvider.Update(model);
            else
                this._paymentToSupplierProvider.Insert(model);
        }
        public Supplier GetSupplierData(int id)
        {
            return _supplierProvider.Get().FirstOrDefault(x => x.Id == id);
        }
        public PaymentToSupplier GetPaymentById(int id)
        {
            return _paymentToSupplierProvider.Get().FirstOrDefault(x => x.Id == id);
        }
        public List<PaymentToSupplier> GetPaymentByInvoiceNo(int id)
        {
            return _paymentToSupplierProvider.Get().Where(x => x.InvoiceNo == id).ToList();
        }
        public void DeleteSupplier(Supplier model)
        {
            _supplierProvider.Delete(model);
        }
        public void DeletePayment(PaymentToSupplier payment)
        {
            _paymentToSupplierProvider.Delete(payment);
        }

    }
}

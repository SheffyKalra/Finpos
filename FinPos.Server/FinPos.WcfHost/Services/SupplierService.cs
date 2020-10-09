using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "SupplierService", Namespace = "http://locahost:8080/SupplierService", InstanceContextMode = InstanceContextMode.Single)]
    public class SupplierService : ISupplierService
    {
        #region Properties
        private readonly ISupplierRepository _supplierRepository;
        FaultData fault = new FaultData();
        #endregion

        #region Constructor
        public SupplierService(ISupplierRepository supplierRepository)
        {
            this._supplierRepository = supplierRepository;
        }
        #endregion

        #region Getter Methods
        public IList<SupplierModel> GetSuppliers()
        {
            try
            {
                List<Supplier> list = new List<Supplier>();
                return _supplierRepository.GetSupplier().Select(x => new SupplierModel(x.Id, x.SupplierName, x.ShortName, x.Address, x.ContactName, x.Telephone, x.Mobile, x.Fax, x.WebsiteUrl, x.Email, x.Notes, x.DiscountPercentage, x.CompanyCode, Convert.ToInt32(x.BranchCode))).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetSuppliers method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<SupplierModel> GetSuppliersByCompanyAndBrach(int companyId, int? branchId)
        {
            try
            {

                ///List<Supplier>   list = new List<Supplier>();
                return _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId).Select(x => new SupplierModel(x.Id, x.SupplierName, x.ShortName, x.Address, x.ContactName, x.Telephone, x.Mobile, x.Fax, x.WebsiteUrl, x.Email, x.Notes, x.DiscountPercentage, x.CompanyCode, Convert.ToInt32(x.BranchCode))).ToList();
                // return list.Select(x => new SupplierModel(x.Id, x.SupplierName, x.ShortName, x.Address, x.ContactName, x.Telephone, x.Mobile, x.Fax, x.WebsiteUrl, x.Email, x.Notes, x.DiscountPercentage, x.CompanyCode, Convert.ToInt32(x.BranchCode))).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetSuppliersByCompanyAndBrach method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public SupplierModel GetSuppliersBySupplierCode(int companyId, int? branchId,int supplierCode)
        {
            try
            {
                Supplier supplier = _supplierRepository.GetSuppliersBySupplierCode(companyId, branchId, supplierCode);
                return new SupplierModel(supplier.Id, supplier.SupplierName, supplier.ShortName, supplier.Address, supplier.ContactName, supplier.Telephone, supplier.Mobile, supplier.Fax, supplier.WebsiteUrl, supplier.Email, supplier.Notes, supplier.DiscountPercentage, supplier.CompanyCode, Convert.ToInt32(supplier.BranchCode));
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetSuppliersBySupplierCode method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentsByPaymentTypeAndInvoiceNo(int companyId, int? branchId, int invocieNo, int purchaseType)
        {
            try
            {
                List<PaymentToSupplier> list = new List<PaymentToSupplier>();
                List<Supplier> listOfSupplier = new List<Supplier>();
                listOfSupplier = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId);
                return _supplierRepository.GetPaymentsByPaymentTypeAndInvoiceNo(companyId, branchId, invocieNo, purchaseType).Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
                //// return list.Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPaymentsByPaymentTypeAndInvoiceNo method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentsByCompanyIdAndBranchId(int companyId, int? branchId)
        {
            try
            {
                List<PaymentToSupplier> list = new List<PaymentToSupplier>();
                List<Supplier> listOfSupplier = new List<Supplier>();
                listOfSupplier = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId);
                return _supplierRepository.GetPaymentsByCompanyIdAndBranchId(companyId, branchId).Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
                //  return list.Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPaymentsByCompanyIdAndBranchId method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentByDateFilter(int companyId, int? branchId, string fromDate, string toDate)
        {
            try
            {
                List<PaymentToSupplier> list = new List<PaymentToSupplier>();
                List<Supplier> listOfSupplier = new List<Supplier>();
                listOfSupplier = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId);
                list = _supplierRepository.GetPaymentByDateFilter(companyId, branchId, fromDate, toDate);
                return list.Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPaymentByDateFilter method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<PaymentToSupplierModel> GetPaymentBySupplierCode(int companyId, int? branchId, int supplierCode)
        {
            try
            {
                List<PaymentToSupplier> list = new List<PaymentToSupplier>();
                List<Supplier> listOfSupplier = new List<Supplier>();
                listOfSupplier = _supplierRepository.GetSuppliersByCompanyAndBrach(companyId, branchId);
                return _supplierRepository.GetPaymentBySupplierCode(companyId, branchId, supplierCode).Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
                /// return list.Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, listOfSupplier.FirstOrDefault(z => z.Id == x.SupplierCode).SupplierName, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetPaymentBySupplierCode method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public int? GetSupplierIdByName(string txtSupplierName, int companyId, int? branchId)
        {
            try
            {
                return _supplierRepository.GetSupplierIdByName(txtSupplierName, companyId, branchId);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetSupplierIdByName method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion

        #region CRUD Operations
        public void SaveUpdateSupplier(SupplierModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var obj = _supplierRepository.GetSupplier().FirstOrDefault(x => x.Id == model.Id);
                    obj.SupplierName = model.SupplierName;
                    obj.ShortName = model.ShortName;
                    obj.Address = model.Address;
                    obj.ContactName = model.ContactName;
                    obj.Telephone = model.Telephone;
                    obj.Mobile = model.Mobile;
                    obj.Fax = model.Fax;
                    obj.WebsiteUrl = model.WebsiteUrl;
                    obj.Email = model.Email;
                    obj.Notes = model.Notes;
                    obj.DiscountPercentage = model.DiscountPercentage;
                    obj.Tax = model.Tax;
                    obj.TaxInclusive = model.TaxInclusive;
                    //obj = new CompanyData(model.Id, model.Code.Value, model.Name, model.Description, model.PhoneNo, model.Logo, model.IsDefault, true, model.CreatedDate, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                    _supplierRepository.SaveUpdateSupplier(obj);
                }
                else
                {
                    Supplier newModel = new Supplier(model.SupplierName, model.ShortName, model.Address, model.ContactName, model.Telephone, model.Mobile, model.Fax, model.WebsiteUrl, model.Email, model.Notes, model.DiscountPercentage, model.Tax, model.TaxInclusive, model.CompanyCode, model.BranchCode);
                    _supplierRepository.SaveUpdateSupplier(newModel);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateSupplier method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveUpdatePayment(PaymentToSupplierModel model)
        {
            try
            {


                if (model.PaymentTosupplierId > 0)
                {
                    var obj = _supplierRepository.GetPaymentsByCompanyIdAndBranchId(model.CompanyCode, model.BranchCode).FirstOrDefault(x => x.Id == model.PaymentTosupplierId);
                    obj.AccountNo = model.AccountNo;
                    obj.Amount = model.Amount;
                    obj.BankName = model.BankName;
                    obj.CompanyCode = obj.CompanyCode;
                    obj.BranchCode = obj.BranchCode;
                    obj.CreatedBy = obj.CreatedBy;
                    obj.CreatedDate = obj.CreatedDate;
                    obj.Description = model.Description;
                    obj.InvoiceNo = model.InvoiceNo;
                    obj.ModifiedBy = model.ModifiedBy;
                    obj.ModifiedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    obj.PaymentDate = model.PaymentDate;
                    obj.PaymentType = model.PaymentType;
                    obj.SupplierCode = model.SupplierCode;
                    obj.Id = obj.Id;
                    obj.PurchaseType = model.PurchaseType;
                    //obj = new CompanyData(model.Id, model.Code.Value, model.Name, model.Description, model.PhoneNo, model.Logo, model.IsDefault, true, model.CreatedDate, model.UpdatedDate, model.ModifiedBy, model.CreatedBy);
                    _supplierRepository.SaveUpdatePayment(obj);
                }
                else
                {
                    PaymentToSupplier newModel = new PaymentToSupplier(model.PaymentTosupplierId, model.SupplierCode, model.Amount, model.PaymentDate, model.Description, model.InvoiceNo, model.AccountNo, model.CreatedBy, model.CreatedDate, model.ModifiedBy, model.ModifiedDate, model.PaymentType, model.BankName, model.CompanyCode, model.BranchCode, model.PurchaseType);
                    _supplierRepository.SaveUpdatePayment(newModel);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateSupplier method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void DeleteSupplier(int id)
        {
            try
            {
                Supplier supplier = new Supplier();
                supplier = _supplierRepository.GetSupplierData(id);
                _supplierRepository.DeleteSupplier(supplier);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteSupplier method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void DeletePayment(int id)
        {
            try
            {
                PaymentToSupplier payment = new PaymentToSupplier();
                payment = _supplierRepository.GetPaymentById(id);
                _supplierRepository.DeletePayment(payment);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeletePayment method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion
        
    }
}

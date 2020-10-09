using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Domain.DataContracts;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.ServiceModel.Channels;
using Ninject;
using System.Reflection;
using FinPos.DAL.Repositories;
using FinPos.Data.Repositories;
//using FinPos.Utility.CommonEnums;
//using FinPos.Utility.CommonMethods;
using System.Globalization;
using System.ComponentModel;
using FinPos.Utility.CommonEnums;

namespace FinPos.WcfHost
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(Name = "FinPosService", Namespace = "http://locahost:8080/finposservice", InstanceContextMode = InstanceContextMode.Single)]
    public class FinPosService : IFinPosService
    {
        private readonly ICompanyRepository _companyRepository;
       
        // private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly ILabelSettingRepository _labelSettingRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IOpeningStockRepository _openingStockRepository;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        
        private readonly ITaxRepository _taxRepository;
      
        FaultData fault = new FaultData();

        public FinPosService(ICompanyRepository companyRepository, IBranchRepository branchRepository, IUserRepository userRepository, IProductRepository productRepository, IPurchaseRepository purchaseRepository, ICategoryRepository categoryRepository, ILabelSettingRepository labelSettingRepository, ISupplierRepository supplierRepository, IOpeningStockRepository openingStockRepository, IStockAdjustmentRepository stockAdjustmentRepository, ISystemConfigurationRepository systemConfigurationRepository, ITaxRepository taxRepository)
        {
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _purchaseRepository = purchaseRepository;
            _labelSettingRepository = labelSettingRepository;
            _supplierRepository = supplierRepository;
            _openingStockRepository = openingStockRepository;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            _systemConfigurationRepository = systemConfigurationRepository;
            _taxRepository = taxRepository;
          
        }
       
      
        
        
        
        
        

       
        

        

        

        
        public IList<PaymentToSupplierModel> GetPaymentByInvoiceNo(int id)
        {
            return _supplierRepository.GetPaymentByInvoiceNo(id).Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, string.Empty, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
            /// return list.Select(x => new PaymentToSupplierModel(x.Id, x.SupplierCode, x.Amount, x.PaymentDate, x.Description, x.InvoiceNo, x.AccountNo, x.CreatedBy, x.CreatedDate, x.ModifiedBy, x.ModifiedDate, x.PaymentType, x.BankName, x.CompanyCode, x.BranchCode, string.Empty, Convert.ToString((CommonEnum.PaymentType)x.PaymentType), x.PurchaseType)).ToList();
        }
     
       
    
      

        


        
        
        public bool CheckProductOpningStock(long productCode)
        {
            bool exists = false;
            List<Purchase> _directpurchases = _purchaseRepository.GetDirectPurchases().ToList();
            int?[] _directpurchasesIDs = _directpurchases.Select(x => x.Id).ToArray();
            int?[] _purchaseFromSupplier = _purchaseRepository.GetPurchases().Where(x => x.Status != 2).Select(x => x.Id).ToArray(); /*temprary code 2 in this line (int)CommonEnum.PurchaseStatus.WaitingForApproval*/
            List<OpeningStock> _openingStock = _openingStockRepository.GetOpeningStockByProductCode().Where(x => x.ProductCode == productCode).ToList();
            List<Stock> _stock = _purchaseRepository.GetStocks().Where(x => (x.ProductCode == productCode && x.PurchaseId != null && _directpurchasesIDs.Contains(x.PurchaseId)) || ((x.ProductCode == productCode && _purchaseFromSupplier.Contains(x.PurchaseOrderId)))).ToList();

            if (_openingStock.Count > 0 && _stock.Count > 0)
            {
                exists = true;
            }
            return exists;
        }
      
   

      
        

       
   
        public void UpdateExistDefaultCompany(int? id)
        {
            try
            {
                FinPos.DAL.Entities.Company IsDefaulltExist = new Company();
                if (id.Value > 0)
                {
                    IsDefaulltExist = _companyRepository.GetCompanies().FirstOrDefault(x => x.Id != id && x.IsDefault);
                }
                else
                {
                    IsDefaulltExist = _companyRepository.GetCompanies().FirstOrDefault(x => x.IsDefault);
                }
                if (IsDefaulltExist != null)
                {
                    IsDefaulltExist.IsDefault = false;
                    _companyRepository.SaveUpdateCompany(IsDefaulltExist);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateExistDefaultCompany method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
       
 
       
        public void UpdateExistDefaultBranch(int companyId, int banchId)
        {
            try
            {
                List<Branch> isDefaulltExist = new List<Branch>();
                if (companyId > 0 && banchId > 0)
                {
                    isDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.Id != banchId).ToList();

                }
                else
                {
                    isDefaulltExist = _branchRepository.GetCompanyBranches(companyId).Where(x => x.IsDefault).ToList();

                }
                foreach (var obj in isDefaulltExist)
                {
                    obj.IsDefault = false;
                    _branchRepository.SaveUpdateBranch(obj);
                }
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateExistDefaultBranch method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public IList<UserModel> GetUsers()
        {
            try
            {
                List<User> users = _userRepository.GetUsers();
                return users.ToList().Select(x => new UserModel(x.Id, x.UserCode, x.CreatedDate, x.FirstName, x.LastName, x.IsAdmin, x.Email, x.Password, x.IsActive, x.ModifiedDate, x.ModifiedBy, x.CreatedBy, x.RoleId)).ToList();

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetUsers method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        
        public void SaveUpdateUser(UserModel model)
        {

            User obj = new User(model.Id, model.UserCode, model.CreatedDate, model.FirstName, model.LastName, model.IsAdmin, model.Email, model.Password, model.IsActive, null, model.FirstName, model.CreatedBy, model.RoleId);
            _userRepository.SaveUpdateUser(obj);

        }


        //public bool IsExist()
        //{
        //    return true;
        //}

        public IList<CategoryModel> GetCategories()
        {


            try
            {
                List<Category> categories = _categoryRepository.GetCategories();
                return categories.Select(x => new CategoryModel(
                    x.Id,
                    x.CategoryName,
                    x.Description,
                    x.BranchCode,
                    x.IsDeleted,
                    x.CreatedDate,
                    x.CreatedBy,
                  x.ModifiedDate,
                    x.ModifiedBy, x.IsActive, x.IsActive ? "Active" : "In Active", x.CompanyCode
                    )).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetCategories method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
      
    
        //OPTIMIZED..
       

     
   
      

      

        
        
    
       

       
   
      
        
   
        
        //Need to Check update case
       

    

        
        
        

        
        

        

        
        
       

        

        
        //public List<StockModel> GetStocksById(int purchaseId, int companyId, int? branchId)
        // {
        //     List<Stock> stocks = _purchaseRepository.GetStocksById(purchaseId)
        //     return stocks?.Select(x => new StockModel(x.Id, x.PurchaseId, x.Quantity, x.CostPrice, x.SellingPrice, x.MRP, x.ItemTaxPercentage, x.BatchNo, x.ProductCode, x.PurchaseOrderId)).ToList();
        // }


        

        

        

        

        

        
     
        //Optimized
   
        

   
     
        public void SaveUpdateSubProductItems(List<SubProductItemModel> model, List<SubProductItemModel> deleteProducts)
        {
            List<SubProductItem> insertSubProductItem = new List<SubProductItem>();
            List<SubProductItem> updateSubProductItem = new List<SubProductItem>();
            List<SubProductItem> deleteSubProductItem = new List<SubProductItem>();
            model?.ForEach(x =>
            {
                SubProductItem updateStock = new SubProductItem(x.Id > 0 ? x.Id : 0, x.ParentProductId, x.Quantity, x.Retail, UserModelVm.UserId, null, DateTime.Now.ToShortDateString(), string.Empty);
                if (x.Id > 0)
                    updateSubProductItem.Add(updateStock);
                else
                    insertSubProductItem.Add(updateStock);


            });
            ;
            if (deleteProducts.Any())
                deleteSubProductItem.AddRange(deleteProducts.Select(x => new SubProductItem(x.Id, x.ParentProductId, x.Quantity, x.Retail, x.CreatedBy, UserModelVm.UserId, x.CreatedDate, DateTime.Now.ToShortDateString())));
            _productRepository.SaveUpdateSubProductItems(updateSubProductItem, insertSubProductItem, deleteSubProductItem);
        }
        

    





        
        
    }
}

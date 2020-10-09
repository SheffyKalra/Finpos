using FinPos.DAL.Interfaces;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WcfHost.Services
{
    [ServiceBehavior(Name = "ProductService", Namespace = "http://locahost:8080/ProductService", InstanceContextMode = InstanceContextMode.Single)]
    public class ProductService : IProductService
    {
        #region Properties
        private readonly IProductRepository _productRepository;
        FaultData fault = new FaultData();
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ILabelSettingRepository _labelSettingRepository;
        #endregion
    
        #region Constructor
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IPurchaseRepository purchaseRepository, ILabelSettingRepository labelSettingRepository)
        {
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
            this._purchaseRepository = purchaseRepository;
            this._labelSettingRepository = labelSettingRepository;
        }
        #endregion
     
        #region Getter methods
        public IList<ProductModel> GetProductsByCompanyAndBranch(int companyId, int? branchId)
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategories();
                List<Product> products = _productRepository.GetProducts();
                return products.Select(x =>
                    new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholesellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryCode != null ? categories.ToList().FirstOrDefault(z => z.Id.Value == x.CategoryCode).CategoryName : string.Empty, x.ItemType.ToString(), null, x.ImageText, x.CompanyCode, x.BulkCode)
                ).Where(item => item.CompanyCode == companyId && item.BranchCode == branchId).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<ProductModel> GetProducts()
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategories();
                return _productRepository.GetProducts().Select(x =>
                      new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholesellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryCode != null ? categories.ToList().FirstOrDefault(z => z.Id.Value == x.CategoryCode).CategoryName : string.Empty, x.ItemType.ToString(), null, x.ImageText, x.CompanyCode, x.BulkCode)
                  ).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public ProductModel GetProductsById(int productId)
        {
            try
            {

                Product product = _productRepository.GetProductsById(productId);
                return new ProductModel(product.ItemName, product.RetailPrice.Value, product.TradePrice.Value, product.CategoryCode.Value, product.ItemType, product.BarCode, product.TaxPercentage.Value);

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductsById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool IsProductExistInRepack(int productId)
        {
            try
            {
                return _productRepository.IsProductExistInRepack(productId);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in IsProductExistInRepack method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public IList<ProductModel> GetProductsByitemType(int companyId, int? branchId, int itemType)
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategoriesByCompanyId(companyId, branchId);
                return _productRepository.GetProductsByitemType(companyId, branchId, itemType).Select(x =>
                     new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholesellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryCode != null ? categories.ToList().FirstOrDefault(z => z.Id.Value == x.CategoryCode).CategoryName : string.Empty, x.ItemType.ToString(), null, x.ImageText, x.CompanyCode, x.BulkCode)
                 ).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductsByitemType method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public ProductModel GetProductsByBulkCode(int companyId, int? branchId, int bulkCode)
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategoriesByCompanyId(companyId, branchId);
                Product product = _productRepository.GetProductsById(companyId, branchId, bulkCode);
                return new ProductModel(product.Id, product.ItemName, product.CategoryCode, product.RetailPrice, product.TradePrice, product.WholesellerPrice, product.ResellerPrice, product.ItemType, product.Weight, product.BarCode, product.TaxPercentage, product.MinimumLevel, product.ReOrderLevel, product.ItemImage, product.IsTaxInclusive, product.ShortName, product.Description, product.BranchCode, product.CategoryCode != null ? categories.ToList().FirstOrDefault(z => z.Id.Value == product.CategoryCode).CategoryName : string.Empty, product.ItemType.ToString(), null, product.ImageText, product.CompanyCode, product.BulkCode);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductsById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<RepackModel> GetRepackByBulkId(int companyId, int? branchId, int bulkCode)
        {
            try
            {
                List<Category> categories = _categoryRepository.GetCategoriesByCompanyId(companyId, branchId);
                return _productRepository.GetRepackByBulkId(companyId, branchId, bulkCode).Select(x =>
                     new RepackModel(x.Id, x.BulkCode, x.ProductCode, x.Quantity, x.CreatedBy, x.ModifiedDate, x.ModifiedBy, x.CreatedDate)
                 ).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetRepackByBulkId method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<WastageModel> GetWastage()
        {
            try
            {
                IList<WastageModel> wastageList = new List<WastageModel>();
                return _productRepository.GetWastage().Select(x =>
                    new WastageModel(x.Id, x.ItemCode, x.ProductName, x.Quantity, x.CreatedDate, x.Reason, x.BatchNo, x.BranchCode, x.CompanyCode)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetWastage method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public IList<WastageModel> GetWastageWithDateFilter(DateTime startDate, DateTime endDate)
        {
            try
            {
                IList<WastageModel> wastageList = new List<WastageModel>();
                return _productRepository.GetWastageWithDateFilter(startDate, endDate).Select(x => new WastageModel(x.Id, x.ItemCode, x.ProductName, x.Quantity, x.CreatedDate, x.Reason, x.BatchNo, x.BranchCode, x.CompanyCode)
                   ).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetWastageWithDateFilter method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public LabelSettingModel GetLabelData(int ItemId)
        {
            try
            {
                LabelSetting labelData = _labelSettingRepository.GetLabelData(ItemId);
                if (labelData != null)
                {
                    return new LabelSettingModel(labelData.Id, labelData.LabelSettingCode, labelData.ItemId, labelData.PrintItemCode, labelData.PrintItemDetail, labelData.PrintUnitMeasure, labelData.PrintItemPrice, labelData.PrintBarCode, labelData.BarCodeHeight, labelData.LabelSheet, labelData.TotalNoOfPrints, labelData.StartRow, labelData.StartColumn);
                }
                LabelSettingModel newLabelData = new LabelSettingModel();
                newLabelData.ItemId = ItemId;
                return newLabelData;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetLabelData method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public MasterLabelSettingModel GetMasaterLabelSetting()
        {
            try
            {
                MasterLabelSetting masterlabel = _labelSettingRepository.GetMasaterLabelSetting();
                if (masterlabel != null)
                {
                    return new MasterLabelSettingModel(masterlabel.Id, masterlabel.LabelSettingCode, masterlabel.PrintItemCode, masterlabel.PrintItemDetail, masterlabel.PrintUnitMeasure, masterlabel.PrintItemPrice, masterlabel.PrintBarCode, masterlabel.BarCodeHeight, masterlabel.LabelSheet, masterlabel.TotalNoOfPrints, masterlabel.StartRow, masterlabel.StartColumn);
                }
                return null;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetMasaterLabelSetting method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<ProductLabelSettingModel> GetProductLabelSettings(int masterLabelId)
        {
            try
            {
                List<ProductLabelSetting> productlabel = _labelSettingRepository.GetProductLabelSettings(masterLabelId);
                return productlabel?.Select(x => new ProductLabelSettingModel(x.Id.Value, x.MasterLabelSettingCode, x.ProductCode, x.Quantity, string.Empty)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductLabelSettings method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public int? GetProductIdByName(string txtProductName, int companyId, int? bracnchId)
        {
            try
            {
                return _productRepository.GetProductIdByName(txtProductName, companyId, bracnchId);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductIdByName method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }

        public List<ProductItemContentModel> GetProductItemContentById(int productItemContentId)
        {
            try
            {
                return _productRepository.GetProductItemContentsById(productItemContentId).Select(x => new ProductItemContentModel(x.Id.Value, x.SubProductId, x.Quantity, x.MainProductId, x.CreatedBy, x.ModifiedBy, x.CreatedDate, x.ModifiedDate, x.IsFreeProduct)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetProductItemContentById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public List<SubProductItemModel> GetSubProductItemById(int productItemContentId)
        {
            try
            {
                return _productRepository.GetSubProductItemById(productItemContentId).Select(x => new SubProductItemModel(x.Id.Value, x.ParentProductId, x.Quantity, x.RetailPrice, x.CreatedBy, x.ModifiedBy, x.CreatedDate, x.ModifiedDate)).ToList();
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in GetSubProductItemById method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion
     
        #region CRUD Operations
        public void SaveUpdateRepackitems(List<RepackModel> model)
        {
            try
            {
                List<Repack> insertRepackItem = new List<Repack>();
                model?.ForEach(x =>
                {
                    Repack updateStock = new Repack(0, x.BulkCode, x.ProductCode, x.Quantity, x.CreatedBy, string.Empty, null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()));
                    insertRepackItem.Add(updateStock);
                });
                _productRepository.SaveUpdateRepackitems(insertRepackItem);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateRepackitems method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveUpdateProductItemContent(List<ProductItemContentModel> model, List<ProductItemContentModel> deleteProducts)
        {
            try
            {
                List<ProductItemContent> insertProductItemContents = new List<ProductItemContent>();
                List<ProductItemContent> updateProductItemContents = new List<ProductItemContent>();
                List<ProductItemContent> deleteProductItemContents = new List<ProductItemContent>();
                model?.ForEach(x =>
                {
                    ProductItemContent updateStock = new ProductItemContent(x.Id > 0 ? x.Id : 0, x.ChildProductId, x.Quantity, x.ParentProductId, UserModelVm.UserId, null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty, x.IsFreeProduct);
                    if (x.Id > 0)
                        updateProductItemContents.Add(updateStock);
                    else
                        insertProductItemContents.Add(updateStock);
                });
                deleteProductItemContents.AddRange(deleteProducts?.Select(x => new ProductItemContent(x.Id, x.ChildProductId, x.Quantity, x.ParentProductId, x.CreatedBy, UserModelVm.UserId, x.CreatedDate, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), x.IsFreeProduct)));
                _productRepository.SaveUpdateProductItemContent(updateProductItemContents, insertProductItemContents, deleteProductItemContents);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateProductItemContent method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveProductItemContent(List<ProductItemContentModel> model)
        {
            try
            {
                List<ProductItemContent> insertProductItemContents = new List<ProductItemContent>();
                model?.ForEach(x =>
                {
                    ProductItemContent updateStock = new ProductItemContent(x.Id > 0 ? x.Id : 0, x.ChildProductId, x.Quantity, x.ParentProductId, UserModelVm.UserId, null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty, x.IsFreeProduct);
                    insertProductItemContents.Add(updateStock);
                });
                _productRepository.SaveProductItemContent(insertProductItemContents);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveProductItemContent method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveUpdateSubProductItems(List<SubProductItemModel> model, List<SubProductItemModel> deleteProducts)
        {
            try
            {
                List<SubProductItem> insertSubProductItem = new List<SubProductItem>();
                List<SubProductItem> updateSubProductItem = new List<SubProductItem>();
                List<SubProductItem> deleteSubProductItem = new List<SubProductItem>();
                model?.ForEach(x =>
                {
                    SubProductItem updateStock = new SubProductItem(x.Id > 0 ? x.Id : 0, x.ParentProductId, x.Quantity, x.Retail, UserModelVm.UserId, null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty);
                    if (x.Id > 0)
                        updateSubProductItem.Add(updateStock);
                    else
                        insertSubProductItem.Add(updateStock);
                });
                ;
                if (deleteProducts.Any())
                    deleteSubProductItem.AddRange(deleteProducts.Select(x => new SubProductItem(x.Id, x.ParentProductId, x.Quantity, x.Retail, x.CreatedBy, UserModelVm.UserId, x.CreatedDate, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()))));
                _productRepository.SaveUpdateSubProductItems(updateSubProductItem, insertSubProductItem, deleteSubProductItem);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveUpdateSubProductItems method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveSubProductItems(List<SubProductItemModel> model)
        {
            try
            {
                List<SubProductItem> insertSubProductItem = new List<SubProductItem>();
                model?.ForEach(x =>
                {
                    SubProductItem updateStock = new SubProductItem(0, x.ParentProductId, x.Quantity, x.Retail, UserModelVm.UserId, null, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), string.Empty);
                    insertSubProductItem.Add(updateStock);
                });
                _productRepository.SaveSubProductItems(insertSubProductItem);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveSubProductItems method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool DeleteProduct(int id)
        {
            try
            {
                var result = _purchaseRepository.IsProductExist(id);
                if (result == false)
                {
                    Product product = _productRepository.GetProductDetail(id);
                    _productRepository.DeleteProduct(product);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteProducts method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public bool DeleteWastage(int id)
        {
            try
            {
                Wastage wastage = _productRepository.GetWastage().FirstOrDefault(x => x.Id == id);
                _productRepository.DeleteWastage(wastage);
                return true;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteWastage method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }

        }
        
        public void UpdateLabel(LabelSettingModel model)
        {
            try
            {

                LabelSetting obj = _labelSettingRepository.GetLabelData(model.ItemId);
                if (obj != null)
                {
                    obj.Id = model.Id;
                }
                else
                {
                    obj = new LabelSetting();
                }
                obj.ItemId = model.ItemId;
                obj.PrintItemCode = model.PrintItemCode;
                obj.PrintItemDetail = model.PrintItemDetail;
                obj.PrintUnitMeasure = model.PrintUnitMeasure;
                obj.PrintItemPrice = model.PrintItemPrice;
                obj.PrintBarCode = model.PrintBarCode;
                obj.BarCodeHeight = model.BarCodeHeight;
                obj.LabelSheet = model.LabelSheet;
                obj.TotalNoOfPrints = model.TotalNoOfPrints;
                obj.StartRow = model.StartRow;
                obj.StartColumn = model.StartColumn;
                _labelSettingRepository.UpdateLabel(obj);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateLabel method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public int UpdateMasterLabel(MasterLabelSettingModel model)
        {
            try
            {
                MasterLabelSetting masterLabelSetting = new MasterLabelSetting(model.Id, model.LabelSettingCode, model.PrintItemCode, model.PrintItemDetail, model.PrintUnitMeasure, model.PrintItemPrice, model.PrintBarCode, model.BarCodeHeight, model.LabelSheet, model.StartRow, model.StartColumn);
                return _labelSettingRepository.UpdateMasterLabel(masterLabelSetting);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in UpdateMasterLabel method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void DeleteMasterLabelSetting()
        {
            try
            {
                MasterLabelSetting masterSetting = _labelSettingRepository.GetMasaterLabelSetting();
                if (masterSetting != null)
                    //  MasterLabelSetting masterLabelSetting = new MasterLabelSetting(masterLabel.Id, masterLabel.LabelSettingCode, masterLabel.PrintItemCode, masterLabel.PrintItemDetail, masterLabel.PrintUnitMeasure, masterLabel.PrintItemPrice, masterLabel.PrintBarCode, masterLabel.BarCodeHeight, masterLabel.LabelSheet, masterLabel.StartRow, masterLabel.StartColumn);
                    _labelSettingRepository.DeleteMasterLabelSetting(masterSetting);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteMasterLabelSetting method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void DeleteProductLabelSetting()
        {
            try
            {
                if (_labelSettingRepository.GetAllProductLabelSettings().Any())
                    _labelSettingRepository.DeleteProductLabelSetting(_labelSettingRepository.GetAllProductLabelSettings());
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in DeleteProductLabelSetting method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        
        public void SaveProductLabel(List<ProductLabelSettingModel> model)
        {
            try
            {
                _labelSettingRepository.SaveProductLabel(model.Select(x => new ProductLabelSetting(x.Id, x.MasterLabelCode, x.ProductCode, x.Quantity)
               ).ToList());
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error in SaveProductLabel method";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public void SaveUpdateWastage(WastageModel model)
        {
            try
            {
                Wastage wastage;
                if (model.WastageId > 0)
                {
                    wastage = _productRepository.GetWastage().FirstOrDefault(x => x.Id == model.WastageId);
                    wastage.ModifiedBy = 1;
                    wastage.ModifiedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                }
                else
                {
                    wastage = new Wastage();
                    wastage.Id = model.WastageId;
                    wastage.ItemCode = model.ItemCode;
                    wastage.ProductName = model.ProductName;
                    wastage.CreatedDate = model.Date;
                    wastage.CreatedBy = 1;
                }
                wastage.Id = model.WastageId;
                wastage.ItemCode = model.ItemCode;
                wastage.ProductName = model.ProductName;
                wastage.CreatedDate = model.Date;
                wastage.Quantity = model.Quantity;
                wastage.Reason = model.Reason;
                wastage.BatchNo = model.BatchNo;
                wastage.BranchCode = model.BranchCode;
                wastage.CompanyCode = model.CompanyCode;
                _productRepository.SaveUpdateWastage(wastage);
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Wastage";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        public int SaveUpdateProduct(ProductModel model)
        {
            try
            {
                Product product;
                if (model.Id > 0)
                {
                    product = _productRepository.GetProducts().FirstOrDefault(x => x.Id == model.Id);
                    product.Id = model.Id;
                    product.ModifiedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    product.ModifiedBy = UserModelVm.UserId;
                }
                else
                {
                    product = new Product();
                    product.CreatedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    product.CreatedBy = UserModelVm.UserId;
                }
                product.ItemName = model.ItemName;
                product.ItemType = model.ItemType;
                product.CategoryCode = model.CategoryCode;
                product.BarCode = model.BarCode;
                product.ItemImage = model.ItemImage;
                product.MinimumLevel = model.MinimumLevel;
                product.ShortName = model.ShortName;
                product.TaxPercentage = model.TaxPercentage;
                product.RetailPrice = model.RetailPrice;
                product.ResellerPrice = model.ResellerPrice;
                product.ReOrderLevel = model.ReOrderLevel;
                product.Description = model.Description;
                product.TradePrice = model.TradePrice;
                product.Weight = model.Weight;
                product.WholesellerPrice = model.WholeSellerPrice;
                product.IsTaxInclusive = model.IsTaxInclusive;
                product.IsDeleted = false;
                product.BranchCode = model.BranchCode;
                product.ImageText = model.ImageText.ToString();
                product.CompanyCode = model.CompanyCode;
                product.BulkCode = model.BulkCode;
                int productId = _productRepository.SaveUpdateProduct(product);
                return productId;
            }
            catch (Exception ex)
            {
                fault.Result = false;
                fault.ErrorMessage = "Error During Save Or Update Product";
                fault.ErrorDetails = ex.ToString();
                throw new FaultException<FaultData>(fault);
            }
        }
        #endregion
        
        
       
    }
}

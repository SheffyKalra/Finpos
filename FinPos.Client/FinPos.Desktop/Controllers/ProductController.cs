using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPos.Domain.DataContracts;
using System.ServiceModel;
using FinPos.DomainContracts.DataContracts;
using FinPos.WcfHost;
using System.Windows;
using System.ServiceModel.Channels;
using FinPos.Utility.CommonEnums;
using FinPos.Data.Entities;
using FinPos.Client.ServiceEndPoints.Interface;

namespace FinPos.Client.Controllers
{
    public class ProductController
    {

        IServiceEndpoints objProductService = new ServiceEndPoints.ServiceEndPoints();
        public ResponseVm GetProducts()
        {
            try
            {
                IList<ProductModel> lstProducts = objProductService.ProductServiceInstance().GetProducts();
                lstProducts = lstProducts.Select(x =>
                     new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholeSellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryName, Enum.GetName(typeof(CommonEnum.ItemTypes), x.ItemType), null, x.ImageText, x.CompanyCode, x.BulkCode)
               ).ToList();

                return new ResponseVm(null, new List<dynamic>(lstProducts).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public ProductModel GetProductById(int ProductId)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetProductsById(ProductId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public bool IsProductExistInRepack(int productId)
        {
            try
            {
                return objProductService.ProductServiceInstance().IsProductExistInRepack(productId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public ResponseVm GetProductsByCompanyAndBranch()
        {
            try
            {
                IList<ProductModel> lstProducts = objProductService.ProductServiceInstance().GetProductsByCompanyAndBranch(UserModelVm.CompanyId, UserModelVm.BranchId).Select(x =>
                       new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholeSellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryName, Enum.GetName(typeof(CommonEnum.ItemTypes), x.ItemType), null, x.ImageText, x.CompanyCode, x.BulkCode)
               ).ToList();
                return new ResponseVm(null, new List<dynamic>(lstProducts).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public ResponseVm GetProductsByitemType(int companyId, int? branchId, int itemType)
        {
            try
            {
                IList<ProductModel> lstProducts = objProductService.ProductServiceInstance().GetProductsByitemType(companyId, branchId, itemType);
                lstProducts = lstProducts.ToList()?.Select(x =>
                     new ProductModel(x.Id, x.ItemName, x.CategoryCode, x.RetailPrice, x.TradePrice, x.WholeSellerPrice, x.ResellerPrice, x.ItemType, x.Weight, x.BarCode, x.TaxPercentage, x.MinimumLevel, x.ReOrderLevel, x.ItemImage, x.IsTaxInclusive, x.ShortName, x.Description, x.BranchCode, x.CategoryName, Enum.GetName(typeof(CommonEnum.ItemTypes), x.ItemType), null, x.ImageText, x.CompanyCode, x.BulkCode)
               ).ToList();
                return new ResponseVm(null, new List<dynamic>(lstProducts).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public ProductModel GetProductsById(int companyId, int? branchId, int bulkCode)
        {
            try
            {
                ProductModel product = objProductService.ProductServiceInstance().GetProductsByBulkCode(companyId, branchId, bulkCode);
                product = new ProductModel(product.Id, product.ItemName, product.CategoryCode, product.RetailPrice, product.TradePrice, product.WholeSellerPrice, product.ResellerPrice, product.ItemType, product.Weight, product.BarCode, product.TaxPercentage, product.MinimumLevel, product.ReOrderLevel, product.ItemImage, product.IsTaxInclusive, product.ShortName, product.Description, product.BranchCode, product.CategoryName, Enum.GetName(typeof(CommonEnum.ItemTypes), product.ItemType), null, product.ImageText, product.CompanyCode, product.BulkCode);
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public List<RepackModel> GetRepackByBulkId(int companyId, int? branchId, int bulkCode)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetRepackByBulkId(companyId, branchId, bulkCode).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public void SaveUpdateRepackitems(List<RepackModel> model)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveUpdateRepackitems(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public ResponseVm GetWastage()
        {
            try
            {
                IList<WastageModel> lstWastage = objProductService.ProductServiceInstance().GetWastage();
                return new ResponseVm(null, new List<object>(lstWastage).ToList());
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public ResponseVm GetWastageWithDateFilter(DateTime startDate, DateTime endDate)
        {
            try
            {
                IList<WastageModel> lstWastage = objProductService.ProductServiceInstance().GetWastageWithDateFilter(startDate, endDate);
                return new ResponseVm(null, new List<object>(lstWastage));
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public ResponseVm DeleteProduct(int id)
        {
            try
            {
                var result = objProductService.ProductServiceInstance().DeleteProduct(id);
                ResponseVm returnFault;
                if (result == false)
                {
                    FaultException<FaultData> ex = new FaultException<FaultData>(new FaultData()
                    {
                        Result = false,
                        ErrorMessage = (string)Application.Current.Resources["product_exist_exeption"],
                        ErrorDetails = "error occurs during product delete"
                    }
                );
                    returnFault = new ResponseVm(ex, new List<object>(1));
                }
                else
                {
                    returnFault = new ResponseVm(null, new List<object>(1));
                }
                return returnFault;
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public ResponseVm DeleteWastage(int id)
        {
            try
            {
                objProductService.ProductServiceInstance().DeleteWastage(id);
                return new ResponseVm(null, new List<object>(1));
            }
            catch (FaultException<FaultData> e)
            {
                return new ResponseVm(e, null);
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public LabelSettingModel GetLebelDate(int itemId)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetLabelData(itemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public void SaveUpdateLabel(LabelSettingModel model)
        {
            try
            {
                objProductService.ProductServiceInstance().UpdateLabel(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public int SaveUpdateMasterLabel(MasterLabelSettingModel model)
        {
            try
            {
                return objProductService.ProductServiceInstance().UpdateMasterLabel(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void DeleteMasterLabelSetting()
        {
            try
            {
                objProductService.ProductServiceInstance().DeleteMasterLabelSetting();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void DeleteProductLabelSetting()
        {
            try
            {
                objProductService.ProductServiceInstance().DeleteProductLabelSetting();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public MasterLabelSettingModel GetMasaterLabelSetting()
        {
            try
            {
                return objProductService.ProductServiceInstance().GetMasaterLabelSetting();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public List<ProductLabelSettingModel> GetProductLabelSettings(int masterLabelId)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetProductLabelSettings(masterLabelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void SaveProductLabel(List<ProductLabelSettingModel> model)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveProductLabel(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public void SaveUpdateWastage(WastageModel model)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveUpdateWastage(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public int SaveUpdateProduct(ProductModel model)
        {
            try
            {

                return objProductService.ProductServiceInstance().SaveUpdateProduct(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public int? GetProductIdByName(string txtProductName)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetProductIdByName(txtProductName, UserModelVm.CompanyId, UserModelVm.BranchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }

        public void SaveUpdateProductItemContent(List<ProductItemContentModel> model, List<ProductItemContentModel> deleteProducts)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveUpdateProductItemContent(model, deleteProducts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void SaveProductItemContent(List<ProductItemContentModel> model)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveProductItemContent(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void SaveUpdateSubProductItems(List<SubProductItemModel> model, List<SubProductItemModel> deleteProducts)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveUpdateSubProductItems(model, deleteProducts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public void SaveSubProductItems(List<SubProductItemModel> model)
        {
            try
            {
                objProductService.ProductServiceInstance().SaveSubProductItems(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public List<ProductItemContentModel> GetProductItemContentById(int productItemContentId)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetProductItemContentById(productItemContentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
        public List<SubProductItemModel> GetSubProductItemById(int productId)
        {
            try
            {
                return objProductService.ProductServiceInstance().GetSubProductItemById(productId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductService.ProductServiceInstanceClosed();
            }
        }
    }
}

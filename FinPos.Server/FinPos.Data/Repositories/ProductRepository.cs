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
    public class ProductRepository : IProductRepository
    {
        private readonly IEntityProvider<Product> _productProvider;
        private readonly IEntityProvider<Wastage> _wastageProvider;
        private readonly IEntityProvider<ProductItemContent> _productItemContentProvider;
        private readonly IEntityProvider<SubProductItem> _subProductItemProvider;
        private readonly IEntityProvider<Repack> _repackItemProvider;

        public ProductRepository(IEntityProvider<Product> productProvider, IEntityProvider<Wastage> wastageProvider, IEntityProvider<ProductItemContent> productItemContentProvider, IEntityProvider<SubProductItem> subProductItemProvider, IEntityProvider<Repack> repackItemProvider)
        {
            _productProvider = productProvider;
            _wastageProvider = wastageProvider;
            _productItemContentProvider = productItemContentProvider;
            _subProductItemProvider = subProductItemProvider;
            _repackItemProvider = repackItemProvider;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            products = _productProvider.Get().ToList();
            return products;
        }
        public List<Product> GetProductsByitemType(int companyId, int? branchId, int itemType)
        {
            List<Product> products = new List<Product>();
            products = _productProvider.Get().Where(x => x.CompanyCode == companyId && x.BranchCode == branchId && x.ItemType == itemType).ToList();
            return products;
        }
        public List<Repack> GetRepackByBulkId(int companyId, int? branchId, int bulkCode)
        {
            List<Repack> repacks = new List<Repack>();
            repacks = _repackItemProvider.Get().Where(x => x.BulkCode == bulkCode).ToList();
            return repacks;
        }
        public Product GetProductsById(int companyId, int? branchId, int bulkCode)
        {
            Product product = _productProvider.Get().FirstOrDefault(x => x.CompanyCode == companyId && x.BranchCode == branchId && x.Id == bulkCode);
            return product;
        }

        public List<Wastage> GetWastage()
        {
            //List<Wastage> wastages = new List<Wastage>();
            return _wastageProvider.Get().ToList();
            //return this._wastageProvider.Get().ToList();
        }

        public void SaveUpdateWastage(Wastage wastage)
        {
            if (wastage.Id > 0)
                this._wastageProvider.Update(wastage);
            else
                this._wastageProvider.Insert(wastage);
        }
        public int SaveUpdateProduct(Product product)
        {
            if (product.Id > 0)
                return this._productProvider.Update(product);
            else
                return this._productProvider.Insert(product);
        }
        public List<ProductItemContent> GetProductItemContents()
        {
            return null;
        }
        public void SaveUpdateProductItemContent(List<ProductItemContent> updateProductItemContent, List<ProductItemContent> insertProductItemContent, List<ProductItemContent> deleteProductItemContent)
        {
            ProductItemContent obj;
            if (updateProductItemContent.Any())
            {
                updateProductItemContent.ForEach(x =>
                {

                    obj = this._productItemContentProvider.GetSingle(z => z.Id == x.Id);
                    obj.Id = obj.Id;
                    obj.SubProductId = obj.SubProductId;
                    obj.Quantity = x.Quantity;
                    obj.CreatedDate = obj.CreatedDate;
                    obj.CreatedBy = x.CreatedBy;
                    obj.ModifiedBy = x.ModifiedBy;
                    obj.ModifiedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    obj.IsFreeProduct = x.IsFreeProduct;
                    this._productItemContentProvider.Update(obj);
                });
            }
            if (deleteProductItemContent != null || deleteProductItemContent.Any())
            {
                deleteProductItemContent.ForEach(z =>
                {
                    var productContent = this._productItemContentProvider.GetSingle(x => x.Id == z.Id);
                    this._productItemContentProvider.Delete(productContent.Id);
                });
            }
            if (insertProductItemContent.Any())
                this._productItemContentProvider.InsertAll(insertProductItemContent);
        }
        public void SaveProductItemContent(List<ProductItemContent> insertProductItemContent)
        {
            if (insertProductItemContent.Any())
                this._productItemContentProvider.InsertAll(insertProductItemContent);
        }
        public void SaveUpdateSubProductItems(List<SubProductItem> updateSubProductItem, List<SubProductItem> insertSubProductItem, List<SubProductItem> deleteSubProductItem)
        {
            SubProductItem obj;
            if (updateSubProductItem.Any())
            {
                updateSubProductItem.ForEach(x =>
                {

                    obj = this._subProductItemProvider.GetSingle(z => z.Id == x.Id);
                    obj.Id = obj.Id;
                    obj.ParentProductId = obj.ParentProductId;
                    obj.RetailPrice = x.RetailPrice;
                    obj.Quantity = x.Quantity;
                    obj.CreatedDate = obj.CreatedDate;
                    obj.CreatedBy = x.CreatedBy;
                    obj.ModifiedBy = x.ModifiedBy;
                    obj.ModifiedDate = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
                    this._subProductItemProvider.Update(obj);
                });
            }
            if (deleteSubProductItem.Any())
            {
                deleteSubProductItem.ForEach(z =>
                {
                    var productContent = this._subProductItemProvider.GetSingle(x => x.Id == z.Id);
                    this._subProductItemProvider.Delete(productContent.Id);
                });
            }
            if (insertSubProductItem.Any())
                this._subProductItemProvider.InsertAll(insertSubProductItem);
        }
        public void SaveSubProductItems(List<SubProductItem> insertSubProductItem)
        {
            if (insertSubProductItem.Any())
                this._subProductItemProvider.InsertAll(insertSubProductItem);
        }
        public void SaveUpdateRepackitems(List<Repack> insertRepackItem)
        {
            if (insertRepackItem.Any())
                this._repackItemProvider.InsertAll(insertRepackItem);
        }
        public bool IsProductExistInRepack(int productId)
        {
            return this._repackItemProvider.Any(x => x.ProductCode == productId);
        }
        public bool IsCategoryExist(int id)
        {
            return this._productProvider.Any(x => x.CategoryCode == id);
        }

        public List<Wastage> GetWastageWithDateFilter(DateTime startDate, DateTime endDate)
        {
            //return this._wastageProvider.Get().Where(x=>x.CreatedDate>=startDate.Date && x.CreatedDate<=endDate.Date).ToList();
            return this._wastageProvider.Get().ToList();
        }
        public Product GetProductDetail(int id)
        {
            return this._productProvider.Get().FirstOrDefault(x => x.Id == id);
        }
        public List<ProductItemContent> GetProductItemContentsById(int productContentId)
        {
            return this._productItemContentProvider.Get().Where(x => x.MainProductId == productContentId).ToList();
        }
        public List<SubProductItem> GetSubProductItemById(int productContentId)
        {
            return this._subProductItemProvider.Get().Where(x => x.ParentProductId == productContentId).ToList();
        }
        public void DeleteProduct(Product product)
        {
            this._productProvider.Delete(product);
        }
        public void DeleteWastage(Wastage wastage)
        {
            this._wastageProvider.Delete(wastage);
        }

        public List<Product> GetProductByComponyIdAndBranchId(int companyId, int? branchId)
        {
            return this._productProvider.Get().Where(item => item.CompanyCode == companyId && item.BranchCode == branchId).ToList();
        }
        public int? GetProductIdByName(string txtProductName, int companyId, int? bracnchId)
        {

            if (this._productProvider.Any(item => item.ItemName == txtProductName && item.CompanyCode==companyId && item.BranchCode==bracnchId))
            {
                return this._productProvider.Get().Where(item => item.ItemName == txtProductName && item.CompanyCode == companyId && item.BranchCode == bracnchId).FirstOrDefault().Id;
            }
            else
            {
                return null;
            }
        }
        public Product GetProductsById(int productId)
        {
          return  this._productProvider.Get().Where(item => item.Id == productId).FirstOrDefault();
        }
    }
}

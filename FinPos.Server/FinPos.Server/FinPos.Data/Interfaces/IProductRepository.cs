using FinPos.Data.Entities;
using FinPos.DomainContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DAL.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetProducts();

        List<Product> GetProductsByitemType(int companyId, int? branchId, int itemType);
        List<Repack> GetRepackByBulkId(int companyId, int? branchId, int bulkCode);
        Product GetProductsById(int companyId, int? branchId, int bulkCode);

        List<Wastage> GetWastage();
        void DeleteProduct(Product product);

        void DeleteWastage(Wastage wastage);
        Product GetProductDetail(int id);

        List<Wastage> GetWastageWithDateFilter(DateTime startDate, DateTime endDate);

        void SaveUpdateWastage(Wastage wastage);

        int SaveUpdateProduct(Product product);
        void SaveUpdateProductItemContent(List<ProductItemContent> updateProductItemContent, List<ProductItemContent> insertProductItemContent, List<ProductItemContent> deleteProductItemContent);
        void SaveProductItemContent(List<ProductItemContent> insertProductItemContent);
        void SaveUpdateSubProductItems(List<SubProductItem> updateSubProductItem, List<SubProductItem> insertSubProductItem, List<SubProductItem> deleteSubProductItem);
        void SaveSubProductItems(List<SubProductItem> insertSubProductItem);
        void SaveUpdateRepackitems(List<Repack> insertSubProductItem);
        List<ProductItemContent> GetProductItemContentsById(int productContentId);
        List<SubProductItem> GetSubProductItemById(int productContentId);
        bool IsCategoryExist(int id);
        bool IsProductExistInRepack(int productId);


        List<Product> GetProductByComponyIdAndBranchId(int companyId, int? branchId);
        int? GetProductIdByName(string txtProductName,int companyId, int? bracnchId);
        Product GetProductsById(int productId);
    }
}

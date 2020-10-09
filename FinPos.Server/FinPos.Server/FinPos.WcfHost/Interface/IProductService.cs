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
    public interface IProductService
    {
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<ProductModel> GetProducts();
        [OperationContract]
        IList<ProductModel> GetProductsByCompanyAndBranch(int companyId, int? branchId);
        [OperationContract]
        ProductModel GetProductsById(int productId);
        [OperationContract]
        bool IsProductExistInRepack(int productId);
        [OperationContract]
        IList<ProductModel> GetProductsByitemType(int companyId, int? branchId, int itemType);
        [OperationContract]
        ProductModel GetProductsByBulkCode(int companyId, int? branchId, int bulkCode);
        [OperationContract]
        IList<RepackModel> GetRepackByBulkId(int companyId, int? branchId, int bulkCode);
        [OperationContract]
        void SaveUpdateRepackitems(List<RepackModel> model);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<WastageModel> GetWastage();
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<WastageModel> GetWastageWithDateFilter(DateTime startDate, DateTime endDate);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeleteProduct(int id);
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DeleteWastage(int id);
        [OperationContract]
        LabelSettingModel GetLabelData(int itemId);
        [OperationContract]
        void UpdateLabel(LabelSettingModel model);
        [OperationContract]
        int UpdateMasterLabel(MasterLabelSettingModel model);
        [OperationContract]
        void DeleteMasterLabelSetting();
        [OperationContract]
        void DeleteProductLabelSetting();
        [OperationContract]
        MasterLabelSettingModel GetMasaterLabelSetting();
        [OperationContract]
        List<ProductLabelSettingModel> GetProductLabelSettings(int masterLabelId);
        [OperationContract]
        void SaveProductLabel(List<ProductLabelSettingModel> model);
        [OperationContract]
        void SaveUpdateWastage(WastageModel model);
        [OperationContract]
        int SaveUpdateProduct(ProductModel model);
        [FaultContract(typeof(FaultData))]
        [OperationContract]
        int? GetProductIdByName(string txtProductName, int companyId, int? branchId);
        [OperationContract]
        void SaveUpdateProductItemContent(List<ProductItemContentModel> model, List<ProductItemContentModel> deleteProducts);
        [OperationContract]
        void SaveProductItemContent(List<ProductItemContentModel> model);
        [OperationContract]
        void SaveUpdateSubProductItems(List<SubProductItemModel> model, List<SubProductItemModel> deleteProducts);
        [OperationContract]
        void SaveSubProductItems(List<SubProductItemModel> model);
        [OperationContract]
        List<ProductItemContentModel> GetProductItemContentById(int productItemContentId);
        [OperationContract]
        List<SubProductItemModel> GetSubProductItemById(int productItemContentId);
    }
}

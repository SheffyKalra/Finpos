using FinPos.Client.ServiceEndPoints;
using FinPos.Client.ServiceEndPoints.Interface;
using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using FinPos.DAL.Providers;
using FinPos.DAL.Repositories;
using FinPos.Data;
using FinPos.Data.Entities;
using FinPos.Data.Interfaces;
using FinPos.Data.Repositories;
using FinPos.WcfHost;
//using MySql.Data.MySqlClient;
using Ninject.Modules;


namespace FinPos.WindowActivator
{
    public class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEntityProvider<Company>>().To<MySQLProvider<Company>>();
            Bind<IEntityProvider<Branch>>().To<MySQLProvider<Branch>>();
            Bind<IEntityProvider<User>>().To<MySQLProvider<User>>();
            Bind<IEntityProvider<Product>>().To<MySQLProvider<Product>>();
            Bind<IEntityProvider<Wastage>>().To<MySQLProvider<Wastage>>();
            Bind<IEntityProvider<Category>>().To<MySQLProvider<Category>>();
            Bind<IEntityProvider<PurchaseOrder>>().To<MySQLProvider<PurchaseOrder>>();
            Bind<IEntityProvider<Purchase>>().To<MySQLProvider<Purchase>>();
            Bind<IEntityProvider<PurchaseReturn>>().To<MySQLProvider<PurchaseReturn>>();
            Bind<IEntityProvider<StockAdjustment>>().To<MySQLProvider<StockAdjustment>>();
            Bind<IEntityProvider<LabelSetting>>().To<MySQLProvider<LabelSetting>>();
            Bind<IEntityProvider<Supplier>>().To<MySQLProvider<Supplier>>();
            Bind<IEntityProvider<Stock>>().To<MySQLProvider<Stock>>();
            Bind<IEntityProvider<OpeningStock>>().To<MySQLProvider<OpeningStock>>();
            Bind<IEntityProvider<SystemConfiguration>>().To<MySQLProvider<SystemConfiguration>>();
            Bind<IEntityProvider<MasterLabelSetting>>().To<MySQLProvider<MasterLabelSetting>>();
            Bind<IEntityProvider<ProductLabelSetting>>().To<MySQLProvider<ProductLabelSetting>>();
            Bind<IEntityProvider<Tax>>().To<MySQLProvider<Tax>>();
            Bind<IEntityProvider<ProductItemContent>>().To<MySQLProvider<ProductItemContent>>();
            Bind<IEntityProvider<SubProductItem>>().To<MySQLProvider<SubProductItem>>();
            Bind<IEntityProvider<Repack>>().To<MySQLProvider<Repack>>();
            Bind<IEntityProvider<Coupon>>().To<MySQLProvider<Coupon>>();
            Bind<IEntityProvider<CouponDetail>>().To<MySQLProvider<CouponDetail>>();
            Bind<IEntityProvider<PaymentToSupplier>>().To<MySQLProvider<PaymentToSupplier>>();
            Bind<IEntityProvider<Offer>>().To<MySQLProvider<Offer>>();
            Bind<IEntityProvider<OfferDetail>>().To<MySQLProvider<OfferDetail>>();
            Bind<ICompanyRepository>().To<CompanyRepository>();
            Bind<IBranchRepository>().To<BranchRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IProductRepository>().To<ProductRepository>();
            Bind<ICategoryRepository>().To<CategoryRepository>();
            Bind<IPurchaseRepository>().To<PurchaseRepository>();
            Bind<ILabelSettingRepository>().To<LabelSettingRepository>();
            Bind<IOpeningStockRepository>().To<OpeningStockRepository>();
            Bind<IStockAdjustmentRepository>().To<StockAdjustmentRepository>();
            Bind<ISupplierRepository>().To<SupplierRepository>();
            //Bind<IFinPosService>().To<FinPosService>();
            Bind<WcfHost.Interface.ICouponManagmentService>().To<WcfHost.Services.CouponManagmentService>();
            Bind<WcfHost.Interface.ICategoryService>().To<WcfHost.Services.CategoryService>();
            Bind<WcfHost.Interface.ICompanyService>().To<WcfHost.Services.CompanyService>();
            Bind<WcfHost.Interface.IOpeningStockService>().To<WcfHost.Services.OpeningStockService>();
            Bind<WcfHost.Interface.IProductService>().To<WcfHost.Services.ProductService>();
            Bind<WcfHost.Interface.IPurchaseService>().To<WcfHost.Services.PurchaseService>();
            Bind<WcfHost.Interface.IStockAdjustmentService>().To<WcfHost.Services.StockAdjustmentService>();
            Bind<WcfHost.Interface.ISupplierService>().To<WcfHost.Services.SupplierService>();
            Bind<WcfHost.Interface.ISystemConfigurationService>().To<WcfHost.Services.SystemConfigurationService>();
            Bind<WcfHost.Interface.ITaxService>().To<WcfHost.Services.TaxService>();
            Bind<WcfHost.Interface.IUserService>().To<WcfHost.Services.UserService>();
            Bind<ISystemConfigurationRepository>().To<SystemConfigurationRepository>();
            Bind<ITaxRepository>().To<TaxRepository>();
            Bind<ICouponManagmentRepository>().To<CouponManagmentRepository>();
            Bind<ICouponDetailsRepository>().To<CouponDetailRepository>();
            Bind<IOfferDetailRepository>().To<OfferDetailRepository>();
            Bind<FinPos.Client.ServiceEndPoints.Interface.IServiceEndpoints>().To<FinPos.Client.ServiceEndPoints.ServiceEndPoints>();
            //Bind<IServiceEndpoints>().To<ServiceEndPoints>();
        }
    }
}

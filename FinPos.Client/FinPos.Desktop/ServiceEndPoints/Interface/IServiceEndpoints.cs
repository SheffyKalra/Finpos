using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.Client.ServiceEndPoints.Interface
{
    public interface IServiceEndpoints
    {
        WcfHost.Interface.ICouponManagmentService CouponManagmentServiceInstance();
        WcfHost.Interface.ICategoryService CategoryServiceInstance();
        WcfHost.Interface.ICompanyService CompanyServiceInstance();
        WcfHost.Interface.IOpeningStockService OpeningStockServiceInstance();
        WcfHost.Interface.IProductService ProductServiceInstance();
        WcfHost.Interface.IPurchaseService PurchaseServiceInstance();
        WcfHost.Interface.IStockAdjustmentService StockAdjustmentServiceInstance();
        WcfHost.Interface.ISupplierService SupplierServiceInstance();
        WcfHost.Interface.ISystemConfigurationService SystemConfigurationServiceInstance();
        WcfHost.Interface.ITaxService TaxServiceInstance();
        WcfHost.Interface.IUserService UserServiceInstance();
        void CouponManegmentServiceInstanceClosed();
        void CategoryServiceInstanceClosed();
        void CompanyServiceInstanceClosed();
        void OpeningStockServiceInstanceClosed();
        void ProductServiceInstanceClosed();
        void PurchaseServiceInstanceClosed();
        void StockAdjustmentServiceInstanceClosed();
        void SupplierServiceInstanceClosed();
        void SystemConfigurationServiceInstanceClosed();
        void TaxServiceInstanceClosed();
        void UserServiceInstanceClosed();
    }
}

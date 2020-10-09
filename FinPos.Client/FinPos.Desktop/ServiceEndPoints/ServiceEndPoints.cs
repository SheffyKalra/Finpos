
using FinPos.Client.ServiceEndPoints.Interface;
using System.ServiceModel;
using System.Windows;

namespace FinPos.Client.ServiceEndPoints
{
    public class ServiceEndPoints : IServiceEndpoints
    {
        #region EndPoint Properties
          
        private NetTcpBinding tcpBindings = new NetTcpBinding();

        /// <summary>
        /// Temp Code 
        /// </summary>
        private EndpointAddress couponManagmentEndPoint = new EndpointAddress("net.tcp://localhost:8090/CouponManagment/");
        private EndpointAddress catagoryManegmentEndPoint = new EndpointAddress("net.tcp://localhost:8090/CategoryService/");
        private EndpointAddress companyManegmentEndPoint = new EndpointAddress("net.tcp://localhost:8090/CompanyService/");
        private EndpointAddress openingStockEndPoint = new EndpointAddress("net.tcp://localhost:8090/OpeningStockService/");
        private EndpointAddress productEndPoint = new EndpointAddress("net.tcp://localhost:8090/ProductService/");
        private EndpointAddress purchaseEndPoint = new EndpointAddress("net.tcp://localhost:8090/PurchaseService/");
        private EndpointAddress stockAdjustmentEndPoint = new EndpointAddress("net.tcp://localhost:8090/StockAdjustmentService/");
        private EndpointAddress supplierEndPoint = new EndpointAddress("net.tcp://localhost:8090/SupplierService/");
        private EndpointAddress systemConfigurationEndPoint = new EndpointAddress("net.tcp://localhost:8090/SystemConfigurationService/");
        private EndpointAddress taxEndPoint = new EndpointAddress("net.tcp://localhost:8090/TaxService/");
        private EndpointAddress userEndPoint = new EndpointAddress("net.tcp://localhost:8090/UserService/");
        /// <summary>
        /// Getting Error Need To Resolve
        /// </summary>
        //private EndpointAddress couponManagmentEndPoint = new EndpointAddress((string)Application.Current.Resources["CouponManagmentService"]);
        //private EndpointAddress catagoryManegmentEndPoint = new EndpointAddress((string)Application.Current.Resources["CategoryService"]);
        //private EndpointAddress companyManegmentEndPoint = new EndpointAddress((string)Application.Current.Resources["CompanyService"]);
        //private EndpointAddress openingStockEndPoint = new EndpointAddress((string)Application.Current.Resources["OpeningStockService"]);
        //private EndpointAddress productEndPoint = new EndpointAddress((string)Application.Current.Resources["ProductService"]);
        //private EndpointAddress purchaseEndPoint = new EndpointAddress((string)Application.Current.Resources["PurchaseService"]);
        //private EndpointAddress stockAdjustmentEndPoint = new EndpointAddress((string)Application.Current.Resources["StockAdjustmentService"]);
        //private EndpointAddress supplierEndPoint = new EndpointAddress((string)Application.Current.Resources["SupplierService"]);
        //private EndpointAddress systemConfigurationEndPoint = new EndpointAddress((string)Application.Current.Resources["SystemConfigurationService"]);
        //private EndpointAddress taxEndPoint = new EndpointAddress((string)Application.Current.Resources["TaxService"]);
        //private EndpointAddress userEndPoint = new EndpointAddress((string)Application.Current.Resources["UserService"]);
        ChannelFactory<WcfHost.Interface.ICouponManagmentService> _channelFactoryCouponManagment;
        ChannelFactory<WcfHost.Interface.ICategoryService> _channelFactoryCategory;
        ChannelFactory<WcfHost.Interface.ICompanyService> _channelFactoryCompany;
        ChannelFactory<WcfHost.Interface.IOpeningStockService> _channelFactoryOpeningStock;
        ChannelFactory<WcfHost.Interface.IProductService> _channelFactoryProduct;
        ChannelFactory<WcfHost.Interface.IPurchaseService> _channelFactoryPurchase;
        ChannelFactory<WcfHost.Interface.IStockAdjustmentService> _channelFactoryStockAdjustment;
        ChannelFactory<WcfHost.Interface.ISupplierService> _channelFactorySupplier;
        ChannelFactory<WcfHost.Interface.ISystemConfigurationService> _channelFactorySystemConfiguration;
        ChannelFactory<WcfHost.Interface.ITaxService> _channelFactoryTax;
        ChannelFactory<WcfHost.Interface.IUserService> _channelFactoryUser;
        #endregion

        #region Service Instances


        /// <summary>
        /// Creates channel Factory Instance for Coupon Managment service
        /// </summary>
        /// <returns>Instance for Coupon Managment service</returns>
        public WcfHost.Interface.ICouponManagmentService CouponManagmentServiceInstance()
        {
            _channelFactoryCouponManagment = new ChannelFactory<WcfHost.Interface.ICouponManagmentService>(tcpBindings, couponManagmentEndPoint);
            WcfHost.Interface.ICouponManagmentService instance;
            return instance = _channelFactoryCouponManagment.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for Category Service
        /// </summary>
        /// <returns>Instance for Category Service</returns>
        public WcfHost.Interface.ICategoryService CategoryServiceInstance()
        {
            _channelFactoryCategory = new ChannelFactory<WcfHost.Interface.ICategoryService>(tcpBindings, catagoryManegmentEndPoint);
            WcfHost.Interface.ICategoryService instance;
            return instance = _channelFactoryCategory.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for Company Service
        /// </summary>
        /// <returns>Instance for Company Service</returns>
        public WcfHost.Interface.ICompanyService CompanyServiceInstance()
        {
            _channelFactoryCompany = new ChannelFactory<WcfHost.Interface.ICompanyService>(tcpBindings, companyManegmentEndPoint);
            WcfHost.Interface.ICompanyService instance;
            return instance = _channelFactoryCompany.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for Opening Stock Service
        /// </summary>
        /// <returns>Instance for OpeningStock Service</returns>
        public WcfHost.Interface.IOpeningStockService OpeningStockServiceInstance()
        {
            _channelFactoryOpeningStock = new ChannelFactory<WcfHost.Interface.IOpeningStockService>(tcpBindings, openingStockEndPoint);
            WcfHost.Interface.IOpeningStockService instance;
            return instance = _channelFactoryOpeningStock.CreateChannel();
        }

        /// <summary>
        /// Creates Channel Factory Instance for Product Service
        /// </summary>
        /// <returns>Instance for Product Service</returns>
        public WcfHost.Interface.IProductService ProductServiceInstance()
        {
            _channelFactoryProduct = new ChannelFactory<WcfHost.Interface.IProductService>(tcpBindings, productEndPoint);
            WcfHost.Interface.IProductService instance;
            return instance = _channelFactoryProduct.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for Purchase Service
        /// </summary>
        /// <returns>Instance for Purchase Service</returns>
        public WcfHost.Interface.IPurchaseService PurchaseServiceInstance()
        {
            _channelFactoryPurchase = new ChannelFactory<WcfHost.Interface.IPurchaseService>(tcpBindings, purchaseEndPoint);
            WcfHost.Interface.IPurchaseService instance;
            return instance = _channelFactoryPurchase.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for StockAdjustment Service
        /// </summary>
        /// <returns>Instance for StockAdjustment Service</returns>
        public WcfHost.Interface.IStockAdjustmentService StockAdjustmentServiceInstance()
        {
            _channelFactoryStockAdjustment = new ChannelFactory<WcfHost.Interface.IStockAdjustmentService>(tcpBindings, stockAdjustmentEndPoint);
            WcfHost.Interface.IStockAdjustmentService instance;
            return instance = _channelFactoryStockAdjustment.CreateChannel();
        }
        /// <summary>
        /// Creates Channel Factory Instance for Supplier Service
        /// </summary>
        /// <returns>Instance for Supplier Service</returns>
        public WcfHost.Interface.ISupplierService SupplierServiceInstance()
        {
            _channelFactorySupplier = new ChannelFactory<WcfHost.Interface.ISupplierService>(tcpBindings, supplierEndPoint);
            WcfHost.Interface.ISupplierService instance;
            return instance = _channelFactorySupplier.CreateChannel();
        }

        /// <summary>
        /// Creates Channel Factory Instance for SystemConfiguration Service
        /// </summary>
        /// <returns>Instance for SystemConfiguration Service</returns>
        public WcfHost.Interface.ISystemConfigurationService SystemConfigurationServiceInstance()
        {
            _channelFactorySystemConfiguration = new ChannelFactory<WcfHost.Interface.ISystemConfigurationService>(tcpBindings, systemConfigurationEndPoint);
            WcfHost.Interface.ISystemConfigurationService instance;
            return instance = _channelFactorySystemConfiguration.CreateChannel();
        }

        /// <summary>
        /// Creates Channel Factory Instance for Tax Service
        /// </summary>
        /// <returns>Instance for Tax Service</returns>
        public WcfHost.Interface.ITaxService TaxServiceInstance()
        {
            _channelFactoryTax = new ChannelFactory<WcfHost.Interface.ITaxService>(tcpBindings, taxEndPoint);
            WcfHost.Interface.ITaxService instance;
            return instance = _channelFactoryTax.CreateChannel();
        }

        /// <summary>
        /// Creates Channel Factory Instance for User Service
        /// </summary>
        /// <returns>Instance for Tax Service</returns>
        public WcfHost.Interface.IUserService UserServiceInstance()
        {
            _channelFactoryUser = new ChannelFactory<WcfHost.Interface.IUserService>(tcpBindings, userEndPoint);
            WcfHost.Interface.IUserService instance;
            return instance = _channelFactoryUser.CreateChannel();
        }

        #endregion

        #region Instance Closed

        /// <summary>
        /// Close the instance for Coupon Managment channel Factory
        /// </summary>
        public void CouponManegmentServiceInstanceClosed()
        {
            _channelFactoryCouponManagment.Close();
        }

        /// <summary>
        /// Close the instance for Category Factory
        /// </summary>
        public void CategoryServiceInstanceClosed()
        {
            _channelFactoryCategory.Close();
        }

        /// <summary>
        /// Close the instance for Company Factory
        /// </summary>
        public void CompanyServiceInstanceClosed()
        {
            _channelFactoryCompany.Close();
        }

        /// <summary>
        /// Close the instance for OpeningStock Factory
        /// </summary>
        public void OpeningStockServiceInstanceClosed()
        {
            _channelFactoryOpeningStock.Close();
        }
        /// <summary>
        /// Close the instance for OpeningStock Factory
        /// </summary>
        public void ProductServiceInstanceClosed()
        {
            _channelFactoryProduct.Close();
        }
        /// <summary>
        /// Close the instance for Purchase Factory
        /// </summary>
        public void PurchaseServiceInstanceClosed()
        {
            _channelFactoryPurchase.Close();
        }
        /// <summary>
        /// Close the instance for StockAdjustment Factory
        /// </summary>
        public void StockAdjustmentServiceInstanceClosed()
        {
            _channelFactoryStockAdjustment.Close();
        }

        /// <summary>
        /// Close the instance for Supplier Factory
        /// </summary>
        public void SupplierServiceInstanceClosed()
        {
            _channelFactorySupplier.Close();
        }
        /// <summary>
        /// Close the instance for SystemConfiguration Factory
        /// </summary>
        public void SystemConfigurationServiceInstanceClosed()
        {
            _channelFactorySystemConfiguration.Close();
        }
        /// <summary>
        /// Close the instance for Tax Factory
        /// </summary>
        public void TaxServiceInstanceClosed()
        {
            _channelFactoryTax.Close();
        }
        /// <summary>
        /// Close the instance for User Factory
        /// </summary>
        public void UserServiceInstanceClosed()
        {
            _channelFactoryUser.Close();
        }
        #endregion
    }
}

using FinPos.WcfHost;
using Ninject;
using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;

namespace FinPos.WindowActivator
{
    public partial class FinPosManager : ServiceBase
    {


        internal static ServiceHost couponManagmentServiceHost = null;
        internal static ServiceHost categoryServiceHost = null;
        internal static ServiceHost companyServiceHost = null;
        internal static ServiceHost OpeningStockServiceHost = null;
        internal static ServiceHost ProductServiceHost = null;
        internal static ServiceHost purchaseServiceHost = null;
        internal static ServiceHost stockAdjustmentServiceHost = null;
        internal static ServiceHost systemConfigurationServiceHost = null;
        internal static ServiceHost taxServiceHost = null;
        internal static ServiceHost userServiceHost = null;
        internal static ServiceHost supplier = null;
        public FinPosManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CloseServiceInstance();

            WcfHost.Interface.ICouponManagmentService couponManagmentservice;
            WcfHost.Interface.ICategoryService categoryservice;
            WcfHost.Interface.ICompanyService companyservice;
            WcfHost.Interface.IOpeningStockService openningStockservice;
            WcfHost.Interface.IProductService productservice;
            WcfHost.Interface.IPurchaseService purchaseservice;
            WcfHost.Interface.IStockAdjustmentService stockAdjustmentservice;
            WcfHost.Interface.ISystemConfigurationService systemConfigurationservice;
            WcfHost.Interface.ITaxService taxservice;
            WcfHost.Interface.IUserService userservice;
            WcfHost.Interface.ISupplierService supplierservice;
            SetKernel(out couponManagmentservice, out categoryservice, out companyservice, out openningStockservice, out productservice, out purchaseservice, out stockAdjustmentservice, out systemConfigurationservice, out taxservice, out userservice, out supplierservice);

            bool directoryExist = Directory.Exists("c:/FinposDb");
            if (!directoryExist)
            {
                Directory.CreateDirectory("c:/FinposDb");
            }
            bool fileExist = File.Exists("c:/FinposDb/finpos.sqlite");
            if (!fileExist)
            {

                //var currentDirectory = System.IO.Directory.GetCurrentDirectory() + "/finpos.sqlite";
                SQLiteConnection.CreateFile("c:/FinposDb/finpos.sqlite");
                //System.IO.File.Copy(currentDirectory, "c:/FinposDb/finpos.sqlite", true);
            }

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:/FinposDb/finpos.sqlite;Version=3;");
            m_dbConnection.Open();

            //string forignKey = "PRAGMA foreign_keys = ON";
            //SQLiteCommand commandforignKey = new SQLiteCommand(forignKey, m_dbConnection);
            //commandforignKey.Prepare();

            /**********************CompanyTable****************************/
            string createCompanyTable = "create table IF NOT EXISTS Company (Id Integer primary key AUTOINCREMENT,Name varchar(100), Description  varchar(200),PhoneNo varchar(20),IsDefault bool,IsActive bool,Logo varchar(20),ModifiedDate varchar(40),ModifiedBy varchar(50), CreatedBy varchar(50), CreatedDate varchar(50))";
            SQLiteCommand commandCompanyTable = new SQLiteCommand(createCompanyTable, m_dbConnection);
            commandCompanyTable.ExecuteNonQuery();


            /**********************************End************************/


            /************User Table****************/
            string createUserTable = "create table IF NOT EXISTS User (Id integer primary key AUTOINCREMENT,UserCode int,FirstName varchar(50),LastName varchar(50) ,IsAdmin bool,Email varchar(50),Password varchar(50),IsActive bool,ModifiedDate varchar(40),ModifiedBy  varchar(40), CreatedBy varchar(50),RoleId int , CreatedDate varchar(50))";
            SQLiteCommand commandUserTable = new SQLiteCommand(createUserTable, m_dbConnection);
            commandUserTable.ExecuteNonQuery();
            /*******************************End ************************/




            /*************************Branch Table*************************/
            string createBranchTable = "create table IF NOT EXISTS Branch (Id integer primary key AUTOINCREMENT,CompanyCode int,Name varchar(100), Description  varchar(200),Address varchar(200),PhoneNo varchar(20),IsDefault bool,IsActive bool,ModifiedDate varchar(40),ModifiedBy varchar(50), CreatedBy varchar(50), CreatedDate varchar(50))";
            SQLiteCommand commandBranchTable = new SQLiteCommand(createBranchTable, m_dbConnection);
            commandBranchTable.ExecuteNonQuery();
            /*******************************End *********************************/



            /*************************Category Table*************************/
            string createCategoryTable = "create table IF NOT EXISTS Category (Id integer primary key AUTOINCREMENT,CategoryName varchar(100), Description  varchar(200),BranchCode integer,IsDeleted bool default null,CreatedBy int,ModifiedDate varchar(40),ModifiedBy int, CreatedDate varchar(50),IsActive bool,CompanyCode integer)";
            SQLiteCommand commandCategoryTable = new SQLiteCommand(createCategoryTable, m_dbConnection);
            commandCategoryTable.ExecuteNonQuery();
            /*******************************End *********************************/



            /*************************LabelSetting Table*************************/
            string createLabelSettingTable = "create table IF NOT EXISTS LabelSetting (Id integer primary key AUTOINCREMENT,LabelSettingCode int, ItemId int,PrintItemCode bool,PrintItemDetail bool,PrintUnitMeasure varchar(50),PrintItemPrice bool,PrintBarCode bool,BarCodeHeight varchar(50),LabelSheet varchar(60),TotalNoOfPrints varchar(60),StartRow varchar(30),StartColumn varchar(30), CreatedDate varchar(40))";
            SQLiteCommand commandLabelSettingTable = new SQLiteCommand(createLabelSettingTable, m_dbConnection);
            commandLabelSettingTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Master LabelSetting Table*************************/
            string createMasterLabelSettingTable = "create table IF NOT EXISTS MasterLabelSetting (Id integer primary key AUTOINCREMENT,LabelSettingCode int,PrintItemCode bool,PrintItemDetail bool,PrintUnitMeasure varchar(50),PrintItemPrice bool,PrintBarCode bool,BarCodeHeight varchar(50),LabelSheet varchar(60),TotalNoOfPrints varchar(60),StartRow varchar(30),StartColumn varchar(30), CreatedDate varchar(40))";
            SQLiteCommand commandMasterLabelSettingTable = new SQLiteCommand(createMasterLabelSettingTable, m_dbConnection);
            commandMasterLabelSettingTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Product LabelSetting Table*************************/
            string createProductLabelSettingTable = "create table IF NOT EXISTS ProductLabelSetting (Id integer primary key AUTOINCREMENT,MasterLabelSettingCode int,ProductCode int,CreatedDate varchar(40),Quantity int)";
            SQLiteCommand commandProductLabelSettingTable = new SQLiteCommand(createProductLabelSettingTable, m_dbConnection);
            commandProductLabelSettingTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Product Table*************************/
            string createProductTable = "create table IF NOT EXISTS Product (Id integer primary key AUTOINCREMENT, ItemName varchar(200),CategoryCode int,RetailPrice decimal(15,2),TradePrice decimal(15,2),WholesellerPrice decimal(15,2),ResellerPrice decimal(15,2),ItemType int,Weight decimal(15,2), BarCode varchar(200),IsTaxInclusive bool,TaxPercentage decimal(15,2),MinimumLevel int,ReOrderLevel int, ItemImage BLOB,ShortName varchar(30),IsDeleted bool,CreatedBy int,ModifiedDate varchar(40),CreatedDate varchar(40),ModifiedBy int default null,Description varchar(500),ImageText varchar(2147483647),BranchCode integer,CompanyCode integer,BulkCode integer)";
            SQLiteCommand commandProductTable = new SQLiteCommand(createProductTable, m_dbConnection);/*,FOREIGN KEY (CategoryCode) REFERENCES Category(Id)*/
            commandProductTable.ExecuteNonQuery();
            /*******************************End *********************************/


            /*************************PurchaseOrder Table*************************/
            string createPurchaseOrderTable = "create table IF NOT EXISTS PurchaseOrder (Id integer primary key AUTOINCREMENT, PurchaseDate varchar(40),SuplierCode int,DiscountPercentage decimal(15,2),DiscountAmount decimal(15,2),DeliveryDate varchar(50),ExpiryDate varchar(50),SurChargeAmount decimal(15,2),TaxPercentage decimal(15,2),CreatedBy int,CreatedDate varchar(40),Status int,ApprovalDate varchar(40),ApprovedBy integer,CompanyCode integer,BranchCode integer,InvoiceNo varchar(40),InvoiceDate varchar(40))";
            SQLiteCommand cmdPurchaseOrderTable = new SQLiteCommand(createPurchaseOrderTable, m_dbConnection);
            cmdPurchaseOrderTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Purchase Table*************************/
            string createPurchaseTable = "create table IF NOT EXISTS Purchase (Id integer primary key AUTOINCREMENT, PurchaseDate varchar(40),SuplierCode int,DiscountPercentage decimal(15,2),DiscountAmount decimal(15,2),DeliveryDate varchar(50),ExpiryDate varchar(50),SurChargeAmount decimal(15,2),TaxPercentage decimal(15,2),CreatedBy int,CreatedDate varchar(40),CompanyCode integer,BranchCode integer)";
            SQLiteCommand cmdPurchaseTable = new SQLiteCommand(createPurchaseTable, m_dbConnection);
            cmdPurchaseTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************PurchaseReturn Table*************************/
            string createPurchaseReturnTable = "create table IF NOT EXISTS PurchaseReturn (Id integer primary key AUTOINCREMENT, PurchaseId integer,ProductCode integer,BatchNo varchar(40),Quantity int,ReturnBy int,ReturnDate varchar(40),CreatedDate varchar(40),Reason varchar(100))";
            SQLiteCommand cmdPurchaseReturnTable = new SQLiteCommand(createPurchaseReturnTable, m_dbConnection);
            cmdPurchaseReturnTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Wastage Table*************************/
            string createWastageTable = "create table IF NOT EXISTS Wastage (Id integer primary key AUTOINCREMENT,ItemCode int, ProductName varchar(40),Quantity int,Reason varchar(100),CreatedBy int,ModifiedBy int,ModifiedDate varchar(40),CreatedDate varchar(40),BatchNo varchar(40),BranchCode integer,CompanyCode integer)";
            SQLiteCommand commandWastageTable = new SQLiteCommand(createWastageTable, m_dbConnection);
            commandWastageTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************Supplier Table*************************/
            string createSupplierTable = "create table IF NOT EXISTS Supplier (Id integer primary key AUTOINCREMENT, SupplierName varchar(40), ShortName varchar(40), Address varchar(40), ContactName varchar(40), Telephone varchar(40), Mobile varchar(40), Fax varchar(40), WebsiteUrl varchar(40), Email varchar(40), Notes varchar(40),TaxInclusive bool,DiscountPercentage double,Tax double,IsDeleted bool,PurchaseType int,CreatedBy int,ModifiedBy int,ModifiedDate varchar(40),CreatedDate varchar(40),CompanyCode integer,BranchCode integer)";
            SQLiteCommand commandSupplierTable = new SQLiteCommand(createSupplierTable, m_dbConnection);
            commandSupplierTable.ExecuteNonQuery();
            /*******************************End *********************************/


            /*************************Stock Table*************************/
            string createStockTable = "create table IF NOT EXISTS Stock (Id integer primary key AUTOINCREMENT,PurchaseId integer,Quantity integer,CostPrice decimal(15,2),SellingPrice decimal(15,2),MRP decimal(15,2),ItemTaxPercentage decimal(15,2),BatchNo varchar(40),ProductCode integer,CreatedDate varchar(40),PurchaseOrderId integer)";
            SQLiteCommand commandStockTable = new SQLiteCommand(createStockTable, m_dbConnection);
            commandStockTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************OpeningStock Table*************************/
            string createOpeningStockTable = "create table IF NOT EXISTS OpeningStock (Id integer primary key AUTOINCREMENT, ProductCode int, Quantity int, BatchNo varchar(40),BranchCode integer, ExpiryDate varchar(40),CreatedBy int,CreatedDate varchar(40),CompanyCode integer)";
            SQLiteCommand commandOpeningStockTable = new SQLiteCommand(createOpeningStockTable, m_dbConnection);
            commandOpeningStockTable.ExecuteNonQuery();
            /*******************************End *********************************/

            /*************************StockAdjustment Table*************************/
            string createStockAdjustment = "create table IF NOT EXISTS StockAdjustment (Id integer primary key AUTOINCREMENT, ProductCode int, Quantity int,Reason varchar(250), BatchNo varchar(40), CompanyCode int, BranchCode int, ExpiryDate varchar(40),CreatedBy int,CreatedDate varchar(40))";
            SQLiteCommand commandStockAdjustmentTable = new SQLiteCommand(createStockAdjustment, m_dbConnection);
            commandStockAdjustmentTable.ExecuteNonQuery();

            /*************************SystemConfiguration Table*************************/
            string createSystemConfiguration = "create table IF NOT EXISTS SystemConfiguration (Id integer primary key AUTOINCREMENT, FinalYearStartDate varchar(40),  FinalYearEndDate varchar(40),CreatedDate varchar(40))";
            SQLiteCommand commandSystemConfiguration = new SQLiteCommand(createSystemConfiguration, m_dbConnection);
            commandSystemConfiguration.ExecuteNonQuery();

            /*************************Tax Table*************************/
            string createTax = "create table IF NOT EXISTS Tax (Id integer primary key AUTOINCREMENT,TaxDetail varchar(50),Rate double,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy  varchar(40))";
            SQLiteCommand commandTax = new SQLiteCommand(createTax, m_dbConnection);
            commandTax.ExecuteNonQuery();

            /*************************ProductItemContent Table*************************/
            string createProductItemContent = "create table IF NOT EXISTS ProductItemContent (Id integer primary key AUTOINCREMENT,SubProductId integer,Quantity int,MainProductId integer,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy  varchar(40),IsFreeProduct bool)";
            SQLiteCommand commandProductItemContent = new SQLiteCommand(createProductItemContent, m_dbConnection);
            commandProductItemContent.ExecuteNonQuery();

            /*************************ChildProductItem Table*************************/
            string createSubProductItem = "create table IF NOT EXISTS SubProductItem (Id integer primary key AUTOINCREMENT,Quantity int,RetailPrice decimal(15,2),ParentProductId integer,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy  varchar(40))";
            SQLiteCommand commandSubProductItem = new SQLiteCommand(createSubProductItem, m_dbConnection);
            commandSubProductItem.ExecuteNonQuery();

            /*************************ChildRepack Table*************************/
            string createRepack = "create table IF NOT EXISTS Repack (Id integer primary key AUTOINCREMENT,BulkCode integer,ProductCode integer,Quantity integer,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy int)";
            SQLiteCommand commandRepack = new SQLiteCommand(createRepack, m_dbConnection);
            commandRepack.ExecuteNonQuery();


            /*************************Coupon Management Table*************************/
            string createCouponManagmentItem = "create table IF NOT EXISTS Coupon (Id integer primary key AUTOINCREMENT,DiscountType int,CValue decimal(15,2),Name varchar(100),FromDate varchar(40),ToDate varchar(40),IsActive bool Default null,IsDelete bool Default null,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy  int,CompanyCode int,BranchCode integer,NoOfCoupons int)";
            SQLiteCommand commandCouponManagmentItem = new SQLiteCommand(createCouponManagmentItem, m_dbConnection);
            commandCouponManagmentItem.ExecuteNonQuery();


            /*************************Offer Management Table*************************/
            string createOfferManagmentItem = "create table IF NOT EXISTS Offer (Id integer primary key AUTOINCREMENT,OfferType int,MinimumValue decimal(15,2),Name varchar(100),FromDate varchar(40),ToDate varchar(40),IsActive bool Default null,IsDelete bool Default null,CreatedBy int,CreatedDate varchar(40),ModifiedDate varchar(40),ModifiedBy  int,CompanyCode int,BranchCode integer,Discount decimal(15,2))";
            SQLiteCommand commandOfferManagmentItem = new SQLiteCommand(createOfferManagmentItem, m_dbConnection);
            commandOfferManagmentItem.ExecuteNonQuery();


            /*************************Coupon Details Table*************************/
            string createOfferDetailItem = "create table IF NOT EXISTS OfferDetail (Id integer primary key AUTOINCREMENT,OfferCode int,ProductCode int,Discount decimal(15,2),FromDate varchar(40),ToDate varchar(40),DiscountType varchar(40),CreatedDate varchar(40))";
            SQLiteCommand commandOfferDetailItem = new SQLiteCommand(createOfferDetailItem, m_dbConnection);
            commandOfferDetailItem.ExecuteNonQuery();



            /*************************Coupon Details Table*************************/
            string createCouponDetailItem = "create table IF NOT EXISTS CouponDetail (Id integer primary key AUTOINCREMENT,CouponCode int,CouponNo varchar(100),IsRedeem bool default null,RedeemDate varchar(40),CreatedDate varchar(40))";
            SQLiteCommand commandCouponDetailItem = new SQLiteCommand(createCouponDetailItem, m_dbConnection);
            commandCouponDetailItem.ExecuteNonQuery();

            /*************************Payement to Supplier Details Table*************************/
            string createPaymentToSupplier = "create table IF NOT EXISTS PaymentToSupplier (Id integer primary key AUTOINCREMENT,SupplierCode int,Amount decimal(10,2),PaymentDate varchar(40),Description varchar(200),InvoiceNo integer,AccountNo varchar(40),CreatedBy int,CreatedDate varchar(40),ModifiedBy int,ModifiedDate varchar(40),PaymentType int,BankName varchar(40),CompanyCode integer,BranchCode integer,PurchaseType integer)";
            SQLiteCommand commandPaymentToSupplier = new SQLiteCommand(createPaymentToSupplier, m_dbConnection);
            commandPaymentToSupplier.ExecuteNonQuery();


            string selectTable = "SELECT COUNT(*) FROM User";
            SQLiteCommand commandCountTableRow = new SQLiteCommand(selectTable, m_dbConnection);
            //   commandCountTableRow.ExecuteNonQuery();
            //  cmd.CommandText = "SELECT COUNT(*) FROM table_name";
            int count = Convert.ToInt32(commandCountTableRow.ExecuteScalar());

            if (count == 0)
            {
                ///******************************************Insert Query************************/

                ///**********************CompanyTable****************************/
                string insertCompanyTable = "Insert into Company values(2,'', '','',0,1,'','','','','')";
                SQLiteCommand commandInsertCompanyTable = new SQLiteCommand(insertCompanyTable, m_dbConnection);
                commandInsertCompanyTable.ExecuteNonQuery();
                ///**********************************End************************/


                /************User Table****************/
                string insertUserTable = "insert into User values(1,1,'','' ,0,'','',0,'','', '',1 ,'')";
                SQLiteCommand commandInsertUserTable = new SQLiteCommand(insertUserTable, m_dbConnection);
                commandInsertUserTable.ExecuteNonQuery();
                /*******************************End ************************/




                /*************************Branch Table*************************/
                string insertBranchTable = "insert into  Branch values(1,1,'','','','',0,1,'','', '','')";
                SQLiteCommand commandInsertBranchTable = new SQLiteCommand(insertBranchTable, m_dbConnection);
                commandInsertBranchTable.ExecuteNonQuery();
                /*******************************End *********************************/



                /*************************Branch Table*************************/
                string insertCategoryTable = "insert into Category values(1,'','',1,null,1,'',1,'',1,1)";
                SQLiteCommand commandInsertCategoryTable = new SQLiteCommand(insertCategoryTable, m_dbConnection);
                commandInsertCategoryTable.ExecuteNonQuery();
                /*******************************End *********************************/



                /*************************Label Table*************************/
                string insertLabelSettingTable = "insert into LabelSetting values(1,1,1,0,0,'',0,0,'','','','','','')";
                SQLiteCommand commandInsertLabelSettingTable = new SQLiteCommand(insertLabelSettingTable, m_dbConnection);
                commandInsertLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Master Label Table*************************/
                string insertMasterLabelSettingTable = "insert into MasterLabelSetting values(1,1,0,0,'',0,0,'','','','','','')";
                SQLiteCommand commandInsertMasterLabelSettingTable = new SQLiteCommand(insertMasterLabelSettingTable, m_dbConnection);
                commandInsertMasterLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Product Label Table*************************/
                string insertProductLabelSettingTable = "insert into ProductLabelSetting values(1,1,0,'',0)";
                SQLiteCommand commandInsertProductLabelSettingTable = new SQLiteCommand(insertProductLabelSettingTable, m_dbConnection);
                commandInsertProductLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Product Table*************************/
                string insertProductTable = "insert into Product values(1,'',1,1.0,1.0,1.0,1.0,1,1.0,'',0,1.0,1,1,'','',0,1,'','',null,'',1,null,1,1)";
                SQLiteCommand commandInsertProductTable = new SQLiteCommand(insertProductTable, m_dbConnection);
                commandInsertProductTable.ExecuteNonQuery();
                /*******************************End *********************************/


                /*************************PurchaseOrder Table*************************/
                string insertPurchaseOrderTable = "insert into PurchaseOrder values(1,'',1,1.0,1.0,'','',1.0,1.0,1,'',1,'',1,1,1,'','')";
                SQLiteCommand cmdInsertPurchaseOrderTable = new SQLiteCommand(insertPurchaseOrderTable, m_dbConnection);
                cmdInsertPurchaseOrderTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Purchase Table*************************/
                string insertPurchaseTable = "insert into Purchase values(1,'',1,1.0,1.0,'','',1.0,1.0,1,'',1,1)";
                SQLiteCommand cmdInsertPurchaseTable = new SQLiteCommand(insertPurchaseTable, m_dbConnection);
                cmdInsertPurchaseTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************PurchaseReturn Table*************************/
                string insertPurchaseReturnTable = "insert into PurchaseReturn values(1,1,1,'',1,1,'','','')";
                SQLiteCommand cmdInsertPurchaseReturnTable = new SQLiteCommand(insertPurchaseReturnTable, m_dbConnection);
                cmdInsertPurchaseReturnTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Wastage Table*************************/
                string insertWastageTable = "insert into Wastage values(1,1,'',1,'',1,1,'','','',1,1)";
                SQLiteCommand commandInsertWastageTable = new SQLiteCommand(insertWastageTable, m_dbConnection);
                commandInsertWastageTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Supplier Table*************************/
                string insertSupplierTable = "insert into Supplier values(1,'','','','','','','','','','',0,1.0,1.0,0,1,1,1,'','',1,1)";
                SQLiteCommand commandInsertSupplierTable = new SQLiteCommand(insertSupplierTable, m_dbConnection);
                commandInsertSupplierTable.ExecuteNonQuery();
                /*******************************End *********************************/


                /*************************Stock Table*************************/
                string insertStockTable = "insert into Stock values(1,1,1,1.0,1.0,1.0,1.0,'',1,'',1)";
                SQLiteCommand commandInsertStockTable = new SQLiteCommand(insertStockTable, m_dbConnection);
                commandInsertStockTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************OpeningStock Table*************************/
                string insertOpeningStockTable = "insert into OpeningStock values(1, 1,1, '',1,'',1,'',1)";
                SQLiteCommand commandInsertOpeningStockTable = new SQLiteCommand(insertOpeningStockTable, m_dbConnection);
                commandInsertOpeningStockTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************StockAdjustment Table*************************/
                string insertStockAdjustment = "insert into StockAdjustment values(1,1,1,'','',1,1,'',1,'')";
                SQLiteCommand commandInsertStockAdjustmentTable = new SQLiteCommand(insertStockAdjustment, m_dbConnection);
                commandInsertStockAdjustmentTable.ExecuteNonQuery();

                /*************************SystemConfiguration Table*************************/
                string insertSystemConfiguration = "insert into SystemConfiguration values(1,'','','')";
                SQLiteCommand commandInsertSystemConfiguration = new SQLiteCommand(insertSystemConfiguration, m_dbConnection);
                commandInsertSystemConfiguration.ExecuteNonQuery();

                /*************************Tax Table*************************/
                string insertTax = "insert into Tax values(1,'',1.0,1,'','',1)";
                SQLiteCommand commandInsertTax = new SQLiteCommand(insertTax, m_dbConnection);
                commandInsertTax.ExecuteNonQuery();

                /*************************ProductItemContent Table*************************/
                string insertProductItemContent = "insert into ProductItemContent values(1,1,1,1,1,'','',1,0)";
                SQLiteCommand commandInsertProductItemContent = new SQLiteCommand(insertProductItemContent, m_dbConnection);
                commandInsertProductItemContent.ExecuteNonQuery();

                /*************************ChildProductItem Table*************************/
                string insertSubProductItem = "insert into SubProductItem values(1,1,1.0,1,1,'','',1)";
                SQLiteCommand commandInsertSubProductItem = new SQLiteCommand(insertSubProductItem, m_dbConnection);
                commandInsertSubProductItem.ExecuteNonQuery();

                /*************************ChildProductItem Table*************************/
                string insertRepack = "insert into Repack values(1,1,1,1,1,'','',1)";
                SQLiteCommand commandInsertRepack = new SQLiteCommand(insertRepack, m_dbConnection);
                commandInsertRepack.ExecuteNonQuery();

                /*************************Coupon Managment Table*************************/
                string insertCouponManagmentItem = "insert into Coupon values(0,0,1.0,'','','',1,0,1,'','',1,1,1,1)";
                SQLiteCommand commandCouponManagmentInsertItem = new SQLiteCommand(insertCouponManagmentItem, m_dbConnection);
                commandCouponManagmentInsertItem.ExecuteNonQuery();

                /*************************Offer Managment Table*************************/
                string insertOfferManagmentItem = "insert into Offer values(0,0,1.0,'','','',1,0,1,'','',1,1,1,1.0)";
                SQLiteCommand commandOfferManagmentInsertItem = new SQLiteCommand(insertOfferManagmentItem, m_dbConnection);
                commandOfferManagmentInsertItem.ExecuteNonQuery();

                /*************************Coupon Managment Table*************************/
                string insertCouponDetailsItem = "insert into CouponDetail values(0,0,'',0,'','')";
                SQLiteCommand commandCouponDetailsInsertItem = new SQLiteCommand(insertCouponDetailsItem, m_dbConnection);
                commandCouponDetailsInsertItem.ExecuteNonQuery();

                /*************************OfferDetail  Table*************************/
                string insertOfferDetailItem = "insert into OfferDetail values(1,1,1,1.0,'','','','')";
                SQLiteCommand commandOfferDetailInsertItem = new SQLiteCommand(insertOfferDetailItem, m_dbConnection);
                commandOfferDetailInsertItem.ExecuteNonQuery();


                /*************************PaymentToSupplier Managment Table*************************/
                string insertPaymentToSupplier = "insert into PaymentToSupplier values(1,0,1.0,'','',0,'',0,'',0,'',0,'',0,0,0)";
                SQLiteCommand commandPaymentToSupplierData = new SQLiteCommand(insertPaymentToSupplier, m_dbConnection);
                commandPaymentToSupplierData.ExecuteNonQuery();

                ///******************************************Delete Query***************************/

                ///**********************CompanyTable****************************/
                string deleteCompanyTable = "Delete from Company";
                SQLiteCommand commandDeleteCompanyTable = new SQLiteCommand(deleteCompanyTable, m_dbConnection);
                commandDeleteCompanyTable.ExecuteNonQuery();
                /**********************************End************************/


                /************User Table****************/
                string deleteUserTable = "Delete from User";
                SQLiteCommand commandDeleteUserTable = new SQLiteCommand(deleteUserTable, m_dbConnection);
                commandDeleteUserTable.ExecuteNonQuery();
                /*******************************End ************************/




                /*************************Branch Table*************************/
                string deleteBranchTable = "Delete from  Branch";
                SQLiteCommand commandDeleteBranchTable = new SQLiteCommand(deleteBranchTable, m_dbConnection);
                commandDeleteBranchTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Product Table*************************/
                string deleteProductTable = "delete from Product";
                SQLiteCommand commandDeleteProductTable = new SQLiteCommand(deleteProductTable, m_dbConnection);
                commandDeleteProductTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Branch Table*************************/
                string deleteCategoryTable = "delete from Category";
                SQLiteCommand commandDeleteCategoryTable = new SQLiteCommand(deleteCategoryTable, m_dbConnection);
                commandDeleteCategoryTable.ExecuteNonQuery();
                /*******************************End *********************************/



                /*************************Branch Table*************************/
                string deleteLabelSettingTable = "delete from LabelSetting";
                SQLiteCommand commandDeleteLabelSettingTable = new SQLiteCommand(deleteLabelSettingTable, m_dbConnection);
                commandDeleteLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Master Label Table*************************/
                string deleteMasterLabelSettingTable = "delete from MasterLabelSetting";
                SQLiteCommand commandDeleteMasterLabelSettingTable = new SQLiteCommand(deleteMasterLabelSettingTable, m_dbConnection);
                commandDeleteMasterLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Product Label Table*************************/
                string deleteProductLabelSettingTable = "delete from ProductLabelSetting";
                SQLiteCommand commandDeleteProductLabelSettingTable = new SQLiteCommand(deleteProductLabelSettingTable, m_dbConnection);
                commandDeleteProductLabelSettingTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Purchase Table*************************/
                string deletePurchaseOrderTable = "Delete from PurchaseOrder";
                SQLiteCommand cmdDeletePurchaseOrderTable = new SQLiteCommand(deletePurchaseOrderTable, m_dbConnection);
                cmdDeletePurchaseOrderTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Purchase Table*************************/
                string deletePurchaseTable = "Delete from Purchase";
                SQLiteCommand cmdDeletePurchaseTable = new SQLiteCommand(deletePurchaseTable, m_dbConnection);
                cmdDeletePurchaseTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************PurchaseReturn Table*************************/
                string deletePurchaseReturnTable = "delete from PurchaseReturn";
                SQLiteCommand cmdDeletePurchaseReturnTable = new SQLiteCommand(deletePurchaseReturnTable, m_dbConnection);
                cmdDeletePurchaseReturnTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Wastage Table*************************/
                string deleteWastageTable = "delete from Wastage";
                SQLiteCommand commandDeleteWastageTable = new SQLiteCommand(deleteWastageTable, m_dbConnection);
                commandDeleteWastageTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************Supplier Table*************************/
                string deleteSupplierTable = "delete from Supplier";
                SQLiteCommand commandDeleteSupplierTable = new SQLiteCommand(deleteSupplierTable, m_dbConnection);
                commandDeleteSupplierTable.ExecuteNonQuery();
                /*******************************End *********************************/


                /*************************Stock Table*************************/
                string deleteStockTable = "Delete from Stock";
                SQLiteCommand commandDeleteStockTable = new SQLiteCommand(deleteStockTable, m_dbConnection);
                commandDeleteStockTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************OpeningStock Table*************************/
                string deleteOpeningStockTable = "Delete from OpeningStock";
                SQLiteCommand commandDeleteOpeningStockTable = new SQLiteCommand(deleteOpeningStockTable, m_dbConnection);
                commandDeleteOpeningStockTable.ExecuteNonQuery();
                /*******************************End *********************************/

                /*************************StockAdjustment Table*************************/
                string deleteStockAdjustment = "delete from StockAdjustment";
                SQLiteCommand commandDeleteStockAdjustmentTable = new SQLiteCommand(deleteStockAdjustment, m_dbConnection);
                commandDeleteStockAdjustmentTable.ExecuteNonQuery();

                /*************************SystemConfiguration Table*************************/
                string deleteSystemConfiguration = "delete from SystemConfiguration";
                SQLiteCommand commandDeleteSystemConfiguration = new SQLiteCommand(deleteSystemConfiguration, m_dbConnection);
                commandDeleteSystemConfiguration.ExecuteNonQuery();

                /*************************Tax Table*************************/
                string deleteTax = "delete from Tax";
                SQLiteCommand commandDeleteTax = new SQLiteCommand(deleteTax, m_dbConnection);
                commandDeleteTax.ExecuteNonQuery();

                /*************************ProductItemContent Table*************************/
                string deleteProductItemContent = "delete from ProductItemContent";
                SQLiteCommand commandDeleteProductItemContent = new SQLiteCommand(deleteProductItemContent, m_dbConnection);
                commandDeleteProductItemContent.ExecuteNonQuery();

                /*************************SubProductItem Table*************************/
                string deleteSubProductItem = "delete from SubProductItem";
                SQLiteCommand commandDeleteSubProductItem = new SQLiteCommand(deleteSubProductItem, m_dbConnection);
                commandDeleteSubProductItem.ExecuteNonQuery();
                /*************************OfferManagmentItem Table*************************/
                string deleteCouponItem = "delete from Coupon";
                SQLiteCommand commandDeleteCouponItem = new SQLiteCommand(deleteCouponItem, m_dbConnection);
                commandDeleteCouponItem.ExecuteNonQuery();

                /*************************CouponManagmentItem Table*************************/
                string deleteOfferDetailItem = "delete from OfferDetail";
                SQLiteCommand commandDeleteOfferDetailItem = new SQLiteCommand(deleteOfferDetailItem, m_dbConnection);
                commandDeleteOfferDetailItem.ExecuteNonQuery();

                /*************************OfferManagmentItem Table*************************/
                string deleteOfferItem = "delete from Offer";
                SQLiteCommand commandDeleteOfferItem = new SQLiteCommand(deleteOfferItem, m_dbConnection);
                commandDeleteOfferItem.ExecuteNonQuery();

                /*************************Coupon Detail Item Table*************************/
                string deleteCouponDetailItem = "delete from CouponDetail";
                SQLiteCommand commandDeleteCouponDetailItem = new SQLiteCommand(deleteCouponDetailItem, m_dbConnection);
                commandDeleteCouponDetailItem.ExecuteNonQuery();

                /*************************SubProductItem Table*************************/
                string deleteRepack = "delete from Repack";
                SQLiteCommand commandDeleteRepack = new SQLiteCommand(deleteRepack, m_dbConnection);
                commandDeleteRepack.ExecuteNonQuery();

                /*************************PaymentToSupplier Table*************************/
                string deletePaymentToSupplier = "delete from PaymentToSupplier";
                SQLiteCommand commandDeletePaymentToSupplier = new SQLiteCommand(deletePaymentToSupplier, m_dbConnection);
                commandDeletePaymentToSupplier.ExecuteNonQuery();

            }

            /*************************Update SqlLite Sequence*************************/

            string updateDatabaseSequence = "update sqlite_sequence set seq=1000";
            SQLiteCommand commandUpdateDatabaseSequence = new SQLiteCommand(updateDatabaseSequence, m_dbConnection);
            commandUpdateDatabaseSequence.ExecuteNonQuery();
            //sql = "insert into companyData (Id,Name, Code) values (2,'Mike', '9002')";

            //command = new SQLiteCommand(sql, m_dbConnection);
            //command.ExecuteNonQuery();
            m_dbConnection.Close();

            CreateServiceHostInstance(couponManagmentservice, categoryservice, companyservice, openningStockservice, productservice, purchaseservice, stockAdjustmentservice, systemConfigurationservice, taxservice, userservice, supplierservice);

            OpenHost();
        }

        private static void SetKernel(out WcfHost.Interface.ICouponManagmentService couponManagmentservice, out WcfHost.Interface.ICategoryService categoryservice, out WcfHost.Interface.ICompanyService companyservice, out WcfHost.Interface.IOpeningStockService openningStockservice, out WcfHost.Interface.IProductService productservice, out WcfHost.Interface.IPurchaseService purchaseservice, out WcfHost.Interface.IStockAdjustmentService stockAdjustmentservice, out WcfHost.Interface.ISystemConfigurationService systemConfigurationservice, out WcfHost.Interface.ITaxService taxservice, out WcfHost.Interface.IUserService userservice, out WcfHost.Interface.ISupplierService supplierservice)
        {
            IKernel _kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());  //Load Module          
            //IFinPosService customerService = _kernel.Get<IFinPosService>();
            couponManagmentservice = _kernel.Get<FinPos.WcfHost.Services.CouponManagmentService>();
            categoryservice = _kernel.Get<FinPos.WcfHost.Services.CategoryService>();
            companyservice = _kernel.Get<FinPos.WcfHost.Services.CompanyService>();
            openningStockservice = _kernel.Get<FinPos.WcfHost.Services.OpeningStockService>();
            productservice = _kernel.Get<FinPos.WcfHost.Services.ProductService>();
            purchaseservice = _kernel.Get<FinPos.WcfHost.Services.PurchaseService>();
            stockAdjustmentservice = _kernel.Get<FinPos.WcfHost.Services.StockAdjustmentService>();
            systemConfigurationservice = _kernel.Get<FinPos.WcfHost.Services.SystemConfigurationService>();
            taxservice = _kernel.Get<FinPos.WcfHost.Services.TaxService>();
            userservice = _kernel.Get<FinPos.WcfHost.Services.UserService>();
            supplierservice = _kernel.Get<FinPos.WcfHost.Services.SupplierService>();
        }

        private static void OpenHost()
        {
            couponManagmentServiceHost.Open();
            categoryServiceHost.Open();
            companyServiceHost.Open();
            OpeningStockServiceHost.Open();
            stockAdjustmentServiceHost.Open();
            ProductServiceHost.Open();
            supplier.Open();
            purchaseServiceHost.Open();
            systemConfigurationServiceHost.Open();
            taxServiceHost.Open();
            userServiceHost.Open();
        }

        private static void CreateServiceHostInstance(WcfHost.Interface.ICouponManagmentService couponManagmentservice, WcfHost.Interface.ICategoryService categoryservice, WcfHost.Interface.ICompanyService companyservice, WcfHost.Interface.IOpeningStockService openningStockservice, WcfHost.Interface.IProductService productservice, WcfHost.Interface.IPurchaseService purchaseservice, WcfHost.Interface.IStockAdjustmentService stockAdjustmentservice, WcfHost.Interface.ISystemConfigurationService systemConfigurationservice, WcfHost.Interface.ITaxService taxservice, WcfHost.Interface.IUserService userservice, WcfHost.Interface.ISupplierService supplierservice)
        {
            categoryServiceHost = new ServiceHost(categoryservice);
            companyServiceHost = new ServiceHost(companyservice);
            couponManagmentServiceHost = new ServiceHost(couponManagmentservice);
            OpeningStockServiceHost = new ServiceHost(openningStockservice);
            stockAdjustmentServiceHost = new ServiceHost(stockAdjustmentservice);
            ProductServiceHost = new ServiceHost(productservice);
            supplier = new ServiceHost(supplierservice);
            purchaseServiceHost = new ServiceHost(purchaseservice);
            systemConfigurationServiceHost = new ServiceHost(systemConfigurationservice);
            taxServiceHost = new ServiceHost(taxservice);
            userServiceHost = new ServiceHost(userservice);
        }

        private static void CloseServiceInstance()
        {
            if (supplier != null)
            {
                supplier.Close();
            }
            if (OpeningStockServiceHost != null)
            {
                OpeningStockServiceHost.Close();
            }
            if (ProductServiceHost != null)
            {
                ProductServiceHost.Close();
            }
            if (purchaseServiceHost != null)
            {
                purchaseServiceHost.Close();
            }
            if (stockAdjustmentServiceHost != null)
            {
                stockAdjustmentServiceHost.Close();
            }
            if (systemConfigurationServiceHost != null)
            {
                systemConfigurationServiceHost.Close();
            }
            if (taxServiceHost != null)
            {
                taxServiceHost.Close();
            }
            if (userServiceHost != null)
            {
                userServiceHost.Close();
            }
            if (couponManagmentServiceHost != null)
            {
                couponManagmentServiceHost.Close();
            }
            if (categoryServiceHost != null)
            {
                categoryServiceHost.Close();
            }
            if (companyServiceHost != null)
            {
                companyServiceHost.Close();
            }
        }

        protected override void OnStop()
        {
            couponManagmentServiceHost.Close();
            categoryServiceHost.Close();
            companyServiceHost.Close();
            OpeningStockServiceHost.Close();
            stockAdjustmentServiceHost.Close();
            ProductServiceHost.Close();
            supplier.Close();
            purchaseServiceHost.Close();
            systemConfigurationServiceHost.Close();
            taxServiceHost.Close();
            userServiceHost.Close();

        }
    }
}

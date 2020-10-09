using System.Data.Entity;
using FinPos.DAL.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using FinPos.Data.Entities;


namespace FinPos.Data
{

    public class FinPosDbContext : DbContext
    {
        public FinPosDbContext() : base("name=FinPosDbContext")
        {

        }
        public DbSet<Company> companyData { get; set; }
        public DbSet<User> userData { get; set; }
        public DbSet<Branch> branchData { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<PurchaseOrder> Purchase { get; set; }

        public DbSet<Purchase> DirectPurchase { get; set; }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<LabelSetting> LabelSetting { get; set; }
        public DbSet<Wastage> Wastage { get; set; }

        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<OpeningStock> OpeningStock { get; set; }

        public DbSet<StockAdjustment> StockAdjustment { get; set; }

        public DbSet<PurchaseReturn> PurchaseReturn { get; set; }
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; }
        public DbSet<MasterLabelSetting> MasterLabelSetting { get; set; }
        public DbSet<ProductLabelSetting> ProductLabelSetting { get; set; }
        public DbSet<Tax> Tax { get; set; }
        public DbSet<ProductItemContent> ProductItemContent { get; set; }
        public DbSet<SubProductItem> SubProductItem { get; set; }
        public DbSet<Repack> Repack { get; set; }

        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<CouponDetail> CouponDetail { get; set; }
        public DbSet<PaymentToSupplier> PaymentToSupplier { get; set; }
        public DbSet<OfferDetail> OfferDetail { get; set; }
        public DbSet<Offer> Offer { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

}

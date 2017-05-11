using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BetterEntityFramework.StoreData
{
    public partial class EfStoreContext : DbContext
    {
        private readonly DbContextOptions _options;

        public EfStoreContext()
        {

        }

        public EfStoreContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategorySubscription> CategorySubscription { get; set; }
        public virtual DbSet<CategoryTree> CategoryTree { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductBundle> ProductBundle { get; set; }
        public virtual DbSet<ProductSku> ProductSku { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserBasket> UserBasket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
            {
                optionsBuilder.UseSqlServer(@"Server=(local);Database=EfStore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.SystemId)
                    .HasName("IX_Category")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<CategorySubscription>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId)
                    .HasName("PK_CategorySubscription");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.CategorySubscription)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategorySubscription_Category");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.CategorySubscription)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategorySubscription_Product");
            });

            modelBuilder.Entity<CategoryTree>(entity =>
            {
                entity.HasKey(e => e.TreeId)
                    .HasName("PK_CategoryTree");

                entity.HasOne(d => d.ChildCategoryNavigation)
                    .WithMany(p => p.CategoryTreeChildCategoryNavigation)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.ChildCategory)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryTree_Category1");

                entity.HasOne(d => d.ParentCategoryNavigation)
                    .WithMany(p => p.CategoryTreeParentCategoryNavigation)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.ParentCategory)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryTree_Category");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.SystemId)
                    .HasName("IX_Product")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<ProductBundle>(entity =>
            {
                entity.HasKey(e => e.BundleId)
                    .HasName("PK_ProductBundle");

                entity.Property(e => e.AssociatedQuantity).HasDefaultValueSql("1");

                entity.Property(e => e.AssociatedSku)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.RootSku)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.HasOne(d => d.AssociatedProductNavigation)
                    .WithMany(p => p.ProductBundleAssociatedProductNavigation)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.AssociatedProduct)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductBundle_Product1");

                entity.HasOne(d => d.RootProductNavigation)
                    .WithMany(p => p.ProductBundleRootProductNavigation)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.RootProduct)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductBundle_Product");
            });

            modelBuilder.Entity<ProductSku>(entity =>
            {
                entity.HasKey(e => e.SkuId)
                    .HasName("PK_ProductSku");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnName("SKU")
                    .HasColumnType("varchar(10)");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.ProductSku)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductSku_Product");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Identity)
                    .HasName("IX_User")
                    .IsUnique();

                entity.Property(e => e.ContactPreference).HasColumnType("varchar(10)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<UserBasket>(entity =>
            {
                entity.HasKey(e => e.BasketId)
                    .HasName("PK_UserBasket");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.UserBasket)
                    .HasPrincipalKey(p => p.SystemId)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserBasket_Product");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.UserBasket)
                    .HasPrincipalKey(p => p.Identity)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserBasket_User");
            });
        }
    }
}
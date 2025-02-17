using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SWD392.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt) 
        {
            
        }
        #region DbSet
        public DbSet<Product>? products { get; set; }
        public DbSet<Image>? images { get; set; }  
        public DbSet<Unit>? units { get; set; }
        public DbSet<Brand>? brands { get; set; }
        public DbSet<Packaging>? packagings { get; set; }
        public DbSet<Solution>? solutions { get; set; }
        public DbSet<Category>? categories { get; set; }
        public DbSet<BrandOrigin>? brandOrigins { get; set; }
        public DbSet<Manufacturer>? manufacturers { get; set; }
        public DbSet<ManufacturedCountry>? manufacturedCountries { get; set; }
        public DbSet<ProductDetail>? productDetails { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 🛑 Quan trọng để Identity hoạt động

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Product)
                .WithMany(s => s.Images)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(up => up.Unit)
                .WithMany(u => u.products)
                .HasForeignKey(up => up.UnitId);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.Brand)
                .WithMany(b => b.products)
                .HasForeignKey(s => s.BrandId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Packaging)
                .WithMany(p => p.products)
                .HasForeignKey(p => p.PackagingId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Solution)
                .WithMany(s => s.Categories)
                .HasForeignKey(c => c.SolutionId);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.Category)
                .WithMany(c => c.products)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.BrandOrigin)
                .WithMany(b => b.products)
                .HasForeignKey(s => s.BrandOriginId);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.Manufacturer)
                .WithMany(m => m.products)
                .HasForeignKey(s => s.ManufacturerId);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.ManufacturedCountry)
                .WithMany(c => c.products)
                .HasForeignKey(s => s.ManufacturedCountryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductDetail)
                .WithMany(pd => pd.products)
                .HasForeignKey(p => p.ProductDetailId);

        }
    }
}

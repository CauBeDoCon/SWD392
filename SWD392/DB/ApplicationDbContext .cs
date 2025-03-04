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
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
        public DbSet<Wallet>? Wallets { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }
        public DbSet<CartProduct>? cartProducts { get; set; }
        public DbSet<Cart>? carts { get; set; }
        public DbSet<DiscountCategory>? discountCategories { get; set; }
        public DbSet<Discount>? discounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 🛑 Quan trọng để Identity hoạt động

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<ApplicationUser>(u => u.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<CartProduct>()
                .HasOne(i => i.Product)
                .WithMany(s => s.CartProducts)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<CartProduct>()
                .HasOne(i => i.Cart)
                .WithMany(s => s.CartProducts)
                .HasForeignKey(i => i.CartId);

            modelBuilder.Entity<Order>()
                .HasOne(i => i.Cart)
                .WithMany(s => s.Orders)
                .HasForeignKey(i => i.CartId);

            modelBuilder.Entity<Cart>()
                .HasOne(r => r.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<ApplicationUser>(c => c.CartId);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(c => c.CartId)
                .IsUnique();

            modelBuilder.Entity<Blog>()
                .HasOne(i => i.User)
                .WithMany(s => s.Blogs)
                .HasForeignKey(i => i.UserId);

        }
    }
}

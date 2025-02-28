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
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<ResultQuiz> ResultQuizzes { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RecommendProduct> RecommendProducts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<BookingResult> BookingResults { get; set; }
        public DbSet<TimeFrame> TimeFrames { get; set; }


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

            modelBuilder.Entity<Notification>()
                .HasOne(i => i.User)
                .WithMany(s => s.Notifications)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(i => i.User)
                .WithMany(s => s.Comments)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(i => i.User)
                .WithMany(s => s.Reviews)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Comment)
                .WithOne(c => c.Review)
                .HasForeignKey<Comment>(c => c.ReviewId);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.ReviewId)
                .IsUnique();

            modelBuilder.Entity<OrderDetail>()
                .HasOne(r => r.Review)
                .WithOne(c => c.OrderDetail)
                .HasForeignKey<Review>(c => c.OrderDetailId);

            modelBuilder.Entity<Review>()
                .HasIndex(c => c.OrderDetailId)
                .IsUnique();

            modelBuilder.Entity<ShippingMethod>()
                .HasOne(r => r.Shipping)
                .WithOne(c => c.ShippingMethod)
                .HasForeignKey<Shipping>(c => c.ShippingMethodId);

            modelBuilder.Entity<Shipping>()
                .HasIndex(c => c.ShippingMethodId)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasOne(r => r.Shipping)
                .WithOne(c => c.Order)
                .HasForeignKey<Shipping>(c => c.OrderId);

            modelBuilder.Entity<Shipping>()
                .HasIndex(c => c.OrderId)
                .IsUnique();

            modelBuilder.Entity<Shipping>()
                .HasOne(r => r.Return)
                .WithOne(c => c.Shipping)
                .HasForeignKey<Return>(c => c.ShippingId);

            modelBuilder.Entity<Return>()
                .HasIndex(c => c.ShippingId)
                .IsUnique();

            modelBuilder.Entity<Return>()
                .HasOne(i => i.Order)
                .WithMany(s => s.Returns)
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<Return>()
                .HasOne(i => i.Product)
                .WithMany(s => s.Returns)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<ResultQuiz>()
                .HasOne(i => i.User)
                .WithMany(s => s.ResultQuizzes)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Routine>()
                .HasOne(i => i.ResultQuiz)
                .WithMany(s => s.Routines)
                .HasForeignKey(i => i.ResultQuizId);

            modelBuilder.Entity<RecommendProduct>()
                .HasOne(i => i.Routine)
                .WithMany(s => s.RecommendProducts)
                .HasForeignKey(i => i.RoutineId);

            modelBuilder.Entity<RecommendProduct>()
                .HasOne(i => i.Product)
                .WithMany(s => s.RecommendProducts)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<BookingHistory>()
                .HasOne(i => i.Booking)
                .WithMany(s => s.BookingHistories)
                .HasForeignKey(i => i.BookingId);

            modelBuilder.Entity<Booking>()
                .HasOne(r => r.BookingResult)
                .WithOne(c => c.Booking)
                .HasForeignKey<BookingResult>(c => c.BookingId);

            modelBuilder.Entity<BookingResult>()
                .HasIndex(c => c.BookingId)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasOne(i => i.User)
                .WithMany(s => s.Bookings)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<TimeFrame>()
                .HasOne(i => i.User)
                .WithMany(s => s.TimeFrames)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(i => i.TimeFrame)
                .WithMany(s => s.Bookings)
                .HasForeignKey(i => i.TimeFrameId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

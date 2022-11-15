using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Models.Auth;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using StepEbay.Data.Models.Telegram;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public ApplicationDbContext()
            : base()
        {

        }

        #region Auth
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region Bet
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseType> PurchaseTypes { get; set; }
        public virtual DbSet<PurchaseState> PurchaseStates { get; set; }
        #endregion

        #region Users
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        #endregion

        #region Products
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductState> ProductStates { get; set; }
        public virtual DbSet<ProductDesc> ProductDesc { get; set; }
        #endregion

        #region Telegram
        public virtual DbSet<DeveloperGroup> DeveloperGroups { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //index
            builder.Entity<RefreshToken>().HasIndex(x => x.UpdateTime);

            builder.Entity<Role>().HasIndex(x => x.Name);

            builder.Entity<User>().HasIndex(x => x.FullName);
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<User>().HasIndex(x => x.NickName).IsUnique();

            builder.Entity<Product>().HasIndex(x => x.Title);
            builder.Entity<ProductState>().HasIndex(x => x.Name);

            builder.Entity<Category>().HasIndex(x => x.Name);

            //default values
            builder.Entity<Purchase>().Property(b => b.PurchaseStateId).HasDefaultValue(1);
            builder.Entity<Product>().Property(b => b.IsActive).HasDefaultValue(true);

            //toTable
            builder.Entity<RefreshToken>().ToTable("RefreshTokens");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UserRoles");

            builder.Entity<Purchase>().ToTable("Purchases");
            builder.Entity<PurchaseType>().ToTable("PurchaseTypes");
            builder.Entity<PurchaseState>().ToTable("PurchaseStates");

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Favorite>().ToTable("Favorites");

            builder.Entity<Product>().ToTable("Products");
            builder.Entity<ProductState>().ToTable("ProductStates");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<ProductDesc>().ToTable("Description");

            builder.Entity<DeveloperGroup>().ToTable("DeveloperGroups");

            //builder.Entity<Favorite>().HasOne(x => x.User).WithMany(x => x.Favorites).OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Favorite>().HasOne(x => x.Product).WithMany(x => x.Favorites).OnDelete(DeleteBehavior.NoAction);
            //builder.Entity<Purchase>().HasOne(x => x.User).WithMany(x => x.Purchases).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Purchase>().HasOne(x => x.Product).WithMany(x => x.Purchases).OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<User>().HasMany(x => x.Purchases).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<User>().HasMany(x => x.Favorites).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Product>().HasMany(x => x.Purchases).WithOne(x => x.Product).OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<Product>().HasMany(x => x.Purchases).WithOne(x => x.Product).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

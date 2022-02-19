using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Models.Auth;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        #region Auth
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Role> Roles {get; set;}
        public virtual DbSet<UserRole> UserRoles { get; set;}
        #endregion

        #region Users
        public virtual DbSet<User> Users { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>().HasIndex(x => x.UpdateTime);

            builder.Entity<Role>().HasIndex(x => x.Name);

            builder.Entity<User>().HasIndex(x => x.FullName);
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<User>().HasIndex(x => x.NickName).IsUnique();

            builder.Entity<RefreshToken>().ToTable("RefreshTokens");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<User>().ToTable("Users");

        }
    }
}

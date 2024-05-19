using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;

namespace University.Infrastructure.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Students_Addresses> Addresses { get; set; }
        public DbSet<Users_Accounts> Accounts { get; set; }
        public DbSet<Users_Accounts_Roles> UserAccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users_Accounts_Roles>()
                .HasKey(uar => new { uar.AccountId, uar.RoleId });

            modelBuilder.Entity<Users_Accounts_Roles>()
                .HasOne(uar => uar.Account)
                .WithMany(ua => ua.Roles)
                .HasForeignKey(uar => uar.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users_Accounts_Roles>()
                .HasOne(uar => uar.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(uar => uar.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Students>()
                .HasOne(s => s.Address)
                .WithOne()
                .HasForeignKey<Students>(s => s.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

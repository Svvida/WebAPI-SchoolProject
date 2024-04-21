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
                .HasKey(uar => new { uar.account_id, uar.role_id });

            modelBuilder.Entity<Users_Accounts_Roles>()
                .HasOne(uar => uar.account)
                .WithMany(ua => ua.roles)
                .HasForeignKey(uar => uar.account_id);

            modelBuilder.Entity<Users_Accounts_Roles>()
                .HasOne(uar => uar.role)
                .WithMany(r => r.accounts)
                .HasForeignKey(uar => uar.role_id);

            modelBuilder.Entity<Students>()
                .HasOne(s => s.address)
                .WithOne()
                .HasForeignKey<Students>(s => s.address_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

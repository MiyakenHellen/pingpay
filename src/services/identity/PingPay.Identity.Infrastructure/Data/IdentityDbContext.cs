using Microsoft.EntityFrameworkCore;
using PingPay.Identity.Domain.Entities;
using PingPay.Shared.Kernel.Abstractions;

namespace PingPay.Identity.Infrastructure.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Merchant> Merchants => Set<Merchant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Merchant>(e => 
            {
                e.ToTable("merchants");
                e.HasKey(m => m.Id);
                e.Property(m => m.Name).IsRequired().HasMaxLength(200);
                e.Property(m => m.Email).IsRequired().HasMaxLength(200);
                e.HasIndex(m => m.Email).IsUnique();
                e.Property(m => m.Cnpj).IsRequired().HasMaxLength(14);
                e.HasIndex(m => m.Cnpj).IsUnique();
                e.Property(m => m.ApiKey).IsRequired();
                e.HasIndex(m => m.ApiKey).IsUnique();
                e.Property(m => m.Status).HasConversion<string>();
                e.OwnsOne(m => m.SimulatorConfig, sc =>
                {
                    sc.Property(s => s.TransactionsPerMinute).HasColumnName("sim_tpm"); 
                    sc.Property(s => s.ErrorRate).HasColumnName("sim_error_rate");
                    sc.Property(s => s.NetworkSegment).HasColumnName("sim_network_segment");
                });
            });
        }
    }
}
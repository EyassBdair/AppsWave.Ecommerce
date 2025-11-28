using AppsWave.Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppsWave.Ecommerce.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                
                // FluentValidation-like rules using EF Core
                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(u => u.PasswordHash)
                    .IsRequired();
                
                // Convert Enum to string for database
                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });
            
            // Configure Product with validation
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.Property(p => p.Price).HasPrecision(18, 2);
                
                entity.Property(p => p.ArabicName)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(p => p.EnglishName)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(p => p.Price)
                    .IsRequired();
            });
            
            // Configure Invoice with validation
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(i => i.TotalAmount).HasPrecision(18, 2);
                
                entity.Property(i => i.UserId)
                    .IsRequired();
                
                entity.Property(i => i.Date)
                    .IsRequired();
                
                entity.Property(i => i.TotalAmount)
                    .IsRequired();
            });
            
            // Configure InvoiceDetail with validation
            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(d => d.Price).HasPrecision(18, 2);
                
                entity.Property(d => d.InvoiceId)
                    .IsRequired();
                
                entity.Property(d => d.ProductId)
                    .IsRequired();
                
                entity.Property(d => d.Quantity)
                    .IsRequired();
                
                entity.Property(d => d.Price)
                    .IsRequired();
            });
        }
    }
}


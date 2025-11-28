using AppsWave.Ecommerce.Domain.Entities;
using AppsWave.Ecommerce.Domain.Enums;
using AppsWave.Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace AppsWave.Ecommerce.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Apply pending migrations
            context.Database.Migrate();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User { FullName = "Admin User", Email = "admin@appswave.com", Username = "admin", PasswordHash = PasswordHasher.HashPassword("admin123"), Role = UserRole.Admin },
                new User { FullName = "Visitor User", Email = "visitor@appswave.com", Username = "visitor", PasswordHash = PasswordHasher.HashPassword("visitor123"), Role = UserRole.Visitor }
            };
            context.Users.AddRange(users);

            var products = new Product[]
            {
                new Product { ArabicName = "لابتوب", EnglishName = "Laptop", Price = 1500.00M },
                new Product { ArabicName = "ماوس", EnglishName = "Mouse", Price = 25.50M },
                new Product { ArabicName = "لوحة مفاتيح", EnglishName = "Keyboard", Price = 50.00M }
            };
            context.Products.AddRange(products);

            context.SaveChanges();
        }
    }
}


using AppsWave.Ecommerce.Application.Services;
using AppsWave.Ecommerce.Domain.Entities;
using AppsWave.Ecommerce.Domain.Enums;
using AppsWave.Ecommerce.Infrastructure.Data;
using AppsWave.Ecommerce.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppsWave.Ecommerce.Tests
{
    public class InvoiceServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateInvoice_ShouldReturnInvoiceDto_WhenValidData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);
            var userId = 1;

            // Add products
            var product1 = new Product
            {
                ArabicName = "منتج 1",
                EnglishName = "Product 1",
                Price = 100.00m
            };
            var product2 = new Product
            {
                ArabicName = "منتج 2",
                EnglishName = "Product 2",
                Price = 50.00m
            };
            context.Products.AddRange(product1, product2);
            await context.SaveChangesAsync();

            var createInvoiceDto = new CreateInvoiceDto
            {
                Details = new List<CreateInvoiceDetailDto>
                {
                    new CreateInvoiceDetailDto { ProductId = product1.Id, Quantity = 2 },
                    new CreateInvoiceDetailDto { ProductId = product2.Id, Quantity = 3 }
                }
            };

            // Act
            var result = await service.CreateInvoiceAsync(createInvoiceDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(2, result.Details.Count);
            Assert.Equal(350.00m, result.TotalAmount); // (100*2) + (50*3) = 200 + 150 = 350
        }

        [Fact]
        public async Task CreateInvoice_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);
            var userId = 1;

            var createInvoiceDto = new CreateInvoiceDto
            {
                Details = new List<CreateInvoiceDetailDto>
                {
                    new CreateInvoiceDetailDto { ProductId = 999, Quantity = 1 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateInvoiceAsync(createInvoiceDto, userId));
        }

        [Fact]
        public async Task GetInvoiceById_ShouldReturnInvoice_WhenUserOwnsInvoice()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);
            var userId = 1;

            var product = new Product
            {
                ArabicName = "منتج",
                EnglishName = "Product",
                Price = 100.00m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductId = product.Id,
                        Price = 100.00m,
                        Quantity = 1
                    }
                }
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetInvoiceByIdAsync(invoice.Id, userId, UserRole.Visitor.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice.Id, result.Id);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetInvoiceById_ShouldReturnInvoice_WhenUserIsAdmin()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);
            var userId = 1;
            var adminUserId = 999;

            var product = new Product
            {
                ArabicName = "منتج",
                EnglishName = "Product",
                Price = 100.00m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductId = product.Id,
                        Price = 100.00m,
                        Quantity = 1
                    }
                }
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetInvoiceByIdAsync(invoice.Id, adminUserId, UserRole.Admin.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice.Id, result.Id);
        }

        [Fact]
        public async Task GetInvoiceById_ShouldThrowException_WhenUserDoesNotOwnInvoice()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);
            var userId = 1;
            var otherUserId = 2;

            var product = new Product
            {
                ArabicName = "منتج",
                EnglishName = "Product",
                Price = 100.00m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductId = product.Id,
                        Price = 100.00m,
                        Quantity = 1
                    }
                }
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.GetInvoiceByIdAsync(invoice.Id, otherUserId, UserRole.Visitor.ToString()));
        }

        [Fact]
        public async Task GetAllInvoices_ShouldReturnAllInvoices()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);

            var invoice1 = new Invoice
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>()
            };
            var invoice2 = new Invoice
            {
                UserId = 2,
                Date = DateTime.UtcNow,
                TotalAmount = 200.00m,
                Details = new List<InvoiceDetail>()
            };
            context.Invoices.AddRange(invoice1, invoice2);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllInvoicesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateInvoice_ShouldUpdateInvoice_WhenValidData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);

            var product1 = new Product
            {
                ArabicName = "منتج 1",
                EnglishName = "Product 1",
                Price = 100.00m
            };
            var product2 = new Product
            {
                ArabicName = "منتج 2",
                EnglishName = "Product 2",
                Price = 50.00m
            };
            context.Products.AddRange(product1, product2);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductId = product1.Id,
                        Price = 100.00m,
                        Quantity = 1
                    }
                }
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var updateDto = new UpdateInvoiceDto
            {
                Id = invoice.Id,
                Details = new List<CreateInvoiceDetailDto>
                {
                    new CreateInvoiceDetailDto { ProductId = product1.Id, Quantity = 2 },
                    new CreateInvoiceDetailDto { ProductId = product2.Id, Quantity = 1 }
                }
            };

            // Act
            var result = await service.UpdateInvoiceAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice.Id, result.Id);
            Assert.Equal(2, result.Details.Count);
            Assert.Equal(250.00m, result.TotalAmount); // (100*2) + (50*1) = 200 + 50 = 250
        }

        [Fact]
        public async Task UpdateInvoice_ShouldReturnNull_WhenInvoiceNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);

            var updateDto = new UpdateInvoiceDto
            {
                Id = 999,
                Details = new List<CreateInvoiceDetailDto>()
            };

            // Act
            var result = await service.UpdateInvoiceAsync(updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateInvoice_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);

            var invoice = new Invoice
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                TotalAmount = 100.00m,
                Details = new List<InvoiceDetail>()
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var updateDto = new UpdateInvoiceDto
            {
                Id = invoice.Id,
                Details = new List<CreateInvoiceDetailDto>
                {
                    new CreateInvoiceDetailDto { ProductId = 999, Quantity = 1 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.UpdateInvoiceAsync(updateDto));
        }
    }
}


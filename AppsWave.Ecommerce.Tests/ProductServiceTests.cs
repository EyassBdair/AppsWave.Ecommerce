using AppsWave.Ecommerce.Application.Services;
using AppsWave.Ecommerce.Domain.Entities;
using AppsWave.Ecommerce.Infrastructure.Data;
using AppsWave.Ecommerce.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppsWave.Ecommerce.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnProductDto_WhenValidData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);
            var productDto = new ProductDto
            {
                ArabicName = "منتج تجريبي",
                EnglishName = "Test Product",
                Price = 100.50m
            };

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.ArabicName, result.ArabicName);
            Assert.Equal(productDto.EnglishName, result.EnglishName);
            Assert.Equal(productDto.Price, result.Price);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnPaginatedResults()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            // Add test products
            for (int i = 1; i <= 15; i++)
            {
                context.Products.Add(new Product
                {
                    ArabicName = $"منتج {i}",
                    EnglishName = $"Product {i}",
                    Price = 10.00m * i
                });
            }
            await context.SaveChangesAsync();

            // Act
            var (items, totalItems) = await service.GetProductsAsync(page: 1, pageSize: 10);

            // Assert
            Assert.Equal(10, items.Count);
            Assert.Equal(15, totalItems);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnUpdatedProduct_WhenProductExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var product = new Product
            {
                ArabicName = "منتج قديم",
                EnglishName = "Old Product",
                Price = 50.00m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var updateDto = new ProductDto
            {
                Id = product.Id,
                ArabicName = "منتج جديد",
                EnglishName = "New Product",
                Price = 75.00m
            };

            // Act
            var result = await service.UpdateProductAsync(product.Id, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.ArabicName, result.ArabicName);
            Assert.Equal(updateDto.EnglishName, result.EnglishName);
            Assert.Equal(updateDto.Price, result.Price);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var product = new Product
            {
                ArabicName = "منتج للحذف",
                EnglishName = "Product to Delete",
                Price = 30.00m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteProductAsync(product.Id);

            // Assert
            Assert.True(result);
            var deletedProduct = await context.Products.FindAsync(product.Id);
            Assert.True(deletedProduct!.IsDeleted);
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldCreateProduct_WhenIdIsZero()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);
            var productDto = new ProductDto
            {
                Id = 0,
                ArabicName = "منتج جديد",
                EnglishName = "New Product",
                Price = 100.00m
            };

            // Act
            var result = await service.CreateOrUpdateProductAsync(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(productDto.ArabicName, result.ArabicName);
            Assert.Equal(productDto.EnglishName, result.EnglishName);
            Assert.Equal(productDto.Price, result.Price);
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldUpdateProduct_WhenIdExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var existingProduct = new Product
            {
                ArabicName = "منتج قديم",
                EnglishName = "Old Product",
                Price = 50.00m
            };
            context.Products.Add(existingProduct);
            await context.SaveChangesAsync();

            var updateDto = new ProductDto
            {
                Id = existingProduct.Id,
                ArabicName = "منتج محدث",
                EnglishName = "Updated Product",
                Price = 75.00m
            };

            // Act
            var result = await service.CreateOrUpdateProductAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingProduct.Id, result.Id);
            Assert.Equal(updateDto.ArabicName, result.ArabicName);
            Assert.Equal(updateDto.EnglishName, result.EnglishName);
            Assert.Equal(updateDto.Price, result.Price);
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldCreateProduct_WhenIdDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var productDto = new ProductDto
            {
                Id = 999, // Non-existent ID
                ArabicName = "منتج جديد",
                EnglishName = "New Product",
                Price = 100.00m
            };

            // Act
            var result = await service.CreateOrUpdateProductAsync(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.NotEqual(999, result.Id); // Should create new product with new ID
            Assert.Equal(productDto.ArabicName, result.ArabicName);
            Assert.Equal(productDto.EnglishName, result.EnglishName);
            Assert.Equal(productDto.Price, result.Price);
        }
    }
}


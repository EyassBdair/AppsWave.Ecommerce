using AppsWave.Ecommerce.Application.Interfaces;
using AppsWave.Ecommerce.Domain.Entities;
using AppsWave.Ecommerce.Domain.Validators;
using AppsWave.Ecommerce.Infrastructure.Data;
using AppsWave.Ecommerce.Shared.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AppsWave.Ecommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<ProductDto> Items, int TotalItems)> GetProductsAsync(int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ArabicName = p.ArabicName,
                    EnglishName = p.EnglishName,
                    Price = p.Price
                })
                .ToListAsync();

            return (products, totalItems);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                Price = product.Price
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ArabicName = productDto.ArabicName,
                EnglishName = productDto.EnglishName,
                Price = productDto.Price
            };

            // Validate entity using FluentValidation
            var validator = new ProductValidator();
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                Price = product.Price
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            product.ArabicName = productDto.ArabicName;
            product.EnglishName = productDto.EnglishName;
            product.Price = productDto.Price;

            // Validate entity using FluentValidation
            var validator = new ProductValidator();
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                Price = product.Price
            };
        }

        public async Task<ProductDto> CreateOrUpdateProductAsync(ProductDto productDto)
        {
            Product product;

            // If ID is provided and > 0, try to update existing product
            if (productDto.Id > 0)
            {
                var existingProduct = await _context.Products.FindAsync(productDto.Id);
                if (existingProduct != null)
                {
                    // Update existing product
                    product = existingProduct;
                    product.ArabicName = productDto.ArabicName;
                    product.EnglishName = productDto.EnglishName;
                    product.Price = productDto.Price;
                }
                else
                {
                    // ID provided but product doesn't exist, create new
                    product = new Product
                    {
                        ArabicName = productDto.ArabicName,
                        EnglishName = productDto.EnglishName,
                        Price = productDto.Price
                    };
                    _context.Products.Add(product);
                }
            }
            else
            {
                // No ID or ID is 0, create new product
                product = new Product
                {
                    ArabicName = productDto.ArabicName,
                    EnglishName = productDto.EnglishName,
                    Price = productDto.Price
                };
                _context.Products.Add(product);
            }

            // Validate entity using FluentValidation
            var validator = new ProductValidator();
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                Price = product.Price
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            product.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();

            return true;
        }
    }
}


using AppsWave.Ecommerce.Application.Interfaces;
using AppsWave.Ecommerce.Domain.Entities;
using AppsWave.Ecommerce.Domain.Enums;
using AppsWave.Ecommerce.Domain.Validators;
using AppsWave.Ecommerce.Infrastructure.Data;
using AppsWave.Ecommerce.Shared.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AppsWave.Ecommerce.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto request, int userId)
        {
            var invoice = new Invoice
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Details = new List<InvoiceDetail>()
            };

            decimal totalAmount = 0;

            foreach (var item in request.Details)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
                }

                var detail = new InvoiceDetail
                {
                    ProductId = item.ProductId,
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                invoice.Details.Add(detail);
                totalAmount += product.Price * item.Quantity;
            }

            invoice.TotalAmount = totalAmount;

            // Validate entity using FluentValidation
            var validator = new InvoiceValidator();
            var validationResult = await validator.ValidateAsync(invoice);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return new InvoiceDto
            {
                Id = invoice.Id,
                Date = invoice.Date,
                UserId = invoice.UserId,
                TotalAmount = invoice.TotalAmount,
                Details = invoice.Details.Select(d => new InvoiceDetailDto
                {
                    ProductId = d.ProductId,
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            };
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id, int userId, string role)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Details)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return null;
            }

            // Users can only see their own invoices, unless Admin
            if (role != UserRole.Admin.ToString() && invoice.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this invoice.");
            }

            return new InvoiceDto
            {
                Id = invoice.Id,
                Date = invoice.Date,
                UserId = invoice.UserId,
                TotalAmount = invoice.TotalAmount,
                Details = invoice.Details.Select(d => new InvoiceDetailDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.EnglishName,
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            };
        }

        public async Task<List<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Details)
                .ToListAsync();

            return invoices.Select(invoice => new InvoiceDto
            {
                Id = invoice.Id,
                Date = invoice.Date,
                UserId = invoice.UserId,
                TotalAmount = invoice.TotalAmount,
                Details = invoice.Details.Select(d => new InvoiceDetailDto
                {
                    ProductId = d.ProductId,
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto request)
        {
                 var invoice = await _context.Invoices
                .Include(i => i.Details)
                .FirstOrDefaultAsync(i => i.Id == request.Id);

            if (invoice == null)
            {
                return null;
            }

            // Remove existing details
            _context.InvoiceDetails.RemoveRange(invoice.Details);
            invoice.Details.Clear();

            // Calculate new total amount and add new details
            decimal totalAmount = 0;

            foreach (var item in request.Details)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
                }

                var detail = new InvoiceDetail
                {
                    InvoiceId = invoice.Id,
                    ProductId = item.ProductId,
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                invoice.Details.Add(detail);
                totalAmount += product.Price * item.Quantity;
            }

            invoice.TotalAmount = totalAmount;

            // Validate entity using FluentValidation
            var validator = new InvoiceValidator();
            var validationResult = await validator.ValidateAsync(invoice);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _context.SaveChangesAsync();

            // Reload invoice with product details for response
            invoice = await _context.Invoices
                .Include(i => i.Details)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.Id == invoice.Id);

            return new InvoiceDto
            {
                Id = invoice!.Id,
                Date = invoice.Date,
                UserId = invoice.UserId,
                TotalAmount = invoice.TotalAmount,
                Details = invoice.Details.Select(d => new InvoiceDetailDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.EnglishName,
                    Price = d.Price,
                    Quantity = d.Quantity
                }).ToList()
            };
        }
    }
}


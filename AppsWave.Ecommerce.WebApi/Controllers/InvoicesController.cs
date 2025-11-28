using AppsWave.Ecommerce.Application.Interfaces;
using AppsWave.Ecommerce.Domain.Enums;
using AppsWave.Ecommerce.Shared.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppsWave.Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Visitor")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Visitor")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            int userId = int.Parse(userIdClaim.Value);

            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(request, userId);
                return Ok(new { Message = "Invoice created successfully.", Invoice = invoice });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getById")]
        [Authorize(Roles = "Admin,Visitor")]
        public async Task<IActionResult> GetInvoiceById([FromBody] GetInvoiceRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            string role = User.FindFirst(ClaimTypes.Role)?.Value ?? UserRole.Visitor.ToString();

            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(request.Id, userId, role);
                if (invoice == null)
                {
                    return NotFound();
                }
                return Ok(invoice);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("getAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceDto request)
        {
            try
            {
                var invoice = await _invoiceService.UpdateInvoiceAsync(request);
                if (invoice == null)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Invoice updated successfully.", Invoice = invoice });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}


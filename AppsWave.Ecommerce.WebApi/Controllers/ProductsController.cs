using AppsWave.Ecommerce.Application.Interfaces;
using AppsWave.Ecommerce.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppsWave.Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("getAll")]
        [Authorize(Roles = "Admin,Visitor")]
        public async Task<IActionResult> GetProducts([FromBody] GetProductsRequest request)
        {
            var (items, totalItems) = await _productService.GetProductsAsync(request.Page, request.PageSize);
            return Ok(new { TotalItems = totalItems, Page = request.Page, PageSize = request.PageSize, Items = items });
        }

        [HttpPost("getById")]
        [Authorize(Roles = "Admin,Visitor")]
        public async Task<IActionResult> GetProductById([FromBody] GetProductRequest request)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost("createOrUpdate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrUpdateProduct([FromBody] ProductDto request)
        {
            try
            {
                var product = await _productService.CreateOrUpdateProductAsync(request);
                
                // If ID was 0 or not provided, it's a new product
                if (request.Id == 0)
                {
                    return Ok(new { Message = "Product created successfully.", Product = product });
                }
                
                // Otherwise it's an update
                return Ok(new { Message = "Product updated successfully.", Product = product });
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductRequest request)
        {
            var result = await _productService.DeleteProductAsync(request.Id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}


using AppsWave.Ecommerce.Shared.DTOs;

namespace AppsWave.Ecommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<(List<ProductDto> Items, int TotalItems)> GetProductsAsync(int page, int pageSize);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto);
        Task<ProductDto> CreateOrUpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}


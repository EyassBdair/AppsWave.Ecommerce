using System.ComponentModel.DataAnnotations;

namespace AppsWave.Ecommerce.Shared.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string ArabicName { get; set; } = string.Empty;

        [Required]
        public string EnglishName { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }

    public class GetProductRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class GetProductsRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class DeleteProductRequest
    {
        [Required]
        public int Id { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace AppsWave.Ecommerce.Shared.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceDetailDto> Details { get; set; } = new();
    }

    public class InvoiceDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateInvoiceDto
    {
        [Required]
        public List<CreateInvoiceDetailDto> Details { get; set; } = new();
    }

    public class CreateInvoiceDetailDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class GetInvoiceRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class UpdateInvoiceDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public List<CreateInvoiceDetailDto> Details { get; set; } = new();
    }
}


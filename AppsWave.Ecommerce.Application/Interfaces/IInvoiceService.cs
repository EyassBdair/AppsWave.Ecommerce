using AppsWave.Ecommerce.Shared.DTOs;

namespace AppsWave.Ecommerce.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto request, int userId);
        Task<InvoiceDto?> GetInvoiceByIdAsync(int id, int userId, string role);
        Task<List<InvoiceDto>> GetAllInvoicesAsync();
        Task<InvoiceDto?> UpdateInvoiceAsync(UpdateInvoiceDto request);
    }
}


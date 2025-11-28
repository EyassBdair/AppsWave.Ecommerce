using AppsWave.Ecommerce.Shared.DTOs;

namespace AppsWave.Ecommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto request);
        Task<string> LoginAsync(LoginDto request);
    }
}


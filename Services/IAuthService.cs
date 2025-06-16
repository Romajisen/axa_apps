// Services/IAuthService.cs
using ProductApi.DTOs;

namespace ProductApi.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto request);
        Task<string> LoginAsync(LoginDto request);
    }
}

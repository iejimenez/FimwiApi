using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(User user, string token)> LoginAsync(LoginDto loginDto);
        Task LogoutAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
        Task<(User user, string token)> RegisterAsync(RegisterDto registerDto);
        Task<string> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task RevokeTokenAsync(RevokeTokenDto revokeTokenDto);
    }
} 
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using System.Collections.Generic;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, IEnumerable<string> roles);
        Task<bool> ValidateTokenAsync(string token);
    }
} 
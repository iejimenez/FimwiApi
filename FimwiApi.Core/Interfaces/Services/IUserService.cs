using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<PagedResponse<UserDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(UserDto userDto);
        Task DeleteAsync(Guid id);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ConfirmEmailAsync(string email, string token);
    }
} 
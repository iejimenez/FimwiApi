using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> GetByIdAsync(Guid id);
        Task<PermissionDto> CreateAsync(PermissionDto permissionDto);
        Task<PermissionDto> UpdateAsync(Guid id, PermissionDto permissionDto);
        Task DeleteAsync(Guid id);
    }
} 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermissionDto>> GetByRoleIdAsync(Guid roleId);
        Task<RolePermissionDto> CreateAsync(RolePermissionDto rolePermissionDto);
        Task DeleteAsync(Guid roleId, Guid permissionId);
    }
} 
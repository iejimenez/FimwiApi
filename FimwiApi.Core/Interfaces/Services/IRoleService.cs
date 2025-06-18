using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<RoleDto> GetByIdAsync(Guid id);
        Task<PagedResponse<RoleDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<RoleDto> CreateAsync(RoleDto roleDto);
        Task<RoleDto> UpdateAsync(RoleDto roleDto);
        Task DeleteAsync(Guid id);
    }
} 
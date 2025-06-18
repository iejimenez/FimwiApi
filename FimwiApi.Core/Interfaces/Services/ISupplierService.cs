using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetByIdAsync(Guid id);
        Task<PagedResponse<SupplierDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<SupplierDto>> SearchAsync(string searchTerm, int page, int pageSize);
        Task<SupplierDto> CreateAsync(SupplierDto supplier);
        Task<SupplierDto> UpdateAsync(SupplierDto supplier);
        Task DeleteAsync(Guid id);
    }
} 
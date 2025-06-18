using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetByIdAsync(Guid id);
        Task<PagedResponse<CustomerDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm, int page, int pageSize);
        Task<CustomerDto> CreateAsync(CustomerDto customerDto);
        Task<CustomerDto> UpdateAsync(CustomerDto customerDto);
        Task DeleteAsync(Guid id);
    }
} 
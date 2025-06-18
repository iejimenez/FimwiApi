using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<PagedResponse<ProductDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm, string category, int page, int pageSize);
        Task<ProductDto> CreateAsync(ProductDto productDto);
        Task<ProductDto> UpdateAsync(ProductDto productDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync();
    }
} 
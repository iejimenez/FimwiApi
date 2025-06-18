using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Models.DTOs;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IBatchService
    {
        Task<IEnumerable<BatchDto>> GetByProductIdAsync(Guid productId);
        Task<BatchDto> GetByIdAsync(Guid id);
        Task<BatchDto> CreateAsync(BatchDto batchDto);
        Task<BatchDto> UpdateAsync(BatchDto batchDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<BatchDto>> GetExpiredBatchesAsync();
        Task<IEnumerable<BatchDto>> GetActiveBatchesAsync();
    }
} 
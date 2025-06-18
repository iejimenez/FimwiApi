using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories
{
    public interface IBatchRepository : IRepository<Batch>
    {
        Task<IEnumerable<Batch>> GetByProductIdAsync(Guid productId);
        Task<Batch> GetByBatchNumberAsync(string batchNumber);
        Task<IEnumerable<Batch>> GetExpiredBatchesAsync();
        Task<IEnumerable<Batch>> GetActiveBatchesAsync();
    }
}
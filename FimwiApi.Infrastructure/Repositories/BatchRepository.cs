using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FimwiApi.Infrastructure.Repositories
{
    public class BatchRepository : GenericRepository<Batch>, IBatchRepository
    {
        public BatchRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Batch>> GetByProductIdAsync(Guid productId)
        {
            return await _dbSet
                .Where(b => b.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Batch> GetByBatchNumberAsync(string batchNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.BatchNumber == batchNumber);
        }

        public async Task<IEnumerable<Batch>> GetExpiredBatchesAsync()
        {
            return await _dbSet
                .Where(b => b.ExpirationDate < DateTime.UtcNow && b.Status == "active")
                .ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetActiveBatchesAsync()
        {
            return await _dbSet
                .Where(b => b.Status == "active")
                .ToListAsync();
        }
    }
} 
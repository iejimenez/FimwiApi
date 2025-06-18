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
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId)
        {
            return await _dbSet
                .Include(po => po.Items)
                .ThenInclude(poi => poi.Product)
                .Where(po => po.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> GetByOrderNumberAsync(string orderNumber)
        {
            return await _dbSet
                .Include(po => po.Items)
                .ThenInclude(poi => poi.Product)
                .FirstOrDefaultAsync(po => po.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(po => po.Items)
                .ThenInclude(poi => poi.Product)
                .Where(po => po.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(po => po.Items)
                .ThenInclude(poi => poi.Product)
                .Where(po => po.Date >= startDate && po.Date <= endDate)
                .ToListAsync();
        }
    }
} 
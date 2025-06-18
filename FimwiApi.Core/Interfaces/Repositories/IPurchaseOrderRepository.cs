using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId);
        Task<PurchaseOrder> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status);
        Task<IEnumerable<PurchaseOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
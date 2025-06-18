using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderDto> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseOrderDto>> GetBySupplierIdAsync(Guid supplierId);
        Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto orderDto);
        Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto orderDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<PurchaseOrderDto>> GetByStatusAsync(string status);
        Task<IEnumerable<PurchaseOrderDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<PagedResponse<PurchaseOrderDto>> GetAllAsync(int pageNumber, int pageSize);
    }
} 
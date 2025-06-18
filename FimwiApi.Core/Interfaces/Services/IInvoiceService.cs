using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Models.DTOs;
using FimwiApi.Core.Models.Responses;

namespace FimwiApi.Core.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GetByIdAsync(Guid id);
        Task<IEnumerable<InvoiceDto>> GetByCustomerIdAsync(Guid customerId);
        Task<InvoiceDto> CreateAsync(InvoiceDto invoiceDto);
        Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<InvoiceDto>> GetByStatusAsync(string status);
        Task<IEnumerable<InvoiceDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<PagedResponse<InvoiceDto>> GetAllAsync(int pageNumber, int pageSize);
    }
} 
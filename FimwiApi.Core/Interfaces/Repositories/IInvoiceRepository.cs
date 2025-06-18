using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetByIdAsync(Guid id);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId);
        Task<Invoice> CreateAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task DeleteAsync(Guid id);
        Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<IEnumerable<Invoice>> GetByStatusAsync(string status);
        Task<IEnumerable<Invoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 
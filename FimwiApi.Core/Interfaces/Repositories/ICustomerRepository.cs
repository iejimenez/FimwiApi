using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByDocumentTypeAndDocumentNumberAsync(string documentTye, string documentNumber);
        Task<IEnumerable<Customer>> SearchAsync(string searchTerm, int page, int pageSize);
    }
}
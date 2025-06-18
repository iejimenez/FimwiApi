using FimwiApi.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FimwiApi.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IBatchRepository Batches { get; }
        ISupplierRepository Suppliers { get; }
        IPurchaseOrderRepository PurchaseOrders { get; }
        ICustomerRepository Customers { get; }
        IInvoiceRepository Invoices { get; }

        Task<int> SaveChangesAsync();
    }
} 
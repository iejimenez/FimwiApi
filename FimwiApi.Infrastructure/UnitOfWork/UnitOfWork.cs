using FimwiApi.Core.Interfaces;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using FimwiApi.Infrastructure.Repositories;

namespace FimwiApi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _userRepository;
    private IProductRepository? _productRepository;
    private IBatchRepository? _batchRepository;
    private ISupplierRepository? _supplierRepository;
    private IPurchaseOrderRepository? _purchaseOrderRepository;
    private ICustomerRepository? _customerRepository;
    private IInvoiceRepository? _invoiceRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
    public IBatchRepository Batches => _batchRepository ??= new BatchRepository(_context);
    public ISupplierRepository Suppliers => _supplierRepository ??= new SupplierRepository(_context);
    public IPurchaseOrderRepository PurchaseOrders => _purchaseOrderRepository ??= new PurchaseOrderRepository(_context);
    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
    public IInvoiceRepository Invoices => _invoiceRepository ??= new InvoiceRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
} 
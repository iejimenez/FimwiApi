using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> SearchAsync(string searchTerm, string category, int pageNumber, int pageSize);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<Product?> GetBySkuAsync(string sku);
} 
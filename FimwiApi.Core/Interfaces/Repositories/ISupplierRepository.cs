using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<IEnumerable<Supplier>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
    Task<Supplier> CreateAsync(Supplier supplier);
    Task<Supplier> UpdateAsync(Supplier supplier);
    Task<bool> DeleteAsync(Guid id);
}
 
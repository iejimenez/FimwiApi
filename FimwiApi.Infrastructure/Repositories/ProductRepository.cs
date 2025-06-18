using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FimwiApi.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Sku == sku);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _dbSet.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, string category, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => 
                    p.Name.Contains(searchTerm) || 
                    p.Description.Contains(searchTerm) || 
                    p.Sku.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _dbSet
                .Where(p => p.CurrentStock <= p.MinStock)
                .ToListAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbSet.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _dbSet.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _dbSet.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _dbSet.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
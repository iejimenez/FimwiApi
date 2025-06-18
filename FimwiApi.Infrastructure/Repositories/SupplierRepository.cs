using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FimwiApi.Infrastructure.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Supplier?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<Supplier?> GetByTaxIdAsync(string taxId)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.TaxId == taxId);
        }

        public async Task<IEnumerable<Supplier>> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => 
                    s.Name.Contains(searchTerm) || 
                    s.Code.Contains(searchTerm) || 
                    s.TaxId.Contains(searchTerm));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            await _dbSet.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier> UpdateAsync(Supplier supplier)
        {
            _dbSet.Update(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var supplier = await _dbSet.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }

            _dbSet.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
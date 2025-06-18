using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FimwiApi.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<Role>> SearchAsync(string searchTerm, int page, int pageSize)
        {
            return await _dbSet
                .Where(r => r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
} 
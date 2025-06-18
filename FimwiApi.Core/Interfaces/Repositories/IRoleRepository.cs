using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> SearchAsync(string searchTerm, int page, int pageSize);
    }
} 
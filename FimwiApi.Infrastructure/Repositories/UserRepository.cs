using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FimwiApi.Core.Entities;
using FimwiApi.Core.Interfaces.Repositories;
using FimwiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FimwiApi.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _dbSet.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _dbSet.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
} 
using LogixSoftwareTask.DataAccessLayer.Data;
using LogixSoftwareTask.DataAccessLayer.Interfaces;
using LogixSoftwareTask.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogixSoftwareTask.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LogixDbContext _context;

        public UserRepository(LogixDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}


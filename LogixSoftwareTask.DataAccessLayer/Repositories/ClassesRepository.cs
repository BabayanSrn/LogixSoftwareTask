using LogixSoftwareTask.DataAccessLayer.Data;
using LogixSoftwareTask.DataAccessLayer.Interfaces;
using LogixSoftwareTask.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogixSoftwareTask.DataAccessLayer.Repositories
{
    public class ClassesRepository : IClassesRepository
    {
        private readonly LogixDbContext _dbContext;

        public ClassesRepository(LogixDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Classes>> GetAllClassesAsync()
        {
            return await _dbContext.Classes.ToListAsync();
        }
        public async Task<List<Classes>> GetClassesByIdsAsync(List<int> classIds)
        {
            return await _dbContext.Classes
                .Where(c => classIds.Contains(c.Id))
                .ToListAsync();
        }
    }
}

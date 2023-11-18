using LogixSoftwareTask.Storage.Entities;

namespace LogixSoftwareTask.DataAccessLayer.Interfaces
{
    public interface IClassesRepository
    {
        Task<IEnumerable<Classes>> GetAllClassesAsync();
        Task<List<Classes>> GetClassesByIdsAsync(List<int> classIds);
    }
}

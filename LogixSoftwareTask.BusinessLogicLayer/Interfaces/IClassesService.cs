using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;

namespace LogixSoftwareTask.BusinessLogicLayer.Interfaces
{
    public interface IClassesService
    {
        Task<IEnumerable<ClassModel>> GetAllClassesAsync(); 
        Task<List<ClassModel>> GetClassesByIdsAsync(List<int> classIds); 
    }
}

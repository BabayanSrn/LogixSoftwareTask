using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.DataAccessLayer.Interfaces;
using LogixSoftwareTask.Storage.Models;

namespace LogixSoftwareTask.BusinessLogicLayer.Services
{
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepository _classesRepository;

        public ClassesService(IClassesRepository classesRepository)
        {
            _classesRepository = classesRepository;
        }

        public async Task<IEnumerable<ClassModel>> GetAllClassesAsync()
        {
            var classes = await _classesRepository.GetAllClassesAsync();
            return classes.Select(entity => new ClassModel
            {
                ClassName = entity.ClassName,
            });
        }

        public async Task<List<ClassModel>> GetClassesByIdsAsync(List<int> classIds)
        {
            var classes = await _classesRepository.GetClassesByIdsAsync(classIds);

            var classModels = classes.Select(entity => new ClassModel
            {
                ClassName = entity.ClassName,
            }).ToList();

            return classModels;
        }
    }
}

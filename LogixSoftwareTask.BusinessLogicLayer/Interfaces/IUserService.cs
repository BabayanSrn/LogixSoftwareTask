using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;

namespace LogixSoftwareTask.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegistrationModel model);
        Task<string> LoginAsync(LoginModel model);
        Task<UserModel> GetUserByIdAsync(int userId);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int userId, UserModel updatedUser);
        Task<bool> DeleteUserAsync(int userId);
        string TransformAddress(string userAddress);
        Task EditClassAsync(List<ClassModel> updatedClasses, string userId);

    }
}

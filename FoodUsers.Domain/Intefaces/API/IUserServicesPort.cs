using FoodUsers.Domain.Models;

namespace FoodUsers.Domain.Intefaces.API
{
    public interface IUserServicesPort
    {
        Task<UserModel> CreateUser(UserModel user, string identityRoleId);
        Task<UserModel> GetUser(int id);
        Task<UserModel?> GetUser(string email, string password);
    }
}

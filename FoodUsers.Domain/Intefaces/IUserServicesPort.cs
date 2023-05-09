using FoodUsers.Domain.Models;

namespace FoodUsers.Domain.Intefaces
{
    public interface IUserServicesPort
    {
        Task CreateUser(User user);
        Task<User> GetUser(int id);
        Task<User> GetUser(string email, string password);
    }
}

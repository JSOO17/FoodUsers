using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;

namespace FoodUsers.Application.Services.Interfaces
{
    public interface IUserServices
    {
        Task CreateUser(UserRequestDTO user);
        Task<UserResponseDTO> GetUser(int id);
        Task<UserResponseDTO> GetUser(string email, string password);
    }
}

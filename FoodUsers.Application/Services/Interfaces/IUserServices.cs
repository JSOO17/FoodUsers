using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Domain.Models;

namespace FoodUsers.Application.Services.Interfaces
{
    public interface IUserServices
    {
        Task<UserResponseDTO> CreateUser(UserRequestDTO userRequest, string identityRoleId);
        Task<UserResponseDTO> GetUser(int id);
        Task<UserResponseDTO?> GetUser(string email, string password);
    }
}

using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Application.Mappers;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Intefaces.API;

namespace FoodUsers.Application.Services
{
    public class UserServices : IUserServices
    {
        private IUserServicesPort userServicesPort;
        public UserServices(IUserServicesPort userServicesPort) 
        {
            this.userServicesPort = userServicesPort;
        }

        public async Task<UserResponseDTO> CreateUser(UserRequestDTO userRequest, string identityRoleId)
        {
            var user = UserMapper.ToUser(userRequest);

            var userModel = await userServicesPort.CreateUser(user, identityRoleId);

            return UserMapper.ToUserResponseDTO(userModel);
        }

        public async Task<UserResponseDTO> GetUser(int id)
        {
            var user = await userServicesPort.GetUser(id);

            return UserMapper.ToUserResponseDTO(user);
        }

        public async Task<UserResponseDTO?> GetUser(string email, string password)
        {
            var user = await userServicesPort.GetUser(email, password);

            return user == null ? null : UserMapper.ToUserResponseDTO(user);
        }
    }
}

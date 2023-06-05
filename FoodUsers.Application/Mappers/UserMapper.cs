using AutoMapper;
using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Domain.Models;

namespace FoodUsers.Application.Mappers
{
    public class UserMapper
    {
        private readonly IMapper mapper;

        public static UserResponseDTO ToUserResponseDTO(UserModel user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserResponseDTO>());

            var mapper = new Mapper(config);

            return mapper.Map<UserModel, UserResponseDTO>(user);
        }

        public static UserModel ToUser(UserRequestDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestDTO, UserModel>());

            var mapper = new Mapper(config);

            return mapper.Map<UserRequestDTO, UserModel>(user);
        }
    }
}

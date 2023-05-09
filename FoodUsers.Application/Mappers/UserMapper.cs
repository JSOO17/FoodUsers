using AutoMapper;
using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Domain.Models;

namespace FoodUsers.Application.Mappers
{
    public class UserMapper
    {
        private readonly IMapper mapper;

        public static UserResponseDTO ToUserResponseDTO(User user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserResponseDTO>());

            var mapper = new Mapper(config);

            return mapper.Map<User, UserResponseDTO>(user);
        }

        public static User ToUser(UserRequestDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRequestDTO, User>());

            var mapper = new Mapper(config);

            return mapper.Map<UserRequestDTO, User>(user);
        }
    }
}

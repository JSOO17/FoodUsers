using AutoMapper;
using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.Data.Models;

namespace FoodUsers.Infrastructure.Data.Mappers
{
    public class UserMapper
    {
        public static User ToUser(UserModel user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, User>());

            var mapper = new Mapper(config);

            return mapper.Map<UserModel, User>(user);
        }

        public static UserModel ToUserModel(User user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());

            var mapper = new Mapper(config);

            return mapper.Map<User, UserModel>(user);
        }
    }
}

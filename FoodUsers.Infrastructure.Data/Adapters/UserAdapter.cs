using FoodUsers.Domain.Intefaces.SPI;
using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.Data.Mappers;
using FoodUsers.Infrastructure.Data.Models;

namespace FoodUsers.Infrastructure.Data.Adapters
{
    public class UserAdapter : IUserPersistencePort
    {
        private readonly foodusersContext _foodusersContext;
        public UserAdapter(foodusersContext foodusersContext) 
        {
            _foodusersContext = foodusersContext;
        }

        public async Task<UserModel> CreateUser(UserModel userModel)
        {
            var user = UserMapper.ToUser(userModel);

            await _foodusersContext.AddAsync(user);

            await _foodusersContext.SaveChangesAsync();

            userModel.Id = user.Id;

            return userModel;
        }

        public Task<UserModel> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel?> GetUser(string email)
        {
            var user = _foodusersContext.Users
                            .Where(user => user.Email == email)
                            .FirstOrDefault();

            return Task.FromResult(user == null ? null : UserMapper.ToUserModel(user));
        }
    }
}

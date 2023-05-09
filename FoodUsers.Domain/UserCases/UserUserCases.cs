using FoodUsers.Domain.Intefaces;
using FoodUsers.Domain.Models;

namespace FoodUsers.Domain.UserCases
{
    public class UserUserCases : IUserServicesPort
    {
        public Task CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(string email, string password)
        {
            return Task.FromResult(new User
            {
                Email = email,
                Name = "Yunenfis",
                Password = password,
                RolId = 1
            });
        }
    }
}

using FoodUsers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodUsers.Domain.Intefaces.SPI
{
    public interface IUserPersistencePort
    {
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> GetUser(int id);
        Task<UserModel?> GetUser(string email);
    }
}

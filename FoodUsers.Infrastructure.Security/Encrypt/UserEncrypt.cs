using FoodUsers.Domain.Intefaces.SPI;

namespace FoodUsers.Infrastructure.Security.Encrypt
{
    public class UserEncrypt : IUserEncryptPort
    {
        public string Encode(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}

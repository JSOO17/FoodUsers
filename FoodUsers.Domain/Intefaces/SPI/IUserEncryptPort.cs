namespace FoodUsers.Domain.Intefaces.SPI
{
    public interface IUserEncryptPort
    {
        string Encode(string password);

        bool Verify(string password, string hashPassword);
    }
}

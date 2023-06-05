namespace FoodUsers.Domain.Exceptions
{
    public class InvalidLoginCredentialsException : Exception
    {
        public InvalidLoginCredentialsException(string message) : base(message)
        {
        }
    }
}

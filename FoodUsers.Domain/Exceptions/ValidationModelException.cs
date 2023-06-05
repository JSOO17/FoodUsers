using FluentValidation;

namespace FoodUsers.Domain.Exceptions
{
    public class ValidationModelException : ValidationException
    {
        public ValidationModelException(string message) : base(message)
        {
        }
    }
}

using FluentValidation;
using FoodUsers.Domain.Models;
using System.Text.RegularExpressions;

namespace FoodUsers.Domain.Utils
{
    public class ValidatorUserModel : AbstractValidator<UserModel>
    {
        private const string PatternCellphone = @"^[0-9+]+$";

        public ValidatorUserModel() 
        {
            RuleFor(user => user.DNI)
                .NotEmpty()
                .WithMessage("The DNI can not be empty.");
            RuleFor(user => user.DNI)
                .NotNull()
                .WithMessage("The DNI can not be null.");

            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("The name can not be empty.");
            RuleFor(user => user.Name)
                .NotNull()
                .WithMessage("The name can not be null.");

            RuleFor(user => user.Lastname)
                .NotEmpty()
                .WithMessage("The lastname can not be empty.");
            RuleFor(user => user.Lastname)
                .NotNull()
                .WithMessage("The lastname can not be null.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("The email can not be empty.");
            RuleFor(user => user.Email)
                .NotNull()
                .WithMessage("The email can not be null.");

            RuleFor(user => user.Cellphone)
                .NotEmpty()
                .WithMessage("The email can not be empty.");
            RuleFor(user => user.Cellphone)
                .NotNull()
                .WithMessage("The email can not be null.");
            RuleFor(user => user.Cellphone)
                .MaximumLength(13)
                .WithMessage("The cellphone can not have more than 13 characters");
            RuleFor(user => user.Cellphone)
                .Must(cellphone => Regex.IsMatch(cellphone, PatternCellphone))
                .WithMessage("The cellphone only numbers and +");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("The password can not be empty.");
            RuleFor(user => user.Password)
                .NotNull()
                .WithMessage("The password can not be null.");
        }
    }
}


using FluentValidation;
using FoodUsers.Domain.Exceptions;
using FoodUsers.Domain.Intefaces.API;
using FoodUsers.Domain.Intefaces.SPI;
using FoodUsers.Domain.Models;
using FoodUsers.Domain.Utils;

namespace FoodUsers.Domain.UserCases
{
    public class UserUserCases : IUserServicesPort
    {
        private readonly IUserPersistencePort _userPersistencePort;
        private readonly IUserEncryptPort _userEncrypt;

        public UserUserCases(IUserPersistencePort userPersistencePort, IUserEncryptPort userEncrypt)
        {
            _userPersistencePort = userPersistencePort;
            _userEncrypt = userEncrypt;
        }

        public async Task<UserModel> CreateUser(UserModel user, string identityRoleId)
        {
            await ValidateUser(user, identityRoleId);

            if (user.RoleId != Roles.Client)
            {
                ValidateRoles(user.RoleId, int.Parse(identityRoleId));
            }

            var passwordHash = _userEncrypt.Encode(user.Password);

            user.Password = passwordHash;

            return await _userPersistencePort.CreateUser(user); 
        }

        public async Task<UserModel?> GetUser(string email, string password)
        {
            var userModel = await _userPersistencePort.GetUser(email) ?? throw new InvalidLoginCredentialsException("Email is incorrect");
            var isPasswordValid = _userEncrypt.Verify(password, userModel.Password);

            if (!isPasswordValid)
            {
                throw new InvalidLoginCredentialsException("password is incorrect");
            }

            return userModel;
        }

        public Task<UserModel> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        private async Task ValidateUser(UserModel user, string identityRoleId)
        {
            var validator = new ValidatorUserModel();

            await validator.ValidateAndThrowAsync(user);
        }

        private void ValidateRoles(int roleId, int identityRoleId)
        {
            var rolesValidation = new Dictionary<int, List<int>>
            {
                { Roles.Owner, new List<int>{ Roles.Admin } },
                { Roles.Employee, new List<int>{ Roles.Owner } }
            };

            var rolesAllowed = rolesValidation[roleId];

            if (!rolesAllowed.Contains(identityRoleId))
            {
                throw new RoleHasNotPermissionException("You don´t have permission");
            }
        }
    }
}

using FluentValidation;
using FoodUsers.Domain.Exceptions;
using FoodUsers.Domain.Intefaces.SPI;
using FoodUsers.Domain.Models;
using FoodUsers.Domain.UserCases;
using Moq;

namespace FoodUsers.Donmain.Tests.UseCases
{
    [TestClass]
    public class UserUsercasesTests
    {
        [TestMethod]
        public async Task CreateUserSuccessfull()
        {
            var mockUserPersistence = new Mock<IUserPersistencePort>();
            var mockUserEncrypt = new Mock<IUserEncryptPort>();

            var userUsecases = new UserUserCases(mockUserPersistence.Object, mockUserEncrypt.Object);

            mockUserEncrypt
                .Setup(p => p.Encode(It.IsAny<string>()))
                .Returns("ff");

            mockUserPersistence
                .Setup(p => p.CreateUser(It.IsAny<UserModel>()))
                .Returns(Task.FromResult(new UserModel()
                {
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "ff",
                    RoleId = 2
                }));

            var userModel = new UserModel()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var user = await userUsecases.CreateUser(userModel, "1");

            Assert.IsNotNull(user);
            Assert.AreEqual("Pepito", user.Name);
            Assert.AreEqual("Perezz", user.Lastname);
            Assert.AreEqual("user@gmail.com", user.Email);
            Assert.AreEqual("ff", user.Password);
            Assert.AreEqual("+341341", user.Cellphone);
            Assert.AreEqual(3314, user.DNI);
            Assert.AreEqual(2, user.RoleId);
        }

        [TestMethod]
        public async Task CreateUserInvalid()
        {
            var mockUserPersistence = new Mock<IUserPersistencePort>();
            var mockUserEncrypt = new Mock<IUserEncryptPort>();

            var userUsecases = new UserUserCases(mockUserPersistence.Object, mockUserEncrypt.Object);

            mockUserEncrypt
                .Setup(p => p.Encode(It.IsAny<string>()))
                .Returns("ff");

            mockUserPersistence
                .Setup(p => p.CreateUser(It.IsAny<UserModel>()))
                .Returns(Task.FromResult(new UserModel()
                {
                    Name = "Pepito",
                    DNI = 3314,
                    Lastname = "Perezz",
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "ff",
                    RoleId = 2
                }));

            var userModel = new UserModel()
            {
                Name = "Pepito",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await userUsecases.CreateUser(userModel, "1");
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Validation failed: \r\n -- Lastname: The lastname can not be empty. Severity: Error\r\n -- Lastname: The lastname can not be null. Severity: Error", exception.Message);
        }

        [TestMethod]
        public async Task GetUserSuccessfull()
        {
            var mockUserPersistence = new Mock<IUserPersistencePort>();
            var mockUserEncrypt = new Mock<IUserEncryptPort>();

            var userUsecases = new UserUserCases(mockUserPersistence.Object, mockUserEncrypt.Object);

            mockUserEncrypt
                .Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            mockUserPersistence
                .Setup(p => p.GetUser(It.IsAny<string>()))
                .Returns(Task.FromResult<UserModel?>(new UserModel()
                {
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                }));

            var userModel = new UserModel()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var user = await userUsecases.GetUser("user@gmail.com", "papa");

            Assert.IsNotNull(user);
            Assert.AreEqual("Pepito", user.Name);
            Assert.AreEqual("Perezz", user.Lastname);
            Assert.AreEqual("user@gmail.com", user.Email);
            Assert.AreEqual("fajgrnvia", user.Password);
            Assert.AreEqual("+341341", user.Cellphone);
            Assert.AreEqual(3314, user.DNI);
            Assert.AreEqual(2, user.RoleId);
        }

        [TestMethod]
        public async Task GetUserInvalidEmail()
        {
            var mockUserPersistence = new Mock<IUserPersistencePort>();
            var mockUserEncrypt = new Mock<IUserEncryptPort>();

            var userUsecases = new UserUserCases(mockUserPersistence.Object, mockUserEncrypt.Object);

            mockUserEncrypt
                .Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            mockUserPersistence
                .Setup(p => p.GetUser(It.IsAny<string>()))
                .Returns(Task.FromResult<UserModel?>(null));

            var userModel = new UserModel()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var exception = await Assert.ThrowsExceptionAsync<InvalidLoginCredentialsException>(async () =>
            {
                await userUsecases.GetUser("user@gmail.com", "papa");
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Email is incorrect", exception.Message);
        }

        [TestMethod]
        public async Task GetUserInvalidPassword()
        {
            var mockUserPersistence = new Mock<IUserPersistencePort>();
            var mockUserEncrypt = new Mock<IUserEncryptPort>();

            var userUsecases = new UserUserCases(mockUserPersistence.Object, mockUserEncrypt.Object);

            mockUserEncrypt
                 .Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(false);

            mockUserPersistence
                .Setup(p => p.GetUser(It.IsAny<string>()))
                .Returns(Task.FromResult<UserModel?>(new UserModel()
                {
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                }));

            var userModel = new UserModel()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var exception = await Assert.ThrowsExceptionAsync<InvalidLoginCredentialsException>(async () =>
            {
                await userUsecases.GetUser("user@gmail.com", "papa");
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("password is incorrect", exception.Message);
        }
    }
}

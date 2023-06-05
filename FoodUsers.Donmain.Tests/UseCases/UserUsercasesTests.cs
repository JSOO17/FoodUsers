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
    }
}

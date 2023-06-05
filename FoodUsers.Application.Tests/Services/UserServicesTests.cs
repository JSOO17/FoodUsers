using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.Services;
using FoodUsers.Domain.Intefaces.API;
using FoodUsers.Domain.Models;
using Moq;

namespace FoodUsers.Application.Tests.Services
{
    [TestClass]
    public class UserServicesTests
    {
        [TestMethod]
        public async Task CreateUser()
        {
            var mockUserServices = new Mock<IUserServicesPort>();

            var userServices = new UserServices(mockUserServices.Object);

            mockUserServices
                .Setup(p => p.CreateUser(It.IsAny<UserModel>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserModel()
                {
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                }));

            var userRequest = new UserRequestDTO()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("fajgrnvia"),
                RoleId = 2
            };

            var userReponse = await userServices.CreateUser(userRequest, "1");

            Assert.IsNotNull(userReponse);
            Assert.AreEqual("Pepito", userReponse.Name);
            Assert.AreEqual("Perezz", userReponse.Lastname);
            Assert.AreEqual("user@gmail.com", userReponse.Email);
            Assert.AreEqual("fajgrnvia", userReponse.Password);
            Assert.AreEqual("+341341", userReponse.Cellphone);
            Assert.AreEqual(3314, userReponse.DNI);
            Assert.AreEqual(2, userReponse.RoleId);
        }
    }
}

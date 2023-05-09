using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodUsers.Infrastructure.API.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public async void CreateUser_Successfull()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices.Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>())).Returns(Task.CompletedTask);

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            var userRequestDTO = new UserRequestDTO();

            var response = (ObjectResult)await controller.CreateUser(userRequestDTO);

            Assert.AreEqual(200, response.StatusCode);
        }

        [TestMethod]
        public async void CreateUser_Error()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices.Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>())).Throws(new Exception());

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            var userRequestDTO = new UserRequestDTO();

            var response = (ObjectResult)await controller.CreateUser(userRequestDTO);

            Assert.AreEqual(500, response.StatusCode);
        }
    }
}

using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Exceptions;
using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.API.Controllers;
using FoodUsers.Infrastructure.API.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodUsers.Infrastructure.API.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public async Task CreateUserSuccessfull()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices
                .Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResponseDTO()
                {
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var userRequestDTO = new UserRequestDTO()
            {
                Name = "Pepito",
                Lastname = "Perezz",
                DNI = 3314,
                Cellphone = "+341341",
                Email = "user@gmail.com",
                Password = "fajgrnvia",
                RoleId = 2
            };

            var response = (OkObjectResult)await controller.CreateUser(userRequestDTO);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(UserResponseDTO));

            var userResponse = response.Value as UserResponseDTO;

            Assert.IsNotNull(userResponse);
            Assert.AreEqual("Pepito", userResponse.Name);
            Assert.AreEqual("Perezz", userResponse.Lastname);
            Assert.AreEqual("user@gmail.com", userResponse.Email);
            Assert.AreEqual("fajgrnvia", userResponse.Password);
            Assert.AreEqual("+341341", userResponse.Cellphone);
            Assert.AreEqual(3314, userResponse.DNI);
            Assert.AreEqual(2, userResponse.RoleId);
        }

        [TestMethod]
        public async Task CreateUser_Error()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices.Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>(), It.IsAny<string>()))
                .Throws(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var userRequestDTO = new UserRequestDTO();

            var response = (ObjectResult)await controller.CreateUser(userRequestDTO);

            Assert.AreEqual(500, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateUser_BadRequest()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices.Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>(), It.IsAny<string>()))
                .Throws(new ValidationModelException("message error"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var userRequestDTO = new UserRequestDTO();

            var response = (ObjectResult)await controller.CreateUser(userRequestDTO);

            Assert.AreEqual(400, response.StatusCode);
        }

        [TestMethod]
        public async Task Login_Successfull()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockSection = new Mock<IConfigurationSection>();

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            mockUserServices
                .Setup(p => p.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResponseDTO()
                {
                    Id = 1,
                    Name = "Pepito",
                    Lastname = "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "pepito@gmail.com",
                    Password = "pepito123",
                    RoleId = 2
                }));

            mockConfiguration.Setup(ap => ap.Value)
                             .Returns(new JWTSettings()
                             {
                                 Key = "keylongkeysooolooooong",
                                 Issuer = "af",
                                 Audience = "faf"
                             });

            var response = (OkObjectResult)await controller.Login(new Login
            {
                Email = "pepito@gmail.com",
                Password = "pepito123"
            });

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsNotNull(response.Value);

            var token = response.Value.ToString();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            IEnumerable<Claim> claims = jwtToken.Claims;

            var roleId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            Assert.IsNotNull(roleId);
            Assert.AreEqual("2", roleId.Value);

            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            Assert.IsNotNull(email);
            Assert.AreEqual("pepito@gmail.com", email.Value);

            var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Assert.IsNotNull(userId);
            Assert.AreEqual("1", userId.Value);
        }

        [TestMethod]
        public async Task Login_PasswordIsIncorrect()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockSection = new Mock<IConfigurationSection>();

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            mockUserServices
                .Setup(p => p.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new InvalidLoginCredentialsException("message error"));

            mockConfiguration.Setup(ap => ap.Value)
                             .Returns(new JWTSettings()
                             {
                                 Key = "keylongkeysooolooooong",
                                 Issuer = "af",
                                 Audience = "faf"
                             });

            var response = (ObjectResult)await controller.Login(new Login
            {
                Email = "pepito@gmail.com",
                Password = "pepito123"
            });

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("bad request. Errors: message error", apiResult.Message);
        }

        [TestMethod]
        public async Task Login_UserNotExists()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockSection = new Mock<IConfigurationSection>();

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            mockUserServices
                .Setup(p => p.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<UserResponseDTO?>(null));

            mockConfiguration.Setup(ap => ap.Value)
                             .Returns(new JWTSettings()
                             {
                                 Key = "keylongkeysooolooooong",
                                 Issuer = "af",
                                 Audience = "faf"
                             });

            var response = (ObjectResult)await controller.Login(new Login
            {
                Email = "pepito@gmail.com",
                Password = "pepito123"
            });

            Assert.AreEqual(404, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(404, apiResult.StatusCode);
            Assert.AreEqual("The user does not exist", apiResult.Message);
        }

        [TestMethod]
        public async Task Login_Error()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockSection = new Mock<IConfigurationSection>();

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object);

            mockUserServices
                .Setup(p => p.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("message error"));

            mockConfiguration.Setup(ap => ap.Value)
                             .Returns(new JWTSettings()
                             {
                                 Key = "keylongkeysooolooooong",
                                 Issuer = "af",
                                 Audience = "faf"
                             });

            var response = (ObjectResult)await controller.Login(new Login
            {
                Email = "pepito@gmail.com",
                Password = "pepito123"
            });

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod] 
        public void ValidateTokenSuccesfull()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();

            mockUserServices.Setup(p => p.CreateUser(It.IsAny<UserRequestDTO>(), It.IsAny<string>()))
                .Throws(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (OkResult)controller.ValidateToken();

            Assert.AreEqual(200, response.StatusCode);
        }

        [TestMethod]
        public void ValidateTokenError()
        {
            var mockUserServices = new Mock<IUserServices>();
            var mockConfiguration = new Mock<IOptions<JWTSettings>>();
            var mockLogger = new Mock<ILogger<UserController>>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "f.f.a";

            var controller = new UserController(mockUserServices.Object, mockConfiguration.Object, mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)controller.ValidateToken();

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }
    }
}

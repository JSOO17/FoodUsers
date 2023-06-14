using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Exceptions;
using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.API.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodUsers.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IOptions<JWTSettings> _configJwt;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserServices userServices, IOptions<JWTSettings> configJwt, ILogger<UserController> logger)
        {
            _userServices = userServices;
            _configJwt = configJwt;
            _logger = logger;
        }

        [HttpPost("/api/Login")]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            try
            {
                var user = await Authenticate(login);

                if (user != null)
                {
                    var token = GenerateToken(user);
                    _logger.LogDebug("User authenticated successfull");

                    return Ok(token);
                }

                _logger.LogInformation("The user does not exist");
                return StatusCode(404, new ApiResult
                {
                    StatusCode = 404,
                    Message = "The user does not exist"
                });
            }
            catch (InvalidLoginCredentialsException ex)
            {
                _logger.LogDebug("bad request. Errors: {0}", ex.Message);

                return StatusCode(400, new ApiResult
                {
                    StatusCode = 400,
                    Message = $"bad request. Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Something was wrong", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong. Exception: " + ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO user)
        {
            try
            {
                var identityRoleId = GetClaim(ClaimTypes.Role).Value ?? string.Empty;

                var userResponse = await _userServices.CreateUser(user, identityRoleId);

                _logger.LogDebug("User {0} was found successfull", user.Name);

                return Ok(userResponse);
            }
            catch (RoleHasNotPermissionException ex)
            {
                _logger.LogDebug("forbbiden. Errors: {0}", ex.Message);

                return StatusCode(403, new ApiResult
                {
                    StatusCode = 403,
                    Message = $"forbbiden. Errors: {ex.Message}"
                });
            }
            catch (ValidationModelException ex)
            {
                _logger.LogDebug("bad request. Errors: {0}", ex.Message);

                return StatusCode(400, new ApiResult
                {
                    StatusCode = 400,
                    Message = $"bad request. Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong"
                });
            }
        }

        [HttpPost]
        [Route("ValidateToken")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            try
            {
                var identityRoleId = GetClaim(ClaimTypes.Role).Value ?? string.Empty;

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong"
                });
            }
        }

        private Claim GetClaim(string claimType)
        {
            var auth = Request.Headers.TryGetValue("Authorization", out var value) ? value.ToString() : string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(auth.Replace(JwtBearerDefaults.AuthenticationScheme + " ", string.Empty));

            var claims = jwtToken.Claims;

            if (claims != null)
            {
                if (claims != null)
                {
                    return claims.FirstOrDefault(x => x.Type == claimType) ?? new Claim(string.Empty, string.Empty);
                }
            }

            return new Claim(string.Empty, string.Empty);
        }

        private string GenerateToken(UserResponseDTO user)
        {
            var jwtSettings = _configJwt.Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.Cellphone),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var token = new JwtSecurityToken(claims: claims,
                                            expires: DateTime.UtcNow.AddDays(1),
                                            signingCredentials: credentials); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserResponseDTO?> Authenticate(Login login)
        {
            return await _userServices.GetUser(login.Email, login.Password);
        }
    }

    public class ApiResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

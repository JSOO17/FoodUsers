using FoodUsers.Application.DTO.Request;
using FoodUsers.Application.DTO.Response;
using FoodUsers.Application.Services.Interfaces;
using FoodUsers.Domain.Models;
using FoodUsers.Domain.Utils;
using FoodUsers.Infrastructure.API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodUsers.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserServices userServices, IConfiguration config, ILogger<UserController> logger)
        {
            _userServices = userServices;
            _config = config;
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

                _logger.LogInformation("The password or user is invalid");
                return NotFound("The password or user is invalid");
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                return Problem("Something was wrong");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO user)
        {
            try
            {
                await _userServices.CreateUser(user);
                _logger.LogDebug("User {0} was found successfull", user.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                return Problem("Something was wrong");
            }
        }

        private Claim GetClaim(string claimType)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;

                if (claims != null)
                {
                    return claims.FirstOrDefault(x => x.Type == claimType) ?? new Claim(string.Empty, string.Empty);
                }
            }

            return new Claim(string.Empty, string.Empty);
        }

        private string GenerateToken(UserResponseDTO user)
        {
            var jwtSettings = _config.GetSection("Jwt").Get<JWTSettings>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RolId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(jwtSettings.Issuer,
                                            jwtSettings.Audience,
                                            claims,
                                            expires: DateTime.UtcNow.AddDays(1),
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserResponseDTO> Authenticate(Login login)
        {
            return await _userServices.GetUser(login.Email, login.Password);
        }
    }
}

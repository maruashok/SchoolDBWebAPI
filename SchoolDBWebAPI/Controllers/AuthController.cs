using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDBWebAPI.DAL.UserIdentity;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models;
using SchoolDBWebAPI.Services.UsersDBModels;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService authService)
        {
            service = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }
            else
            {
                RequestResponse response = await service.RegisterUserAsync(userDetails);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            ApplicationUser identityUser;

            if (!ModelState.IsValid || credentials == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }
            else
            {
                RequestResponse response = await service.LoginUserAsync(credentials);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            else
            {
                RequestResponse response = await service.RefreshToken(tokenModel);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(string username)
        {
            RequestResponse response = await service.Logout(username);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
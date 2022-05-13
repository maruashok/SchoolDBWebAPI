using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolDBWebAPI.Services.UsersDBModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UsersDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        public AuthController(IOptions<JwtBearerTokenSettings> _jwtTokenOptions, UserManager<ApplicationUser> _userManager, UsersDbContext _dbContext)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            jwtBearerTokenSettings = _jwtTokenOptions.Value;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var userExists = await userManager.FindByNameAsync(userDetails.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var identityUser = new ApplicationUser() { UserName = userDetails.Username, Email = userDetails.Email };
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            ApplicationUser identityUser;

            if (!ModelState.IsValid || credentials == null || (identityUser = await ValidateUser(credentials)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }
            else
            {
                // Generate access token
                var tokenDescriptor = await CreateTokenAsync(identityUser);

                // Generate refresh token and set it to cookie
                var refreshToken = GenerateRefreshToken();

                identityUser.RefreshToken = refreshToken;
                identityUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(jwtBearerTokenSettings.RefreshTokenExpiryInDays);

                await userManager.UpdateAsync(identityUser);

                return Ok(new
                {
                    Email = identityUser.Email,
                    RefreshToken = refreshToken,
                    Username = identityUser.UserName,
                    Expiration = tokenDescriptor.Expires,
                    Token = new JwtSecurityTokenHandler().WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                });
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

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;

            var identityUser = await userManager.FindByNameAsync(username);

            if (identityUser == null || identityUser.RefreshToken != refreshToken || identityUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var tokenDescriptor = await CreateTokenAsync(identityUser);
            var newRefreshToken = GenerateRefreshToken();

            identityUser.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(identityUser);

            return new ObjectResult(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return NoContent();
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey)),
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private async Task<ApplicationUser> ValidateUser(LoginCredentials credentials)
        {
            var identityUser = await userManager.FindByNameAsync(credentials.Username);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        private async Task<SecurityTokenDescriptor> CreateTokenAsync(ApplicationUser identityUser)
        {
            var userRoles = await userManager.GetRolesAsync(identityUser);
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, identityUser.Email),
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return new SecurityTokenDescriptor
            {
                Issuer = jwtBearerTokenSettings.Issuer,
                Subject = new ClaimsIdentity(authClaims),
                Audience = jwtBearerTokenSettings.Audience,
                Expires = DateTime.Now.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
        }
    }
}
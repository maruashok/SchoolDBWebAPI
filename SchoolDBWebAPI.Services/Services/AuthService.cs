using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models;
using SchoolDBWebAPI.Services.UsersDBModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtBearerTokenSettings> jwtBearerTokenSettings)
        {
            this.tokenHandler = new();
            this.userManager = userManager;
            this.jwtBearerTokenSettings = jwtBearerTokenSettings.Value;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RequestResponse> Logout(string username)
        {
            RequestResponse response = new RequestResponse();

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid user name";
            }

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            response.Success = true;

            return response;
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

        public async Task<RequestResponse> RefreshToken(TokenModel tokenModel)
        {
            RequestResponse response = new RequestResponse();

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                response.Success = false;
                response.Message = "Invalid access token or refresh token";
            }

            string username = principal.Identity.Name;

            var identityUser = await userManager.FindByNameAsync(username);

            if (identityUser == null || identityUser.RefreshToken != refreshToken || identityUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                response.Success = false;
                response.Message = "Invalid access token or refresh token";
            }

            var tokenDescriptor = await CreateTokenAsync(identityUser);
            var newRefreshToken = GenerateRefreshToken();

            identityUser.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(identityUser);

            response.Success = true;
            response.Data = new
            {
                refreshToken = newRefreshToken,
                Token = new JwtSecurityTokenHandler().WriteToken(tokenHandler.CreateToken(tokenDescriptor))
            };

            return response;
        }

        public async Task<RequestResponse> RegisterUserAsync(UserDetails userDetails)
        {
            RequestResponse response = new RequestResponse();

            var userExists = await userManager.FindByNameAsync(userDetails.Username);

            if (userExists != null)
            {
                response.Success = false;
                response.Message = "User already exists!";
            }
            else
            {
                var identityUser = new ApplicationUser() { UserName = userDetails.Username, Email = userDetails.Email };
                var result = await userManager.CreateAsync(identityUser, userDetails.Password);

                if (!result.Succeeded)
                {
                    var dictionary = new ModelStateDictionary();
                    foreach (IdentityError error in result.Errors)
                    {
                        dictionary.AddModelError(error.Code, error.Description);
                    }

                    response.Success = false;
                    response.Data = dictionary;
                    response.Message = "User Registration Failed";
                }
                else
                {
                    response.Success = true;
                    response.Message = "User Registration Successfull";
                }
            }

            return response;
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

        public async Task<RequestResponse> LoginUserAsync(LoginCredentials credentials)
        {
            RequestResponse response = new RequestResponse();

            ApplicationUser identityUser = await ValidateUser(credentials);

            if (identityUser == null)
            {
                response.Success = false;
                response.Message = "Login failed";
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

                response.Success = true;
                response.Data = new
                {
                    Email = identityUser.Email,
                    RefreshToken = refreshToken,
                    Username = identityUser.UserName,
                    Expiration = tokenDescriptor.Expires,
                    Token = new JwtSecurityTokenHandler().WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                };
            }

            return response;
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
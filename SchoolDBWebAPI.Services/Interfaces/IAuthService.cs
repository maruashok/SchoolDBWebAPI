using SchoolDBWebAPI.Services.Models;
using SchoolDBWebAPI.Services.UsersDBModels;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RequestResponse> LoginUserAsync(LoginCredentials credentials);

        Task<RequestResponse> Logout(string username);

        Task<RequestResponse> RefreshToken(TokenModel tokenModel);

        Task<RequestResponse> RegisterUserAsync(UserDetails userDetails);
    }
}
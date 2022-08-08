using Microsoft.AspNetCore.Identity;

namespace SchoolDBWebAPI.DAL.UserIdentity
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
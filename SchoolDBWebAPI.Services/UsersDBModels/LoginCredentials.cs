using System.ComponentModel.DataAnnotations;

namespace SchoolDBWebAPI.Services.UsersDBModels
{
    public class LoginCredentials
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
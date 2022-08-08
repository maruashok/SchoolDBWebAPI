using Microsoft.Extensions.DependencyInjection;
using SchoolDBWebAPI.DAL.Interfaces;
using SchoolDBWebAPI.DAL.Repository;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Services;

namespace SchoolDBWebAPI.Dependency
{
    public static class ServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
        }

        public static void AddRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IQuizDetailService, QuizDetailService>();
        }
    }
}
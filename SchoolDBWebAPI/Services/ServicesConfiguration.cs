using Microsoft.Extensions.DependencyInjection;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;

namespace SchoolDBWebAPI.Dependency
{
    public static class ServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuizQuesRepository, QuizQuesRepository>();
        }

        public static void AddRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizDetailService, QuizDetailService>();
            services.AddScoped<IQuizQuesRepository, QuizQuesRepository>();
        }
    }
}
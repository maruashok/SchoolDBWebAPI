using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolDBWebAPI.DAL.UserIdentity
{
    public class UsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
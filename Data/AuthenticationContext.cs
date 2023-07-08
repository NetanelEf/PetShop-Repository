using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AuthenticationContext : IdentityDbContext<IdentityUser>
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<AuthenticationContext>(options => options.UseSqlite("ConnectionString:DefaultConnection"));
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 7;
                //options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AuthenticationContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "UserCookie";
            });
        }
    }
}

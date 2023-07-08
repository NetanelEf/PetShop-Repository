using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AdminPageModels
    {
        public Animals Animals { get; set; } = new Animals();
        public Categories Categories { get; set; } = new Categories();
        public IEnumerable<IdentityUser> IdentityUsers { get; set; } = Enumerable.Empty<IdentityUser>();
        public IEnumerable<IdentityRole> Roles { get; set; } = Enumerable.Empty<IdentityRole>();
    }
}

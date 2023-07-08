using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class PetShopContext : DbContext
    {
        public DbSet<Animals> Animal { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Comments> Comment { get; set; }
        public PetShopContext(DbContextOptions<PetShopContext> options) : base(options)
        {
            
        }
    }
}

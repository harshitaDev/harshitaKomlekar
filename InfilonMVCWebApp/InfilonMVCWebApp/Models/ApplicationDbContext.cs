using Microsoft.EntityFrameworkCore;
namespace InfilonMVCWebApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> tblUsers { get; set; }
    }
}

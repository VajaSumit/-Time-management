using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserLogManagement.Models
{
    public class DbContextFile : IdentityDbContext<IdentityUser>
    {
        public DbContextFile(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Applicationuser> applicationusers { get; set; }

        public DbSet<userlog> userlogs { get; set; }


    }
}

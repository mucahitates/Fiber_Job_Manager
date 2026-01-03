using Microsoft.EntityFrameworkCore;
using FiberJobManager.Api.Models;

namespace FiberJobManager.Api.Data
{

    using FiberJobManager.Api.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }
        
        public DbSet<Revision> Revisions { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Person> People { get; set; }


    }
}

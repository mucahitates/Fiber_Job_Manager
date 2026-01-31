using Microsoft.EntityFrameworkCore;
using FiberJobManager.Api.Models;

namespace FiberJobManager.Api.Data
{

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

        public DbSet<TempDocument> TempDocuments { get; set; }

        public DbSet<JobFieldReport> JobFieldReports { get; set; }

        public DbSet<JobRevisionHistory> JobRevisionHistories { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Region> Regions { get; set; }

    }
}

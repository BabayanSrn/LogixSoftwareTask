using LogixSoftwareTask.Storage.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogixSoftwareTask.DataAccessLayer.Data
{
    public class LogixDbContext : IdentityDbContext<User, IdentityRole<string>, string>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Classes> Classes { get; set; }

        public LogixDbContext(DbContextOptions<LogixDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Classes)
                .WithMany(c => c.Users);

            base.OnModelCreating(modelBuilder);
        }
    }
}

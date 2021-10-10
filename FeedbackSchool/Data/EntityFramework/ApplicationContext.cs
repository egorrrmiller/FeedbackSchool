using FeedbackSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSchool.Data.EntityFramework
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Guest> FeedbackList { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Startup.Connection);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
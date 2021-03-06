using FeedbackSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSchool.Data;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<FeedbackModel> Feedback { get; set; }

    public DbSet<ClassModel> Class { get; set; }
    public DbSet<SchoolModel> School { get; set; }
}
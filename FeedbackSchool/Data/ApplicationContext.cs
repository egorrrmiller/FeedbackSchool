using FeedbackSchool.Models.FeedbackViewModels;
using FeedbackSchool.Models.ManageViewModels;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSchool.Data;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<FeedbackModel> Feedback { get; set; }

    public DbSet<ManageModel> Manage { get; set; }
}
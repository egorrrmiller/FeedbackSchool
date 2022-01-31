﻿using FeedbackSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSchool.Data.EntityFramework;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<FeedbackList> FeedbackList { get; set; }
    public DbSet<FeedbackModel> FeedbackModel { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Startup.Connection);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedbackModel>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}
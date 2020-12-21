using System;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;

namespace Taskr.Persistance
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>().HasData(new Task
            {
                Id = Guid.Parse("64fa643f-2d35-46e7-b3f8-31fa673d719b"),
                Title = "Garden Trimming in Lagos Ajah",
                Description = "Please my garden needs trimmin, Im in lagos",
                InitialPrice = 20.30m
            });
        }
    }
}
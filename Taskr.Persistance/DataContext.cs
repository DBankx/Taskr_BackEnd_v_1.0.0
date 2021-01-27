using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;

namespace Taskr.Persistance
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Watch> Watches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>().HasData(new Job
            {
                Id = Guid.Parse("64fa643f-2d35-46e7-b3f8-31fa673d719b"),
                Title = "Garden Trimming in Lagos Ajah",
                Description = "Please my garden needs trimmin, Im in lagos",
                InitialPrice = 20.30m
            });

            modelBuilder.Entity<ApplicationUser>().OwnsMany(p => p.SkillSet, a =>
            {
                a.WithOwner().HasForeignKey("OwnerId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });
            
            modelBuilder.Entity<ApplicationUser>().OwnsMany(p => p.Languages, a =>
            {
                a.WithOwner().HasForeignKey("OwnerId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });

            modelBuilder.Entity<ApplicationUser>().OwnsOne(p => p.Socials);

        }
    }
}
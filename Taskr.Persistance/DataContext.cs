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
        public DbSet<AppUserNotification> UserNotifications { get; set; }
        
        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
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

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.CreatedJobs)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.AssignedJobs)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>().OwnsOne(x => x.BankAccount);

        }
    }
}
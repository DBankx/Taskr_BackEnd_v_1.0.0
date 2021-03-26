using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Taskr.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Job> CreatedJobs { get; set; }
        public virtual ICollection<Job> AssignedJobs{ get; set; }
        public string Avatar { get; set; }
        public virtual ICollection<Watch> Watching { get; set; }
        public string Tagline { get; set; }
        public Socials Socials { get; set; } 
        public virtual ICollection<Skill> SkillSet { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        
        public string StripeCustomerId { get; set; }

    }

    public enum ExperienceLevel
    {
        Beginner,
        Intermediate,
        Expert
    }
}
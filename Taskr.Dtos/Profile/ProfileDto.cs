using System;
using System.Collections.Generic;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.Dtos.Profile
{
    // TODO - add reviews
    public class ProfileDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public BankAccount BankAccount { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public ICollection<Skill> SkillSet { get; set; }
        public ICollection<Language> Languages { get; set; }
        public Socials Socials { get; set; }
        public string Tagline { get; set; }
        public double AvgReviewRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}
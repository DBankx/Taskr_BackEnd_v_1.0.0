using System;
using System.Collections.Generic;
using Taskr.Domain;

namespace Taskr.Dtos.Profile
{
    public class PublicProfileDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public ICollection<Skill> SkillSet { get; set; }
        public ICollection<Language> Languages { get; set; }
        public Socials Socials { get; set; }
        public string Tagline { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Taskr.Dtos.Job;

namespace Taskr.Dtos.Profile
{
    // TODO - add reviews
    public class ProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
    }
}
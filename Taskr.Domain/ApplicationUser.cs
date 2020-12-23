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
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Job> CreatedJobs { get; set; }
    }
}
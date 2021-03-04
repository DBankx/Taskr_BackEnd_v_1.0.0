using System;

namespace Taskr.Dtos.Job
{
    public class JobUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Bio { get; set; }
    }
}
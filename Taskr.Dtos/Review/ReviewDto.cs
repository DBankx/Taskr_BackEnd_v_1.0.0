using System;
using Taskr.Domain;

namespace Taskr.Dtos.Review
{
    public class ReviewDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PostedAt { get; set; }
        public ReviewJobDto Job { get; set; }
        public ReviewUserDto Reviewer { get; set; }
    }
}
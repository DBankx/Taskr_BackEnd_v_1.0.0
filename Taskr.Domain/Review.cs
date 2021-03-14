using System;

namespace Taskr.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public DateTime PostedAt { get; set; }
        public ApplicationUser Reviewer { get; set; }
        public ApplicationUser Reviewee { get; set; }
        public ReviewType Type { get; set; }
    }

    public enum ReviewType
    {
        Taskr,
        Runner
    }
}
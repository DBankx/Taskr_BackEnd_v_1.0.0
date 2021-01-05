using System;

namespace Taskr.Domain
{
    public class Watch
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Job Job { get; set; }
        public DateTime WatchedAt { get; set; }
    }
}
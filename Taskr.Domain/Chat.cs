using System;
using System.Collections.Generic;

namespace Taskr.Domain
{
    public class Chat
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser Taskr { get; set; }
        public virtual ApplicationUser Runner { get; set; }
        public virtual Job Job { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
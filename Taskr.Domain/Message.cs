using System;

namespace Taskr.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public Chat Chat { get; set; }
    }
}
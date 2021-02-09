using MediatR;

namespace Taskr.Infrastructure.MediatrNotifications
{
    public class MailRequestNotification : INotification
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        
    }
}
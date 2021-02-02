using System;
using Taskr.Domain;

namespace Taskr.Dtos.Profile
{
    public class UserNotificationDto
    {
        public Guid Id { get; set; }
        public string ToUserId { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvatar { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public Guid NotifierObjectId { get; set; }
    }
}
using System;
using MediatR;
using Taskr.Domain;

namespace Taskr.Infrastructure.UserNotification
{
    public class UserPrivateMessageNotification : INotification
    {
        public string ToUserId { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvatar { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public Guid NotifierObjectId { get; set; }

        public UserPrivateMessageNotification(
            string toUserId, 
            string fromUserId, string fromUserName, string fromUserAvatar, 
            string message, Guid notifierObjectId, DateTime createdAt, NotificationType type, NotificationStatus status)
        {
            ToUserId = toUserId;
            FromUserId = fromUserId;
            FromUserAvatar = fromUserAvatar;
            FromUserName = fromUserName;
            Message = message;
            NotifierObjectId = notifierObjectId;
            CreatedAt = createdAt;
            Type = type;
            Status = status;
        }
    }
}
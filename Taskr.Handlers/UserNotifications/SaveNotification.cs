using System.Threading;
using MediatR;
using Taskr.Domain;
using Taskr.Infrastructure.UserNotification;
using Taskr.Persistance;

namespace Taskr.Handlers.UserNotifications
{
    public class SaveNotification : INotificationHandler<UserPrivateMessageNotification>
    {
        private readonly DataContext _context;

        public SaveNotification(DataContext context)
        {
            _context = context;
        } 
        
        public async System.Threading.Tasks.Task Handle(UserPrivateMessageNotification notification, CancellationToken cancellationToken)
        {
            var userNotif = new AppUserNotification
            {
                Message = notification.Message,
                Status = notification.Status,
                Type = notification.Type,
                CreatedAt = notification.CreatedAt,
                FromUserAvatar = notification.FromUserAvatar,
                ToUserId = notification.ToUserId,
                FromUserId = notification.FromUserId,
                FromUserName = notification.FromUserName,
                NotifierObjectId = notification.NotifierObjectId
            };

            _context.UserNotifications.Add(userNotif);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
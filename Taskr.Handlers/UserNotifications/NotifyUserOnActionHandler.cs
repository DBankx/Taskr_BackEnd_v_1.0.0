using System.Threading;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Taskr.Infrastructure.MediatrNotifications;
using Taskr.Infrastructure.SignalRHubs;

namespace Taskr.Handlers.UserNotifications
{
    public class NotifyUserOnActionHandler : INotificationHandler<UserPrivateMessageNotification>
    {
        private readonly IHubContext<PortalHub> _hubContext;

        public NotifyUserOnActionHandler(IHubContext<PortalHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async System.Threading.Tasks.Task Handle(UserPrivateMessageNotification notification, CancellationToken cancellationToken)
        {
            string recipientUserId = notification.ToUserId;

            await _hubContext
                .Clients.User(recipientUserId).SendAsync("ReceiveNotification", notification, cancellationToken);
        }
    }
}
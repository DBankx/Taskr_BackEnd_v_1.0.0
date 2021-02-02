using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Profile;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.UserNotifications
{
    public class MarkAllNotificationsAsReadHandler : IRequestHandler<MarkAllNotificationsAsRead>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public MarkAllNotificationsAsReadHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        public async Task<Unit> Handle(MarkAllNotificationsAsRead request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null) throw new RestException(HttpStatusCode.Unauthorized, "You are not authorized");

            var notifications = await _context.UserNotifications.Where(x => x.ToUserId == user.Id).ToListAsync(cancellationToken);

            foreach (var notif in notifications)
            {
                notif.Status = NotificationStatus.Read;
            }

            var read = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(read) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
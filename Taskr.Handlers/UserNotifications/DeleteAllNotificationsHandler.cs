using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Profile;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.UserNotifications
{
    public class DeleteAllNotificationsHandler : IRequestHandler<DeleteAllNotifications>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeleteAllNotificationsHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        public async Task<Unit> Handle(DeleteAllNotifications request, CancellationToken cancellationToken)
        {
           var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);
           
                       if (user == null) throw new RestException(HttpStatusCode.Unauthorized, "You are not authorized");

                       var notifications = await _context.UserNotifications.Where(x => x.ToUserId == user.Id).ToListAsync(cancellationToken);
           
          
                       _context.UserNotifications.RemoveRange(notifications);
           
                       var deleted = await _context.SaveChangesAsync(cancellationToken) > 0;
                       
                       if(deleted) return Unit.Value;
           
                       throw new Exception("Problem saving changes");
        }
    }
}
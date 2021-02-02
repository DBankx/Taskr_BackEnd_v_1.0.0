﻿using System;
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
    public class DeleteNotificationHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeleteNotificationHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null) throw new RestException(HttpStatusCode.Unauthorized, "You are not authorized");

            var notification = await _context.UserNotifications.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (notification == null)
                throw new RestException(HttpStatusCode.NotFound, new {error = "Notification not found"});

            if (notification.ToUserId != user.Id)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to delete this notification"});

            _context.UserNotifications.Remove(notification);

            var deleted = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(deleted) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
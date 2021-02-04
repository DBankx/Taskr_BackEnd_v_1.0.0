using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Bid;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Infrastructure.UserNotification;
using Taskr.Persistance;

namespace Taskr.Handlers.Bid
{
    public class MarkBidAsSeenHandler : IRequestHandler<MarkBidAsSeen>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IMediator _mediator;

        public MarkBidAsSeenHandler(DataContext context, IUserAccess userAccess, IMediator mediator)
        {
            _context = context;
            _userAccess = userAccess;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(MarkBidAsSeen request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {errors = "You are un authorized"});

            var bid = await _context.Bids.SingleOrDefaultAsync(x => x.Id == request.BidId, cancellationToken);

            if (bid == null) throw new RestException(HttpStatusCode.NotFound, new {bid = "Not found"});

            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == bid.JobId, cancellationToken);
            
            if(job.UserId != user.Id)  throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized to complete this action"});

            if (bid.Status == BidStatus.Seen)
                throw new RestException(HttpStatusCode.BadRequest,
                    new {error = "You have already marked this bid as seen"});
            
            bid.Status = BidStatus.Seen;

            var changed = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(!changed) throw new Exception("Problem saving changes");
            
            var appNotif = new UserPrivateMessageNotification(bid.UserId, user.Id, user.UserName, user.Avatar,
                            $"{user.UserName} has seen your bid on '{job.Title}'", job.Id, DateTime.Now, NotificationType.Bid,
                            NotificationStatus.UnRead);
            
                        await _mediator.Publish(appNotif, cancellationToken);
            
            return Unit.Value;
        }
    }
}
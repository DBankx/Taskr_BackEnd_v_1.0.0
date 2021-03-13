using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Order;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.MediatrNotifications;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Order
{
    public class RequestPayoutCommand : IRequestHandler<RequestPayout>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IMediator _mediator;

        public RequestPayoutCommand(DataContext context, IUserAccess userAccess, IMediator mediator)
        {
            _context = context;
            _userAccess = userAccess;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(RequestPayout request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});
            
            var order = await _context.Orders.Include(x => x.User).Include(x => x.PayTo).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});
            
            if(order.PayTo != user) throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are not allowed to complete this request"});

            order.Status = OrderStatus.AwaitingPayout;

            var requested = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(!requested) throw new Exception("Problem saving changes");

            _mediator.Publish(new UserPrivateMessageNotification(order.User.Id, order.PayTo.Id, order.PayTo.UserName,
                order.PayTo.Avatar, $"{order.PayTo.UserName} has requested a payout on '{order.Job.Title}'", order.Id,
                DateTime.Now, NotificationType.Order, NotificationStatus.UnRead));

            return Unit.Value;

        }
    }
}
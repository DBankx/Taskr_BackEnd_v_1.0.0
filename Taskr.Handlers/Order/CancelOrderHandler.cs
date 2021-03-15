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
    public class CancelOrderHandler : IRequestHandler<CancelOrder>
    {
        private readonly DataContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccess _userAccess;

        public CancelOrderHandler(DataContext context, IMediator mediator, IUserAccess userAccess)
        {
            _context = context;
            _mediator = mediator;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(CancelOrder request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.Include(x => x.User).Include(x => x.Job).Include(x => x.AcceptedBid).Include(x => x.PayTo).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber,
                cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});

            if (order.User != user)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});
            
            order.Status = OrderStatus.Cancelled;
            order.Job.JobStatus = JobStatus.Active;
            order.Job.AssignedUser = null;
            order.AcceptedBid.Status = BidStatus.Submitted;
            order.AcceptedBid = null;

            var cancelledOrder = await _context.SaveChangesAsync() > 0;

            if (!cancelledOrder) throw new Exception("Problem saving changes");

            _mediator.Publish(new UserPrivateMessageNotification(order.PayTo.Id, order.User.Id, order.User.UserName,
                order.User.Avatar, $"{order.User.UserName} cancelled the order on {order.Job.Title}", order.Id,
                DateTime.Now, NotificationType.Order, NotificationStatus.UnRead));
            
            return Unit.Value;
        }
    }
}
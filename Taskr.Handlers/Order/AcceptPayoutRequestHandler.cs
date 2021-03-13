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
    public class AcceptPayoutRequestHandler : IRequestHandler<AcceptPayoutRequest>
    {
        private readonly IUserAccess _userAccess;
        private readonly IMediator _mediator;
        private readonly DataContext _context;

        public AcceptPayoutRequestHandler(IUserAccess userAccess, IMediator mediator, DataContext context)
        {
            _userAccess = userAccess;
            _mediator = mediator;
            _context = context;
        }
        
        public async Task<Unit> Handle(AcceptPayoutRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.Include(x => x.User).Include(x => x.PayTo).Include(x => x.Job).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});

            if (order.User != user)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});

            order.Status = OrderStatus.Completed;
            order.Job.JobStatus = JobStatus.Completed;
            order.OrderCompletedDate = DateTime.Now;

            var accepted = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!accepted) throw new Exception("Problem saving changes");
            
            _mediator.Publish(new UserPrivateMessageNotification(order.PayTo.Id, order.User.Id, order.User.UserName,
                order.User.Avatar, $"{order.User.UserName} has accepted your payout on '{order.Job.Title}'", order.Id,
                DateTime.Now, NotificationType.Order, NotificationStatus.UnRead));

            return Unit.Value;

        }
    }
}
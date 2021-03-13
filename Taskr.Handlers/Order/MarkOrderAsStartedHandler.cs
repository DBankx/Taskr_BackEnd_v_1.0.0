using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.MediatrNotifications;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;
using Taskr.Queries.Order;

namespace Taskr.Handlers.Order
{
    public class MarkOrderAsStartedHandler : IRequestHandler<MarkOrderAsStarted>
    {
        private readonly DataContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccess _userAccess;

        public MarkOrderAsStartedHandler(DataContext context, IMediator mediator, IUserAccess userAccess)
        {
            _context = context;
            _mediator = mediator;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(MarkOrderAsStarted request, CancellationToken cancellationToken)
        {
            var loggedInUser =
                await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                    cancellationToken);

            if (_userAccess == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.Include(x => x.PayTo).Include(x => x.User).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order== null) throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});

            if (order.PayTo.Id != loggedInUser.Id)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized to complete this action"});

            order.Status = OrderStatus.Started;
            order.OrderStartedDate = DateTime.Now;
            
            var saved = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!saved) throw new Exception("Problem saving changes");
            
               _mediator.Publish(new UserPrivateMessageNotification(order.User.Id, order.PayTo.Id, order.PayTo.UserName, order.PayTo.Avatar, $"{order.PayTo.UserName} has started your task", order.Id, DateTime.Now, NotificationType.Order, NotificationStatus.UnRead));
            
                        // _mediator.Publish(new MailRequestNotification
                        // {
                        //     To = order.User.Email,
                        //     Subject = "Your order has started",
                        //     Body = $"<div>" +
                        //            $"<h3>Hi {order.User.FirstName},</h3>" +
                        //            $"Your order #<a href='{Environment.GetEnvironmentVariable("CLIENT_URL")}/order/{order.OrderNumber}'>{order.OrderNumber}</a> has been started." +
                        //            $"<p>Rest assured it will be finished before the delivery date provided</p>" +
                        //            $"<a style='border-radius:4px;background-color:#373373;color:#fff;margin-top:2em;padding:1em;text-decoration:none;' href='{Environment.GetEnvironmentVariable("CLIENT_URL")}/order/{order.OrderNumber}' >View Order</a>" +
                        //     $"</div>"
                        // });
            
            return Unit.Value;


        }
    }
}
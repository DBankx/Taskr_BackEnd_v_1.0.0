using System;
using System.Linq;
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
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;
        private readonly IMediator _mediator;

        public ConfirmOrderHandler(DataContext context, IUserAccess userAccess, IMediator mediator)
        {
            _context = context;
            _userAccess = userAccess;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.Include(x => x.User).Include(x => x.Job).ThenInclude(x => x.Photos).Include(x => x.AcceptedBid).Include(x => x.PayTo).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber,
                cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});
            if (user != order.User)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});

            order.Status = OrderStatus.Confirmed;
            order.Job.JobStatus = JobStatus.Assigned;
            order.Job.AssignedUser = order.PayTo;
            order.AcceptedBid.Status = BidStatus.Approved;

            var confirmed = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!confirmed) throw new Exception("Problem saving changes");
            

            // Send notification to runner
                        var appNotif = new UserPrivateMessageNotification(order.PayTo.Id, order.User.Id, order.User.UserName,
                            order.User.Avatar, $"{order.User.UserName} has accepted your bid on {order.Job.Title}", order.Job.Id,
                            DateTime.Now, NotificationType.Assign, NotificationStatus.UnRead);
                        
                        _mediator.Publish(appNotif, cancellationToken);
                        
                        _mediator.Publish(new MailRequestNotification
                        {
                            To = order.User.Email, Subject = $"Order notification for '{order.Job.Title}' payment ",
                            Body =
                               $"<div style='background-color:#EBF8F3;padding:2em;width:100%;text-align:center;color:#373373'><h1>Order Confirmation</h1><p>{order.User.FirstName} {order.User.LastName}, thank you for your order</p><p>We've sent your order to <a href='{Environment.GetEnvironmentVariable("CLIENT_URL")}/public-profile/{order.PayTo.Id}'><b>{order.PayTo.UserName}</b></a> to start work on your task. We'll let you know as soon as they've started.</p></div>" +
                               $"<h3 style='text-align:center'>Order Summary</h3>" +
                               $"<h5 style='text-align:center'>{order.OrderPlacementDate.ToString("D")}</h5>" +
                               $"<div style='display:flex;width:100%;justify-content:center'>" +
                               $"<img src={order.Job.Photos.ToList()[0].Url} alt='task-photo' style='width:300px;height:400px;margin-right:1.5em' />" +
                               $"<div>" +
                               $"<hr />" +
                               $"<div style='display:flex;justify-content:space-between'><p>Discount</p><p>$0.00</p></div>" +
                               $"<div style='display:flex;justify-content:space-between'><p>Bid amount paid</p><p>${order.AcceptedBid.Price}</p></div>" +
                               $"<div>" +
                               $"<hr />" +
                               $"<div style='display:flex;justify-content:space-between'><p>Order No</p><p>#{order.OrderNumber}</p></div>" +
                               $"<div style='display:flex;justify-content:space-between'><p>Original Budget</p><p>${order.Job.InitialPrice}</p></div>" +
                               $"<div style='display:flex;justify-content:space-between'><p>Runner</p><p><a href='{Environment.GetEnvironmentVariable("CLIENT_URL")}/public-profile/{order.PayTo.Id}'><b>{order.PayTo.UserName}</b></a></p></div>" +
                               $"</div>"+
                               $"</div>" +
                               $"</div>" +
                               $"<h3>Order total</h3>" +
                               $"<div>" +
                               $"<hr />" +
                               $"<div style='display:flex;justify-content:space-between'><p>Subtotal price</p><p>${order.AcceptedBid.Price}</p></div>" +
                               $"<div style='display:flex;justify-content:space-between'><p>Taskr service fee</p><p>$1.50</p></div>" +
                               $"<div style='display:flex;justify-content:space-between'><p>Tax</p><p>$1.00</p></div>" +
                               $"<hr />" +
                               $"<div style='display:flex;justify-content:space-between'><h1>Total price</h1><h3>${order.TotalAmount}</h3></div>" +
                               $"</div>" +
                               $"<div style='width:100%;text-align:center'>" +
                               $"<a href='{Environment.GetEnvironmentVariable("CLIENT_URL")}/orders/{order.OrderNumber}'  style='border-radius:4px;background-color:#373373;color:#fff;margin-top:2em;padding:1em;text-decoration:none;'>View order</a>"+
                               $"</div>" 
                        }, cancellationToken);
                        
                        
            return Unit.Value;

        }
    }
}
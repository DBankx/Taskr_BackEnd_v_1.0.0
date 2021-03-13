using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Order;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Order
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeleteOrderHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});

            if (order.User !=  user)
            {
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});
            }

            _context.Orders.Remove(order);

            var removed = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(removed) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
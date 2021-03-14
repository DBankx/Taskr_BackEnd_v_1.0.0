using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Review;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Review
{
    public class AddReviewHandler : IRequestHandler<AddReview>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public AddReviewHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(AddReview request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var order = await _context.Orders.Include(x => x.Job).Include(x => x.User).Include(x => x.PayTo).SingleOrDefaultAsync(x => x.OrderNumber == request.OrderNumber,
                cancellationToken);

            if (order == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Order not found"});

            var review = new Domain.Review
            {
                Text = request.Text,
                Order = order,
                PostedAt = DateTime.Now,
                Rating = request.Rating,
            };

            if (user == order.User)
            {
                review.Type = ReviewType.Runner;
                review.Reviewer = order.User;
                review.Reviewee = order.PayTo;
            } else if (user == order.PayTo)
            {
                review.Type = ReviewType.Taskr;
                review.Reviewer = order.PayTo;
                review.Reviewee = order.User;
            }
            else
            {
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});
            }

            _context.Reviews.Add(review);

            var addedReview = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!addedReview) throw new Exception("Problem saving changes");
            
            return Unit.Value;
        }
    }
}
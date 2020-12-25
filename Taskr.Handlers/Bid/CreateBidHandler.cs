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
using Taskr.Persistance;

namespace Taskr.Handlers.Bid
{
    public class CreateBidHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public CreateBidHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken: cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});
            
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken: cancellationToken);
            
            if (job == null)
            { 
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            }

            if (job.User == user)
                throw new RestException(HttpStatusCode.BadRequest, new {bid = "You cannot bid on your own post"});
            
            // delete any previous bid the user has on the job
            var prevBid = await _context.Bids.SingleOrDefaultAsync(x => x.User == user && x.Job == job, cancellationToken: cancellationToken);

            if (prevBid != null)
            {
                _context.Bids.Remove(prevBid);
            }

            if (request.Price >= job.InitialPrice)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                    new {price = "The price of a bid must be lower than the job price"});
            }
            
            var bid = new Domain.Bid
            {
                Status = BidStatus.Submitted,
                CreatedAt = DateTime.Now,
                JobId = request.JobId,
                Description = request.Description,
                Price = request.Price,
                UserId = user.Id,
                User = user,
                Id = request.Id
            };

            await _context.Bids.AddAsync(bid, cancellationToken);

            var created = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!created)
            {
                throw new Exception("Error occurred while creating bid");
            }

            return Unit.Value;
        }
    }
}
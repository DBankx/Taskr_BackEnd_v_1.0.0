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
    public class DeclineBidHandler : IRequestHandler<DeclineBidCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeclineBidHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(DeclineBidCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId);

            if (job == null)
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            if (job.UserId != _userAccess.GetCurrentUserId())
            {
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this action"});
            }

            var bid = await _context.Bids.SingleOrDefaultAsync(x => x.Id == request.BidId);

            if (bid == null)
                throw new RestException(HttpStatusCode.NotFound, new {error = "Bid not found"});

            bid.Status = BidStatus.Rejected;

            var success = await _context.SaveChangesAsync() > 0;
            if (success) return Unit.Value;
            throw new RestException(HttpStatusCode.InternalServerError,
                new {error = "An error occurred on the server"});
        }
    }
}
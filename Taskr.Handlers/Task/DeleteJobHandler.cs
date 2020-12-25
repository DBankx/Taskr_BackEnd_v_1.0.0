using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Task;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Task
{
    
    public class DeleteJobHandler : IRequestHandler<DeleteJobCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeleteJobHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = _userAccess.GetCurrentUserId();
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken: cancellationToken);
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            }

            if (job.UserId != loggedInUserId)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to delete this content"});
            
            _context.Jobs.Remove(job);
            
            var deleted = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!deleted)
                throw new RestException(HttpStatusCode.InternalServerError,
                    new {error = "Server error occurred"});
            return Unit.Value;
        }
    }
}
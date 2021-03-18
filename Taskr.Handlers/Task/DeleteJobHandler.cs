using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Task;
using Taskr.Domain;
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
            var job = await _context.Jobs.Include(x => x.Photos).Include(x => x.Watching).SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken: cancellationToken);
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            }

            if (job.UserId != loggedInUserId)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to delete this content"});

            if (job.JobStatus != JobStatus.Active)
                throw new RestException(HttpStatusCode.Forbidden, new {error = "This task is currently in use"});
            
            // get all chats associated with the job
            var jobChats = await _context.Chats.Include(x => x.Messages).Where(x => x.Job == job).ToListAsync(cancellationToken);
            
            // delete all orders
            
            _context.Orders.RemoveRange(await _context.Orders.Where(x => x.Job == job).ToListAsync(cancellationToken));

            _context.Chats.RemoveRange(jobChats);
            
            // foreach (var chat in jobChats)
            // { 
            //     _context.Messages.RemoveRange(chat.Messages);
            // }
            
            _context.Jobs.Remove(job);
            
            
            
            var deleted = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!deleted)
                throw new RestException(HttpStatusCode.InternalServerError,
                    new {error = "Server error occurred"});
            return Unit.Value;
        }
    }
}
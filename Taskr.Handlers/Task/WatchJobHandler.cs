using System;
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
    /// <summary>
    /// TODO - owner of job should not be able to watch their job
    /// </summary>
    public class WatchJobHandler : IRequestHandler<WatchJobCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public WatchJobHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(WatchJobCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId());

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId);

            if (job == null) throw new RestException(HttpStatusCode.NotFound, new {job = "Not found"});

            if (job.UserId == user.Id)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {watch = "You cannot watch your own task"});
            }

            var watchData = await _context.Watches.SingleOrDefaultAsync(x => x.Job == job && x.User == user);

            if (watchData != null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new {watch = "you are already watching this task"});
            }
            
            watchData =  new Watch
            {
                User = user,
                Job = job,
                WatchedAt = DateTime.Now
            };
            
            _context.Watches.Add(watchData);

            var watched = await _context.SaveChangesAsync() > 0;
            if(watched) return Unit.Value;

            throw new Exception("problem saving changes");
        }
    }
}
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
    public class UnWatchTaskHandler : IRequestHandler<UnWatchTaskCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public UnWatchTaskHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(UnWatchTaskCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = _userAccess.GetCurrentUserId();

            if (string.IsNullOrEmpty(loggedInUserId))
                throw new RestException(HttpStatusCode.BadRequest, new {errors = "You are unAuthorized"});

            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId);

            if (job == null) throw new RestException(HttpStatusCode.BadRequest, new {job = "Not found"});

            var watch = await _context.Watches.SingleOrDefaultAsync(x => x.Job == job && x.User.Id == loggedInUserId);

            if (watch == null) throw new RestException(HttpStatusCode.BadRequest, new {watch = "You are not watching task"});

            _context.Watches.Remove(watch);

            var created = await _context.SaveChangesAsync() > 0;
            
            if(created) return Unit.Value;

            throw new Exception("Problem saving changes");

        }
    }
}
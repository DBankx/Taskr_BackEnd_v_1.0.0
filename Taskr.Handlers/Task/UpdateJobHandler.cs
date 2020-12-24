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
    public class UpdateJobHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public UpdateJobHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.Id);
            
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {error = "Job not found"});
            }

            if (job.UserId != _userAccess.GetCurrentUserId())
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to modify this content"});

            job.Description = request.Description ?? job.Description;
            job.Title = request.Title ?? job.Title;
            job.InitialPrice = request.InitialPrice ?? job.InitialPrice;

            var success = await _context.SaveChangesAsync() > 0;
            if(success) return Unit.Value;
            throw new RestException(HttpStatusCode.InternalServerError, new {error = "Error occurred modifying job"});
        }
    }
}
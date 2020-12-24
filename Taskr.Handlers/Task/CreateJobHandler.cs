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
    public class CreateJobHandler : IRequestHandler<CreateJobCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public CreateJobHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
           
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId());
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var job = new Job
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                InitialPrice = request.InitialPrice,
                User = user,
                UserId = user.Id
            };
            
            await _context.Jobs.AddAsync(job, cancellationToken);
            var created = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!created)
            {
                throw new RestException(HttpStatusCode.InternalServerError,
                    new {error = "Server error occurred"});
            }

            return Unit.Value;
        }
    }
}
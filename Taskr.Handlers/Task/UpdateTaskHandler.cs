using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.Persistance;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly IJobService _jobService;
        private readonly DataContext _context;

        public UpdateTaskHandler(IJobService jobService, DataContext context)
        {
            _jobService = jobService;
            _context = context;
        }
        
        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _jobService.GetJobByIdAsync(request.Id);
            if (task.Data == null)
            {
                return false;
            }

            task.Data.Description = request.Description ?? task.Data.Description;
            task.Data.Title = request.Title ?? task.Data.Title;
            task.Data.InitialPrice = request.InitialPrice ?? task.Data.InitialPrice;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
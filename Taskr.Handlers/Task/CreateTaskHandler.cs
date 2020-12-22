using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, bool>
    {
        private readonly IJobService _jobService;

        public CreateTaskHandler(IJobService jobService)
        {
            _jobService = jobService;
        }
        
        public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Job
            {
                Title = request.Title,
                Description = request.Description,
                InitialPrice = request.InitialPrice
            };
            
            return await _jobService.CreateTaskAsync(task);
        }
    }
}
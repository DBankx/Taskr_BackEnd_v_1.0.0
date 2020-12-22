using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IJobService _jobService;

        public DeleteTaskHandler(IJobService jobService)
        {
            _jobService = jobService;
        }
        
        public Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return _jobService.DeleteTaskAsync(request.TaskId);
        }
    }
}
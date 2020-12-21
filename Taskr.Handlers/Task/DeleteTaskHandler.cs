using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskService _taskService;

        public DeleteTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        public Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return _taskService.DeleteTaskAsync(request.TaskId);
        }
    }
}
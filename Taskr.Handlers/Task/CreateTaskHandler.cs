using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, bool>
    {
        private readonly ITaskService _taskService;

        public CreateTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Task
            {
                Title = request.Title,
                Description = request.Description,
                InitialPrice = request.InitialPrice
            };
            
            return await _taskService.CreateTaskAsync(task);
        }
    }
}
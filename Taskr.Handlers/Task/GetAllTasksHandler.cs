using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Queries.Task;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, List<Domain.Task>>
    {
        private readonly ITaskService _taskService;

        public GetAllTasksHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        public async Task<List<Domain.Task>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return _taskService.GetAllTasks();
        }
    }
}
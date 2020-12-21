using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Queries;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, Domain.Task>
    {
        private readonly ITaskService _taskService;

        public GetTaskByIdHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        public async  Task<Domain.Task> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await _taskService.GetTaskByIdAsync(request.TaskId);
        }
    }
}
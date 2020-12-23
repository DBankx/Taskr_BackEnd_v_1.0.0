using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.Dtos.ApiResponse;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, ApiResponse<object>>
    {
        private readonly IJobService _jobService;

        public DeleteTaskHandler(IJobService jobService)
        {
            _jobService = jobService;
        }
        
        public Task<ApiResponse<object>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return _jobService.DeleteJobAsync(request.TaskId, request.UserId);
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Queries;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, List<Domain.Job>>
    {
        private readonly IJobService _jobService;

        public GetAllTasksHandler(IJobService jobService)
        {
            _jobService = jobService;
        }
        
        public async Task<List<Domain.Job>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await _jobService.GetAllTasksAsync();
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Queries;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, Domain.Job>
    {
        private readonly IJobService _jobService;

        public GetTaskByIdHandler(IJobService jobService)
        {
            _jobService = jobService;
        }
        
        public async  Task<Domain.Job> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await _jobService.GetTaskByIdAsync(request.TaskId);
        }
    }
}
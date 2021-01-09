using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Task;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.ExtensionMethods;
using Taskr.Infrastructure.Pagination;
using Taskr.Queries.Bid;
using Taskr.Queries.Task.Filter;

namespace Taskr.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PagedResponse<List<AllJobsDto>>> GetAllJobs([FromQuery] PaginationFilter filter, [FromQuery] GetAllJobsFilter jobsFilter, CancellationToken ct)
        {
            var query = new GetAllJobsQuery {PaginationFilter = filter, Route = Request.Path.Value, JobFilters = jobsFilter};
                var result = await _mediator.Send(query, ct);
                return result;
            }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<JobDto>> GetJobById(Guid jobId, CancellationToken ct)
        {
       
                var query = new GetJobByIdQuery(jobId);
                var result = await _mediator.Send(query, ct);
                return result;
        }

        [HttpDelete("{jobId}")]
        public async Task<ActionResult<Unit>> DeleteJob(Guid jobId, CancellationToken ct)
        {
            var command = new DeleteJobCommand(jobId);
            var result = await _mediator.Send(command, ct);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateJob([FromForm] CreateJobCommand command, CancellationToken ct)
        {
            return await _mediator.Send(command, ct);
        }

        [HttpPut("{jobId}")]
        public async Task<ActionResult<Unit>> UpdateJob(Guid jobId, [FromBody] UpdateJobCommand command, CancellationToken ct)
        {
            command.Id = jobId;
            var result = await _mediator.Send(command, ct);
            return result;
        }

        [HttpPost("watch/{jobId}")]
        public async Task<ActionResult<Unit>> WatchTask(Guid jobId)
        {
            var watchJobCommand = new WatchJobCommand {JobId = jobId};
            return await _mediator.Send(watchJobCommand);
        }
    }
}
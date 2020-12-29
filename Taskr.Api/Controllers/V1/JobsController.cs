using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Task;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.ExtensionMethods;
using Taskr.Queries.Bid;

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
        [EnableQuery(PageSize = 10)]
        public async Task<IQueryable<AllJobsDto>> GetAllJobs(CancellationToken ct)
        {
            await Task.Delay(1000, cancellationToken: ct);
                var query = new GetAllJobsQuery();
                var result = await _mediator.Send(query, ct);
                return result;
            }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<Job>> GetJobsById(Guid jobId, CancellationToken ct)
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
        public async Task<ActionResult<Unit>> CreateJob([FromBody] CreateJobCommand command, CancellationToken ct)
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
        
    }
}
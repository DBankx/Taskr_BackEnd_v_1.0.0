using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Task;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;
using Taskr.Infrastructure.ExtensionMethods;
using Taskr.Queries.Bid;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
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
        public async Task<ActionResult<List<Job>>> GetAllTasks()
        {
            var query = new GetAllJobsQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<Job>> GetTaskById(Guid jobId)
        {
            var query = new GetJobByIdQuery(jobId);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpDelete("{jobId}")]
        public async Task<ActionResult<Unit>> DeleteTask(Guid jobId)
        {
            var command = new DeleteJobCommand(jobId);
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateTask([FromBody] CreateJobCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{jobId}")]
        public async Task<ActionResult<Unit>> UpdateTask(Guid jobId, [FromBody] UpdateJobCommand command)
        {
            command.Id = jobId;
            var result = await _mediator.Send(command);
            return result;
        }
        
    }
}
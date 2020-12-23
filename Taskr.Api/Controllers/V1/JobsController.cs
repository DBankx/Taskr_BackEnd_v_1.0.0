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
using Taskr.Queries;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJobService _jobService;

        public JobsController(IMediator mediator, IJobService jobService)
        {
            _mediator = mediator;
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var query = new GetAllTasksQuery();
            var result = await _mediator.Send(query);
            return Ok(new ApiSuccessResponse<List<Job>>(result.Data));
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetTaskById(Guid jobId)
        {
            var query = new GetTaskByIdQuery(jobId);
            var result = await _mediator.Send(query);
            if (result.Success == false)
            {
                return NotFound(new ApiErrorResponse(result.Errors));
            }
            return Ok(new ApiSuccessResponse<Job>(result.Data));
        }

        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteTask(Guid jobId)
        {
            var command = new DeleteTaskCommand(jobId, HttpContext.GetCurrentUserId());
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new ApiErrorResponse(result.Errors));
            }

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new ApiErrorResponse(result.Errors));
            }

            return Ok(new ApiSuccessResponse<object>(result.Data));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            command.UserId = HttpContext.GetCurrentUserId();
            var result = await _mediator.Send(command);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new ApiErrorResponse(result.Errors));
            }

            if (result.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode((int) HttpStatusCode.InternalServerError, new ApiErrorResponse(result.Errors));

            return CreatedAtAction("GetTaskById", new {jobId = command.Id},  new ApiSuccessResponse<object>(result.Data));
        }

        [HttpPut("{jobId}")]
        public async Task<IActionResult> UpdateTask(Guid jobId, [FromBody] UpdateTaskCommand command)
        {
            command.Id = jobId;
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound(new {error = "Error occurred while updating task"});
            }

            return Ok(command);
        }
        
    }
}
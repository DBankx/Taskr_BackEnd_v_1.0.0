using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Task;
using Taskr.Queries;
using Task = Taskr.Domain.Task;

namespace Taskr.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var query = new GetAllTasksQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var query = new GetTaskByIdQuery(taskId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(new {error = "Resource not found"});
            }
            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var command = new DeleteTaskCommand(taskId);
            var result = await _mediator.Send(command);
            if (result == false)
            {
                return NotFound(new {error = "Resource not found"});
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest(new {error = "Error occurred during creation of task"});
            }

            return CreatedAtAction("GetTaskById", new {taskId = command.Id},  new {message = "Task created successfully"});
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            command.Id = taskId;
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound(new {error = "Error occurred while updating task"});
            }

            return Ok(command);
        }
    }
}
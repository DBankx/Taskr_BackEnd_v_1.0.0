using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskr.Queries.Task;

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
    }
}
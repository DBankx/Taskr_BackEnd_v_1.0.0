using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Dtos.Job;
using Taskr.Queries.Profile;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("jobs")]
        public async Task<ActionResult<List<JobsListDto>>> GetProfileTasks([FromQuery] string status)
        {
            var query = new GetProfileTasksQuery(status);
            return await _mediator.Send(query);
        }
        
        
    }
}
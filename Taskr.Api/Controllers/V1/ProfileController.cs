using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Profile;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Dtos.Profile;
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
        public async Task<ActionResult<List<JobsListDto>>> GetProfileTasks([FromQuery] JobStatus status)
        {
            var query = new GetProfileJobsQuery(status);
            return await _mediator.Send(query);
        }
        
        [HttpGet]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var profileQuery = new GetProfileQuery();
            return await _mediator.Send(profileQuery);
        }

        [HttpPost("skills")]
        public async Task<ActionResult<Unit>> AddProfileSkills(AddSkillsCommand command)
        {
            return await _mediator.Send(command);
        }
        
         [HttpPost("languages")]
         public async Task<ActionResult<Unit>> AddProfileLanguages(AddLanguageCommand command)
         { 
             return await _mediator.Send(command);
         }

         [HttpPost("update")]
         public async Task<ActionResult<Unit>> UpdateProfile(UpdateProfileCommand command)
         {
             return await _mediator.Send(command);
         }
    }
}
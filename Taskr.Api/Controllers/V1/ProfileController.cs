﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Profile;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.Pagination;
using Taskr.Infrastructure.UserNotification;
using Taskr.Queries.Profile;
using Taskr.Queries.UserNotifications;

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
        public async Task<ActionResult<ProfileDto>> GetProfile(CancellationToken ct)
        {
            var profileQuery = new GetProfileQuery();
            return await _mediator.Send(profileQuery, ct);
        }

        [HttpPost("skills")]
        public async Task<ActionResult<Unit>> AddProfileSkills(AddSkillsCommand command, CancellationToken ct)
        {
            return await _mediator.Send(command, ct);
        }
        
         [HttpPost("languages")]
         public async Task<ActionResult<Unit>> AddProfileLanguages(AddLanguageCommand command, CancellationToken ct)
         { 
             return await _mediator.Send(command, ct);
         }

         [HttpPost("update")]
         public async Task<ActionResult<Unit>> UpdateProfile(UpdateProfileCommand command, CancellationToken ct)
         {
             return await _mediator.Send(command, ct);
         }

         [HttpPost("socials")]
         public async Task<ActionResult<Unit>> UpdateSocials(UpdateSocialCommand command, CancellationToken ct)
         {
             return await _mediator.Send(command, ct);
         }

         [HttpGet("notifications")]
         public async Task<ActionResult<PagedResponse<List<UserPrivateMessageNotification>>>> GetNotifications(
             [FromQuery] NotificationStatus status, [FromQuery] PaginationFilter filter, CancellationToken ct)
         {
             var query = new GetNotificationsQuery(status, filter, Request.Path.Value);
             return await _mediator.Send(query, ct);
         }
    }
}
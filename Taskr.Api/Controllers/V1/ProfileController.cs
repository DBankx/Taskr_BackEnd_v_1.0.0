using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Profile;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Dtos.Profile;
using Taskr.Dtos.Review;
using Taskr.Infrastructure.Pagination;
using Taskr.Queries.Profile;
using Taskr.Queries.PublicProfile;
using Taskr.Queries.Review;
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
         public async Task<ActionResult<PagedResponse<List<UserNotificationDto>>>> GetNotifications(
             [FromQuery] NotificationStatus status, [FromQuery] PaginationFilter filter, CancellationToken ct)
         {
             var query = new GetNotificationsQuery(status, filter, Request.Path.Value);
             return await _mediator.Send(query, ct);
         }

         [HttpDelete("notifications/{notificationId}")]
         public async Task<ActionResult<Unit>> DeleteUserNotification(Guid notificationId, CancellationToken ct)
         {
             var command = new DeleteNotificationCommand(notificationId);
             return await _mediator.Send(command, ct);
         }
         
         [HttpPut("notifications/{notificationId}")]
         public async Task<ActionResult<Unit>> MarkNotificationAsRead(Guid notificationId, CancellationToken ct)
         {
             var command = new MarkNotificationAsReadCommand(notificationId);
             return await _mediator.Send(command, ct);
         } 
         
         [HttpDelete("notifications")]
         public async Task<ActionResult<Unit>> DeleteAllNotifications(CancellationToken ct)
         { 
             var command = new DeleteAllNotifications(); 
             return await _mediator.Send(command, ct);
         }

         [HttpPut("notifications/read")]
         public async Task<ActionResult<Unit>> MarkAllNotificationsAsRead(CancellationToken ct)
         {
             var command = new MarkAllNotificationsAsRead();
             return await _mediator.Send(command, ct);
         }

         [HttpGet("watchlist")]
         public async Task<ActionResult<List<JobsListDto>>> GetUserWatchlist([FromQuery] string sortBy, CancellationToken ct)
         {
             var query = new GetUserWatchlist(sortBy);
             return await _mediator.Send(query, ct);
         }

         [AllowAnonymous]
         [HttpGet("public/details/{userId}")]
         public async Task<ActionResult<PublicProfileDto>> GetPublicProfileDetails(string userId, CancellationToken ct)
         {
             var query = new GetPublicProfileDetails(userId);
             return await _mediator.Send(query, ct);
         }

         [AllowAnonymous]
         [HttpGet("public/tasks/{userId}")]
         public async Task<ActionResult<List<JobsListDto>>> GetRecentlyUploadedTasks(string userId, CancellationToken ct)
         {
             var query = new GetRecentlyUploadedJobs(userId);
             return await _mediator.Send(query, ct);
         }

         [HttpPost("bank")]
         public async Task<ActionResult<Unit>> CreateBankAccount(AddBankAccount command, CancellationToken ct)
         {
             return await _mediator.Send(command, ct);
         }

         [AllowAnonymous]
         [HttpGet("reviews/{userId}")]
         public async Task<ActionResult<ReturnReviewsDto>> GetUserReviews([FromQuery] string predicate, string userId)
         {
             return await _mediator.Send(new GetUserReview(userId, predicate));
         }
    }
}
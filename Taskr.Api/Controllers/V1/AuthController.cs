using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Auth;
using Taskr.Dtos.Auth;
using Taskr.Queries.Auth;

namespace Taskr.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<Unit>> SignUp([FromBody] SignUpCommand command)
        { 
            return await _mediator.Send(command);
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<Unit>> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            return await _mediator.Send(new ConfirmEmail(userId, code));
        }

        [HttpPost("signIn")]
        public async Task<ActionResult> SignIn([FromBody] SignInCommand command)
        {
            var authResponse = await _mediator.Send(command);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthErrorResponse
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                User = authResponse.User
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentUser(CancellationToken ct)
        {
            var authQuery = new GetCurrentUserQuery();
            var authResponse = await _mediator.Send(authQuery, ct);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthErrorResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                User = authResponse.User
            });
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<Unit>> ChangePassword(ChangePassword command)
        {
            return await _mediator.Send(command);
        }   
    }
}
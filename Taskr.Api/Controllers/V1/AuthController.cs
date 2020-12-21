using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Auth;
using Taskr.Dtos.Auth;

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
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
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

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
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
    }
}
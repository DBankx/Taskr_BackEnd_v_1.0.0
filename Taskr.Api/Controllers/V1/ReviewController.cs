using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Review;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{orderNumber}")]
        public async Task<ActionResult<Unit>> AddReview(AddReview command, string orderNumber, CancellationToken ct)
        {
            command.OrderNumber = orderNumber;
            return await _mediator.Send(command, ct);
        }
    }
}
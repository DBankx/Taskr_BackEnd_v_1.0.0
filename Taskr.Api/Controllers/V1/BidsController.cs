using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Bid;
using Taskr.Domain;
using Taskr.Dtos;
using Taskr.Dtos.Bid;
using Taskr.Dtos.Order;
using Taskr.Infrastructure.ExtensionMethods;
using Taskr.Queries.Bid;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BidsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{jobId}")]
        public async Task<ActionResult<BidDto>> CreateBid(Guid jobId, [FromBody] CreateBidCommand command, CancellationToken cancellationToken)
        {
            command.JobId = jobId;
            var result = await _mediator.Send(command, cancellationToken);
            return result;
        }

        [HttpGet("{jobId}")]
        [AllowAnonymous]
        public async Task<List<TaskBidDto>> GetAllJobBids(Guid jobId, CancellationToken ct)
        {
            var query = new GetAllJobBidsQuery(jobId);
            var result = await _mediator.Send(query, ct);
            return result;
        }

        [HttpGet("get-bid/{bidId}")]
        public async Task<ActionResult<SingleBidDto>> GetBidById(Guid bidId, CancellationToken ct)
        {
            var query = new GetBidByIdQuery(bidId);
            return await _mediator.Send(query, ct);
        }

        [HttpPut("seen/{bidId}")]
        public async Task<ActionResult<Unit>> MarkBidAsSeen(Guid bidId)
        {
            var markBidAsSeenCommand = new MarkBidAsSeen(bidId);
            return await _mediator.Send(markBidAsSeenCommand);
        }

        [HttpPost("accept/{jobId}/{bidId}")]
        public async Task<ActionResult<OrderDetailsDto>> AcceptBid(Guid jobId, Guid bidId, CancellationToken ct)
        {
            var acceptBidCommand = new AcceptBidCommand(bidId, jobId);
            return await _mediator.Send(acceptBidCommand, ct);
        }
    }
}
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
        public async Task<IQueryable<Bid>> GetAllJobBids(Guid jobId, CancellationToken ct)
        {
            var query = new GetAllJobBidsQuery(jobId);
            var result = await _mediator.Send(query, ct);
            return result;
        }

        [HttpGet("get-bid/{bidId}")]
        public async Task<ActionResult<Bid>> GetBidById(Guid bidId, CancellationToken ct)
        {
            var query = new GetBidByIdQuery(bidId);
            return await _mediator.Send(query, ct);
        }

        [HttpPost("decline/{jobId}/{bidId}")]
        public async Task<ActionResult<Unit>> DeclineBid(Guid jobId, Guid bidId, CancellationToken ct)
        {
            var command = new DeclineBidCommand(bidId, jobId);
            return await _mediator.Send(command, ct);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Order;
using Taskr.Dtos.Order;
using Taskr.Queries.Order;

namespace Taskr.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{orderNumber}")]
        public async Task<ActionResult<Unit>> DeleteOrder(string orderNumber, CancellationToken ct)
        {
            var command = new DeleteOrderCommand(orderNumber);
            return await _mediator.Send(command, ct);
        }

        [HttpPut("confirm/{orderNumber}")]
        public async Task<ActionResult<Unit>> ConfirmOrder(string orderNumber, CancellationToken ct)
        {
            var command = new ConfirmOrderCommand(orderNumber);
            return await _mediator.Send(command, ct);
        }

        [HttpGet]
        public async Task<ActionResult<List<ListOrderDto>>> GetAllOrders([FromQuery] string predicate, CancellationToken ct)
        {
            var query = new GetAllOrders(predicate);
            return await _mediator.Send(query, ct);
        }

        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByNumber(string orderNumber, CancellationToken ct)
        {
            return await _mediator.Send(new GetOrderByNumber(orderNumber), ct);
        }

        [HttpPut("start/{orderNumber}")]
        public async Task<ActionResult<Unit>> MarkOrderAsStarted(string orderNumber, CancellationToken ct)
        {
            return await _mediator.Send(new MarkOrderAsStarted(orderNumber), ct);
        }

        [HttpPut("request-payout/{orderNumber}")]
        public async Task<ActionResult<Unit>> RequestPayout(string orderNumber, CancellationToken ct)
        {
            return await _mediator.Send(new RequestPayout(orderNumber), ct);
        }
        
        [HttpPut("reject-payout/{orderNumber}")]
                public async Task<ActionResult<Unit>> RejectPayoutRequest (string orderNumber, CancellationToken ct)
                {
                    return await _mediator.Send(new RejectPayout(orderNumber), ct);
                }
        
        
    }
}
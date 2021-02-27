using System;
using MediatR;
using Taskr.Dtos.Order;

namespace Taskr.Commands.Bid
{
    public class AcceptBidCommand : IRequest, IRequest<OrderDetailsDto>
    {
        public Guid BidId { get; set; }
        public Guid JobId { get; set; }

        public AcceptBidCommand(Guid bidId, Guid jobId)
        {
            BidId = bidId;
            JobId = jobId;
        }
    }
}
using System;
using MediatR;

namespace Taskr.Commands.Bid
{
    public class DeclineBidCommand : IRequest
    {
        public Guid BidId { get; set; }
        public Guid JobId { get; set; }

        public DeclineBidCommand(Guid bidId, Guid jobId)
        {
            BidId = bidId;
            JobId = jobId;
        }
    }
}
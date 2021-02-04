using System;
using MediatR;

namespace Taskr.Commands.Bid
{
    public class MarkBidAsSeen : IRequest
    {
        public Guid BidId { get; set; }

        public MarkBidAsSeen(Guid bidId)
        {
            BidId = bidId;
        }
    }
}
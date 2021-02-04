using System;
using Taskr.Domain;

namespace Taskr.Dtos.Bid
{
    public class SingleBidDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public BidStatus Status { get; set; }
        public BidCreatorDto BidCreator { get; set; }
    }
}
using System;
using Taskr.Domain;

namespace Taskr.Dtos.Bid
{
    public class TaskBidDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public BidStatus Status { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}
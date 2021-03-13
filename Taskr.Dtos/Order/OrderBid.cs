using System;
using Taskr.Domain;

namespace Taskr.Dtos.Order
{
    public class OrderBid
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public BidStatus Status { get; set; }
    }
}
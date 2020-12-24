using System;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Commands.Bid
{
    /// <summary>
    /// TODO - create validation
    /// </summary>
    public class CreateBidCommand : IRequest
    {
        public Guid JobId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public BidStatus Status { get; set; }
    }
}
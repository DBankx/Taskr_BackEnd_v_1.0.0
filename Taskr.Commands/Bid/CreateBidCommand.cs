using System;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos;

namespace Taskr.Commands.Bid
{
    
    public class CreateBidCommand : IRequest<BidDto>
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
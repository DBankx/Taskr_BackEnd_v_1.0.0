using System;
using MediatR;
using Taskr.Domain;

namespace Taskr.Commands.Bid
{
    /// <summary>
    /// TODO - create validation
    /// </summary>
    public class CreateBidCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
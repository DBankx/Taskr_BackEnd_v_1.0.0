using System;
using MediatR;
using Taskr.Dtos.Bid;

namespace Taskr.Queries.Bid
{
    public class GetBidByIdQuery : IRequest<SingleBidDto>
    {
        public Guid Id { get; set; }

        public GetBidByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
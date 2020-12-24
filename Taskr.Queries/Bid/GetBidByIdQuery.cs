using System;
using MediatR;

namespace Taskr.Queries.Bid
{
    public class GetBidByIdQuery : IRequest<Domain.Bid>
    {
        public Guid Id { get; set; }

        public GetBidByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
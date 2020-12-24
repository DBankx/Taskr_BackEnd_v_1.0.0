using System;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries.Bid
{
    public class GetJobByIdQuery : IRequest<Job>
    {
        public Guid Id { get; set; }

        public GetJobByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
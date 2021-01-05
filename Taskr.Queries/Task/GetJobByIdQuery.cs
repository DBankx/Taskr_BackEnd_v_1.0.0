using System;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.Queries.Bid
{
    public class GetJobByIdQuery : IRequest<JobDto>
    {
        public Guid Id { get; set; }

        public GetJobByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
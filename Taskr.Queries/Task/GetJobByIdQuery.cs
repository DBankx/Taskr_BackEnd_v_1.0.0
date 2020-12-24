using System;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

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
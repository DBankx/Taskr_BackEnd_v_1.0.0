using System;
using System.Collections.Generic;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Queries.Bid
{
    public class GetAllJobBidsQuery : IRequest<List<Domain.Bid>>
    {
        public Guid JobId { get; set; }

        public GetAllJobBidsQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Bid;

namespace Taskr.Queries.Bid
{
    public class GetAllJobBidsQuery : IRequest<List<TaskBidDto>>
    {
        public Guid JobId { get; set; }

        public GetAllJobBidsQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
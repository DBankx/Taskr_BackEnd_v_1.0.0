using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries.Bid
{
    public class GetAllJobBidsQuery : IRequest<IQueryable<Domain.Bid>>
    {
        public Guid JobId { get; set; }

        public GetAllJobBidsQuery(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
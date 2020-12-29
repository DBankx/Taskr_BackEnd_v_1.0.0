using System.Collections.Generic;
using System.Linq;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.Queries.Bid
{
    public class GetAllJobsQuery : IRequest<IQueryable<AllJobsDto>>
    {
        
    }
}
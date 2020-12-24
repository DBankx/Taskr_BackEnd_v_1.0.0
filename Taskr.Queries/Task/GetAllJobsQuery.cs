using System.Collections.Generic;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries.Bid
{
    public class GetAllJobsQuery : IRequest<List<Job>>
    {
        
    }
}
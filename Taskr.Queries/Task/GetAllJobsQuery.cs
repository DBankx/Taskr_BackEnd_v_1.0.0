using System.Collections.Generic;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Queries.Bid
{
    public class GetAllJobsQuery : IRequest<List<Job>>
    {
        
    }
}
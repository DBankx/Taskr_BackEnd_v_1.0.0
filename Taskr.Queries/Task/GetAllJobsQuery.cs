using System.Collections.Generic;
using System.Linq;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Pagination;
using Taskr.Queries.Task.Filter;

namespace Taskr.Queries.Bid
{
    public class GetAllJobsQuery : IRequest<PagedResponse<List<AllJobsDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
        public GetAllJobsFilter JobFilters { get; set; }
    }
}
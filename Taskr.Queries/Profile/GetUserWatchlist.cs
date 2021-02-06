using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Job;

namespace Taskr.Queries.Profile
{
    public class GetUserWatchlist : IRequest<List<JobsListDto>>
    {
        public string SortBy { get; set; }

        public GetUserWatchlist(string sortBy)
        {
            SortBy = sortBy;
        }
    }
}
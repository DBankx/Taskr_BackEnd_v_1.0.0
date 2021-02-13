using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Job;

namespace Taskr.Queries.PublicProfile
{
    public class GetRecentlyUploadedJobs : IRequest<List<JobsListDto>>
    {
        public string UserId { get; set; }

        public GetRecentlyUploadedJobs(string userId)
        {
            UserId = userId;
        }
    }
}
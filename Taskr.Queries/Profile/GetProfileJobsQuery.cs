using System.Collections.Generic;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.Queries.Profile
{
    public class GetProfileJobsQuery : IRequest<List<JobsListDto>>
    {
        public JobStatus JobStatus { get; set; }

        public GetProfileJobsQuery(JobStatus jobStatus)
        {
            JobStatus = jobStatus;
        }
    }
}
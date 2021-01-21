using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Job;

namespace Taskr.Queries.Profile
{
    public class GetProfileTasksQuery : IRequest<List<JobsListDto>>
    {
        public string Predicate { get; set; }

        public GetProfileTasksQuery(string predicate)
        {
            Predicate = predicate;
        }
    }
}
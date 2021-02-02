using System;
using System.Linq;
using Taskr.Domain;
using Taskr.Queries.Task.Filter;

namespace Taskr.Queries.Filter
{
    public static class AddFilterToQueries
    {
        public static IQueryable<Job> FilterJobs(GetAllJobsFilter filter, IQueryable<Job> queryable)
        {
            // filter for title
            if (!string.IsNullOrEmpty(filter?.Title))
            {
                queryable = queryable.Where(x => x.Title.Contains(filter.Title));
            }

            // filter for pricing
            if (filter.MinPrice >= 0 && filter.MaxPrice > 0)
            {
                queryable = queryable.Where(x =>
                    x.InitialPrice >= filter.MinPrice && x.InitialPrice <= filter.MaxPrice);
            } 
            
            return queryable;
        }
        
    }
}
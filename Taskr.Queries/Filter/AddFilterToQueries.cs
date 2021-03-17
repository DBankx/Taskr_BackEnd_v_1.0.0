using System;
using System.Collections.Generic;
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

            if (filter.Category.HasValue)
            {
                queryable = queryable.Where(x => x.Category == filter.Category);
            }

            if (filter.DeliveryType.HasValue)
            {
                queryable = queryable.Where(x => x.DeliveryType == filter.DeliveryType);
            }
            

            switch (filter.SortBy)
            {
                case "OLDEST":
                    queryable = queryable.OrderBy(x => x.CreatedAt);
                    break;
                case "LOWEST":
                    queryable = queryable.OrderBy(x => x.InitialPrice);
                    break;
                case "HIGHEST":
                    queryable = queryable.OrderByDescending(x => x.InitialPrice);
                    break;
                default:
                    queryable = queryable.OrderByDescending(x => x.CreatedAt);
                    break;
            }
            
            return queryable.Where(x => x.JobStatus == JobStatus.Active);
        }

        public static List<Job> FilterWatchlist(string sortBy, List<Job> queryable)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "LOWEST_PRICE":
                        queryable = queryable.OrderBy(x => x.InitialPrice).ToList();
                        break;
                    case "HIGHEST_PRICE":
                        queryable = queryable.OrderByDescending(x => x.InitialPrice).ToList();
                        break;
                    case "TITLE_A_TO_Z":
                        queryable = queryable.OrderBy(x => x.Title).ToList();
                        break;
                    case "TITLE_Z_TO_A":
                        queryable = queryable.OrderByDescending(x => x.Title).ToList();
                        break;
                    case "NEWEST":
                        queryable = queryable.OrderByDescending(x => x.CreatedAt).ToList();
                        break;
                    case "OLDEST":
                        queryable = queryable.OrderBy(x => x.CreatedAt).ToList();
                        break;
                    default:
                        return queryable;
                }
            }
            return queryable;
        }

        public static List<Domain.Order> FilterOrders(string predicate, List<Domain.Order> queryable, string userId) 
        {
            if (!string.IsNullOrEmpty(predicate))
            {
                switch (predicate)
                {
                    case "ACTIVE":
                        queryable = queryable.Where(x => x.Status == OrderStatus.Confirmed && x.User.Id == userId || x.Status == OrderStatus.Started && x.User.Id == userId).ToList();
                        break;
                    case "RUNNER":
                        queryable = queryable.Where(x => x.Status == OrderStatus.Confirmed && x.PayTo.Id == userId || x.Status == OrderStatus.Started && x.PayTo.Id == userId).ToList();
                        break;
                    case "COMPLETED":
                        queryable = queryable.Where(x => x.Status == OrderStatus.Completed && x.User.Id == userId || x.Status == OrderStatus.Completed && x.PayTo.Id == userId).ToList();
                        break;
                    case "PAYOUT": 
                        queryable = queryable.Where(x => x.Status == OrderStatus.AwaitingPayout && x.PayTo.Id == userId || x.Status == OrderStatus.AwaitingPayout && x.PayTo.Id == userId).ToList();
                        break;
                    case "CANCELLED": 
                        queryable = queryable.Where(x => x.Status == OrderStatus.Cancelled && x.User.Id == userId || x.Status == OrderStatus.Cancelled && x.PayTo.Id == userId).ToList();
                        break;
                    default:
                        return queryable;
                }
            }

            return queryable;
        }
    }
}
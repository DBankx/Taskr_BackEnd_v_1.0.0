using System.Collections.Generic;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.Pagination;

namespace Taskr.Queries.UserNotifications
{
    public class GetNotificationsQuery : IRequest<PagedResponse<List<UserNotificationDto>>>
    {
        public NotificationStatus Status { get; set; }
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }

        public GetNotificationsQuery(NotificationStatus status, PaginationFilter paginationFilter, string route)
        {
            Status = status;
            PaginationFilter = paginationFilter;
            Route = route;
        }
    }
}
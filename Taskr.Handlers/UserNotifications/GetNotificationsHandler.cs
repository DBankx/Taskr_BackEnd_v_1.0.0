using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Job;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Pagination;
using Taskr.Infrastructure.Security;
using Taskr.Infrastructure.UserNotification;
using Taskr.Queries.Filter;
using Taskr.Queries.UserNotifications;

namespace Taskr.Handlers.UserNotifications
{
    public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, PagedResponse<List<UserNotificationDto>>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public GetNotificationsHandler(IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper, IUriService uriService)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
            _uriService = uriService;
        }
        
        public async Task<PagedResponse<List<UserNotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var notifs = _queryProcessor.Query<AppUserNotification>()
                .Where(x => x.ToUserId == user.Id).OrderByDescending(x => x.CreatedAt);
            
            // paginating the data
            var validFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);
            var pagedData = await notifs.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync(cancellationToken: cancellationToken);
            var totalRecords = await notifs.CountAsync(cancellationToken: cancellationToken);
            var pagedResponse = PaginationHelper.CreatePagedReponse<UserNotificationDto>(_mapper.Map<List<UserNotificationDto>>(pagedData), validFilter,
                totalRecords, _uriService, request.Route);
            return pagedResponse;
        }
    }
}
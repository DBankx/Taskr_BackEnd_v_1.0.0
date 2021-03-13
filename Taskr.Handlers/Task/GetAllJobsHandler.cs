using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Pagination;
using Taskr.Persistance;
using Taskr.Queries.Bid;
using Taskr.Queries.Filter;

namespace Taskr.Handlers.Task
{
    public class GetAllJobsHandler : IRequestHandler<GetAllJobsQuery, PagedResponse<List<JobsListDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IQueryProcessor _queryProcessor;

        public GetAllJobsHandler(IMapper mapper, IUriService uriService, IQueryProcessor queryProcessor)
        {
            _mapper = mapper;
            _uriService = uriService;
            _queryProcessor = queryProcessor;
        }
        
        
        public async Task<PagedResponse<List<JobsListDto>>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            if (request.JobFilters != null && !request.JobFilters.ValidPriceRange)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                    new {error = "Max price cannot be less than min price"});
            }

            var queryable = _queryProcessor.Query<Job>()
                .Include(x => x.User)
                .Include(x => x.Photos)
                .Include(x => x.Watching).AsQueryable();
            
            queryable = AddFilterToQueries.FilterJobs(request.JobFilters, queryable);
            
            // paginating the data
            var validFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);
            var pagedData = await queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync(cancellationToken: cancellationToken);
            var totalRecords = await queryable.CountAsync(cancellationToken: cancellationToken);
            var pagedResponse = PaginationHelper.CreatePagedReponse<JobsListDto>(_mapper.Map<List<JobsListDto>>(pagedData), validFilter,
                totalRecords, _uriService, request.Route);
            return pagedResponse;
        }
    }
}
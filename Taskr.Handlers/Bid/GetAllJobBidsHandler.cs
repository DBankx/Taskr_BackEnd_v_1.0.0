using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Bid;
using Taskr.Infrastructure.Helpers;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Bid
{
    public class GetAllJobBidsHandler : IRequestHandler<GetAllJobBidsQuery, List<TaskBidDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;

        public GetAllJobBidsHandler(DataContext context, IMapper mapper, IQueryProcessor queryProcessor)
        {
            _context = context;
            _mapper = mapper;
            _queryProcessor = queryProcessor;
        }
        
        public async Task<List<TaskBidDto>> Handle(GetAllJobBidsQuery request, CancellationToken cancellationToken)
        {

            var bids = await _queryProcessor.Query<Domain.Bid>()
                .Include(x => x.Job)
                .Include(x => x.User)
                .Where(x => x.JobId == request.JobId).ToListAsync(cancellationToken: cancellationToken); 

            return _mapper.Map<List<TaskBidDto>>(bids);
        }
    }
}
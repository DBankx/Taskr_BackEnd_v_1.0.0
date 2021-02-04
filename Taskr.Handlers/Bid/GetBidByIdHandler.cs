using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Bid;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Helpers;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Bid
{
    public class GetBidByIdHandler : IRequestHandler<GetBidByIdQuery, SingleBidDto>
    {
        private readonly DataContext _context;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;

        public GetBidByIdHandler(DataContext context, IQueryProcessor queryProcessor, IMapper mapper)
        {
            _context = context;
            _queryProcessor = queryProcessor;
            _mapper = mapper;
        }
        
        public async Task<SingleBidDto> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            var bid = await _queryProcessor.Query<Domain.Bid>()
                .Include(x => x.Job)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            
            if (bid == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Bid not found"});

            return _mapper.Map<SingleBidDto>(bid);
        }
    }
}
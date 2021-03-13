using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos;
using Taskr.Dtos.Bid;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;

namespace Taskr.MappingProfiles.Job
{
    public class AcceptedBidResolver : IValueResolver<Domain.Job, JobDto, BidDto>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;

        public AcceptedBidResolver(IQueryProcessor queryProcessor, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
        }
        
        public BidDto Resolve(Domain.Job source, JobDto destination, BidDto destMember, ResolutionContext context)
        {
            var acceptedBid = _queryProcessor.Query<Domain.Bid>()
                .SingleOrDefaultAsync(x => x.Job == source && x.Status == BidStatus.Approved).Result;

            return _mapper.Map<BidDto>(acceptedBid);
        }
    }
}
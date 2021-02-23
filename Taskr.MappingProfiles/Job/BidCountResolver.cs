using System.Threading.Tasks;
using AutoMapper;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Persistance;

namespace Taskr.MappingProfiles.Job
{
    public class BidCountResolver : IValueResolver<Domain.Job, JobDto, int>
    {
        public BidCountResolver()
        {
            
        }
        
        public int Resolve(Domain.Job source, JobDto destination, int destMember, ResolutionContext context)
        {
            // get all the bids that has the jobId
            var bidCount = source.Bids.Count;
            return bidCount;
        }
    }
}
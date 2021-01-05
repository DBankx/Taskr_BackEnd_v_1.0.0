using System.Threading.Tasks;
using AutoMapper;
using Taskr.Dtos.Job;
using Taskr.Persistance;

namespace Taskr.MappingProfiles.Job
{
    public class WatchCountResolver : IValueResolver<Domain.Job, JobDto, int>
    {
        private readonly DataContext _context;
       
               public WatchCountResolver(DataContext context)
               {
                   _context = context;
               }
               
               public int Resolve(Domain.Job source, JobDto destination, int destMember, ResolutionContext context)
               {
                   // get all the bids that has the jobId
                   var watchCount = source.Watching.Count;
                   return watchCount;
               } 
    }
}

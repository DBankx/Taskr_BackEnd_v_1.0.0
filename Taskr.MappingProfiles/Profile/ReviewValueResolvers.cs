using System.Linq;
using AutoMapper;
using Taskr.Domain;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.Helpers;

namespace Taskr.MappingProfiles.Profile
{
    public class AvgRatingResolver : IValueResolver<ApplicationUser, PublicProfileDto, double>
    {
        private readonly IQueryProcessor _queryProcessor;

        public AvgRatingResolver(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }
        
        public double Resolve(ApplicationUser source, PublicProfileDto destination, double destMember, ResolutionContext context)
        {
            var reviews = _queryProcessor.Query<Domain.Review>().Where(x => x.Reviewee == source);
            return reviews.Any() ? reviews.Average(x => x.Rating) : 0;
        }
    }
    
    public class ReviewCountResolver : IValueResolver<ApplicationUser, PublicProfileDto, int>
    { 
        private readonly IQueryProcessor _queryProcessor;
    
        public ReviewCountResolver(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }
            
        public int Resolve(ApplicationUser source, PublicProfileDto destination, int destMember, ResolutionContext context)
            {
                return _queryProcessor.Query<Domain.Review>().Where(x => x.Reviewee == source).Count();
            }
        }
}
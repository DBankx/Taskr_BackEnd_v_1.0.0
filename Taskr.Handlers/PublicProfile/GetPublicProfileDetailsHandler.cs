using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.Helpers;
using Taskr.Queries.PublicProfile;

namespace Taskr.Handlers.PublicProfile
{
    public class GetPublicProfileDetailsHandler : IRequestHandler<GetPublicProfileDetails, PublicProfileDto>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;

        public GetPublicProfileDetailsHandler(IQueryProcessor queryProcessor, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
        }
        
        public async Task<PublicProfileDto> Handle(GetPublicProfileDetails request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null) throw new RestException(HttpStatusCode.NotFound, new {user = "Not found"});

            return _mapper.Map<PublicProfileDto>(user);
        }
    }
}
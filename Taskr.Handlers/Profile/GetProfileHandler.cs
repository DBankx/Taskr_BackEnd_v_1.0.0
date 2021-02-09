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
using Taskr.Infrastructure.Security;
using Taskr.Queries.Profile;

namespace Taskr.Handlers.Profile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;
        private readonly IUserAccess _userAccess;

        public GetProfileHandler(IQueryProcessor queryProcessor, IMapper mapper, IUserAccess userAccess)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
            _userAccess = userAccess;
        }
        
        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken: cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are un authorized"});

            var userJobs = await _queryProcessor.Query<Job>().Include(x => x.Bids)
                .Include(x => x.Photos)
                .Include(x => x.User)
                .Include(x => x.Watching)
                .ThenInclude(x => x.User)
                .Where(x => x.User == user)
                .ToListAsync(cancellationToken);

            var profile = new ProfileDto
            {
                Avatar = user.Avatar,
                Bio = user.Bio,
                City = user.City,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                SkillSet = user.SkillSet,
                Languages = user.Languages,
                Socials = user.Socials,
                Tagline = user.Tagline
            };

            return profile;

        }
    }
}
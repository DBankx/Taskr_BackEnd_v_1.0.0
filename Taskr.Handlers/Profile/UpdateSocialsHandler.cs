using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Profile;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Profile
{
    public class UpdateSocialsHandler : IRequestHandler<UpdateSocialCommand>
    {
        private readonly IUserAccess _userAccess;
        private readonly DataContext _context;

        public UpdateSocialsHandler(IUserAccess userAccess, DataContext context)
        {
            _userAccess = userAccess;
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateSocialCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {errors = "You are unauthorized"});

            user.Socials.Facebook = request.Facebook ?? user.Socials.Facebook;
            user.Socials.Instagram = request.Instagram ?? user.Socials.Instagram;
            user.Socials.Pinterest = request.Pinterest ?? user.Socials.Pinterest;
            user.Socials.Twitter = request.Twitter ?? user.Socials.Twitter;

            var saved = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(saved) return Unit.Value;

            throw new Exception("problem saving changes");

        }
    }
}
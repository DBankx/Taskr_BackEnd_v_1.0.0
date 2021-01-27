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
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public UpdateProfileHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(),
                cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {errors = "You are unauthorized"});

            user.Bio = request.Description ?? user.Bio;
            user.Tagline = request.Tagline ?? user.Tagline;

            var socials = new Socials
            {
                Twitter = request.Twitter,
                Facebook = request.Facebook,
                Pinterest = request.Pinterest,
                Instagram = request.Instagram
            };

            user.Socials = socials;

            var saved = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(saved) return Unit.Value;

            throw new Exception("problem saving changes");
        }
    }
}
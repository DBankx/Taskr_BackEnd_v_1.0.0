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
    public class AddLanguageHandler : IRequestHandler<AddLanguageCommand>
    {
        private readonly IUserAccess _userAccess;
        private readonly DataContext _context;

        public AddLanguageHandler(IUserAccess userAccess, DataContext context)
        {
            _userAccess = userAccess;
            _context = context;
        }
        
        public async Task<Unit> Handle(AddLanguageCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken: cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var language = new Language 
            {
                LanguageName = request.LanguageName,
                ExperienceLevel= request.ExperienceLevel
            };
            
            user.Languages.Add(language);

            var changesSaved = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            if(changesSaved) return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}
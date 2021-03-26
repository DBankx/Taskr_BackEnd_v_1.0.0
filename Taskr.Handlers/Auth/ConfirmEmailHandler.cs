using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Persistance;

namespace Taskr.Handlers.Auth
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmail>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<Unit> Handle(ConfirmEmail request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) throw new RestException(HttpStatusCode.NotFound, new {error = "User not found"});
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Errors.Any())
                throw new RestException(HttpStatusCode.BadRequest,
                    new {error = $"An error occurred while confirming {user.Email}"});
            
            return Unit.Value;
        }
    }
}
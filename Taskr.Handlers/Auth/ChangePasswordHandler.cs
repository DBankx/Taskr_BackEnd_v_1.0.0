using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Auth
{
    public class ChangePasswordHandler : IRequestHandler<ChangePassword>
    {
        private readonly DataContext _dataContext;
        private readonly IUserAccess _userAccess;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordHandler(DataContext dataContext, IUserAccess userAccess, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userAccess = userAccess;
            _userManager = userManager;
        }
        
        public async Task<Unit> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized,
                    new {error = "You are unauthorized to complete this request"});
            
            
            var match = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

            if (!match)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new {currentPassword = "Current password is incorrect"});
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (result.Succeeded)
            {
                return Unit.Value;
            }

            throw new Exception("Problem resetting password");

        }
    }
}
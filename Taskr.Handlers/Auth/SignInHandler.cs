using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Taskr.Infrastructure.Jwt;
using Taskr.Persistance;

namespace Taskr.Handlers.Auth
{
    public class SignInHandler : IRequestHandler<SignInCommand, AuthResponse>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignInHandler(IJwtGenerator jwtGenerator, DataContext context, UserManager<ApplicationUser> userManager)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<AuthResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Errors = new[] {"Invalid credentials"}
                };
            }

            var passwordMatch = await _userManager.CheckPasswordAsync(user, request.Password);
            
            if (!passwordMatch)
            {
                return new AuthResponse
                {
                    Errors = new[] {"Invalid credentials"}
                };
            }

            var hasNotifs =
                await _context.UserNotifications.AnyAsync(
                    x => x.ToUserId == user.Id && x.Status == NotificationStatus.UnRead, cancellationToken: cancellationToken);
            var authUserResponse = new AuthUserResponse
            {
                Email = user.Email,
                Username = user.UserName,
                Avatar = user.Avatar,
                HasUnReadNotifications = hasNotifs,
                Id = user.Id
            };

            return new AuthResponse
            {
                Success = true,
                Token = _jwtGenerator.GenerateToken(user),
                User = authUserResponse
            };
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Auth;
using Taskr.Infrastructure.Jwt;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;
using Taskr.Queries.Auth;

namespace Taskr.Handlers.Auth
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, AuthResponse>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserAccess _userAccess;
        private readonly DataContext _context;

        public GetCurrentUserHandler(IJwtGenerator jwtGenerator, IUserAccess userAccess, DataContext context)
        {
            _jwtGenerator = jwtGenerator;
            _userAccess = userAccess;
            _context = context;
        }
        
        public async Task<AuthResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken: cancellationToken);

            if (user == null)
            {
                return new AuthResponse
                {
                    Errors = new[] {"User not found"}
                };
            }

            var authUserResponse = new AuthUserResponse
            {
                Avatar = user.Avatar,
                Email = user.Email,
                Username = user.UserName
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
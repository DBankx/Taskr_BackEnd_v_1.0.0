using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Auth;
using Taskr.Domain;
using Taskr.Dtos.Auth;
using Taskr.RepositoryServices.AuthService;

namespace Taskr.Handlers.Auth
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public SignUpHandler(IAuthService authService)
        {
            _authService = authService;
        }
        
        public async Task<AuthResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Bio = request.Bio,
                City = request.City,
                UserName = request.Username
            };

            return await _authService.SignUpAsync(user, request.Password);
        }
    }
}
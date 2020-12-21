using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Auth;
using Taskr.Dtos.Auth;
using Taskr.RepositoryServices.AuthService;

namespace Taskr.Handlers.Auth
{
    public class SignInHandler : IRequestHandler<SignInCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public SignInHandler(IAuthService authService)
        {
            _authService = authService;
        }
        
        public async Task<AuthResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SignInAsync(request.Email, request.Password);
        }
    }
}
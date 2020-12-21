using MediatR;
using Taskr.Dtos.Auth;

namespace Taskr.Commands.Auth
{
    public class SignInCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
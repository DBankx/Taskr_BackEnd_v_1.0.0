using MediatR;

namespace Taskr.Commands.Auth
{
    public class ChangePassword : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
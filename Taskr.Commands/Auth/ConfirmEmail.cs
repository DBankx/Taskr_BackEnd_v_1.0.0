using MediatR;

namespace Taskr.Commands.Auth
{
    public class ConfirmEmail : IRequest
    {
        public string Code { get; set; }
        public string UserId { get; set; }
        
        public ConfirmEmail(string userId, string code)
        {
            UserId = userId;
            Code = code;
        }
    }
}
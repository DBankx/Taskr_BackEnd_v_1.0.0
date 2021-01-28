using MediatR;

namespace Taskr.Commands.Profile
{
    public class UpdateSocialCommand : IRequest
    {
         public string Twitter { get; set; }
         public string Facebook { get; set; }
         public string Instagram { get; set; }
         public string Pinterest { get; set; } 
    }
}
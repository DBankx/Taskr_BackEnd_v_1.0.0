using MediatR;

namespace Taskr.Commands.Profile
{
    public class UpdateProfileCommand : IRequest
    {
        public string Description { get; set; }
        public string Tagline { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Pinterest { get; set; } 
    }
}
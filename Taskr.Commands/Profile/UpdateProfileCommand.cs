using MediatR;

namespace Taskr.Commands.Profile
{
    public class UpdateProfileCommand : IRequest
    {
        public string Description { get; set; }
        public string Tagline { get; set; }
       
    }
}
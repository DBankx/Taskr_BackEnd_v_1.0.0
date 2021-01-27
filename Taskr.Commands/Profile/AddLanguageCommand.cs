using MediatR;
using Taskr.Domain;

namespace Taskr.Commands.Profile
{
    public class AddLanguageCommand : IRequest
    {
        public string LanguageName { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
    }
}
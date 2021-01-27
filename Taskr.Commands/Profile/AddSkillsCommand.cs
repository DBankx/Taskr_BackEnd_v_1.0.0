using MediatR;
using Taskr.Domain;

namespace Taskr.Commands.Profile
{
    public class AddSkillsCommand : IRequest
    {
        public string SkillName { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
    }
}
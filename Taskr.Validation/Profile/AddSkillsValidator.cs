using FluentValidation;
using Taskr.Commands.Profile;
using Taskr.Domain;

namespace Taskr.Validation.Profile
{
    public class AddSkillsValidator : AbstractValidator<AddSkillsCommand>
    {
        public AddSkillsValidator()
        {
            RuleFor(x => x.SkillName).NotNull().NotEmpty();
            RuleFor(x => x.ExperienceLevel).IsInEnum();
        }
    }
}
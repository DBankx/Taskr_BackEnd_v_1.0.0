using FluentValidation;
using Taskr.Commands.Profile;
using Taskr.Domain;

namespace Taskr.Validation.Profile
{
    public class AddLanguageValidator : AbstractValidator<AddLanguageCommand>
    {
        public AddLanguageValidator()
        {
            RuleFor(x => x.LanguageName).NotNull().NotEmpty();
            RuleFor(x => x.ExperienceLevel).IsInEnum();
        }
    }
}
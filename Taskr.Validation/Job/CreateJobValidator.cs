using FluentValidation;
using Taskr.Commands.Task;

namespace Taskr.Validation.Job
{
    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.InitialPrice).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().NotNull();
        }
    }
}
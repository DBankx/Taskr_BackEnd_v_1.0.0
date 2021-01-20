using FluentValidation;
using Taskr.Commands.Task;
using Taskr.Domain;

namespace Taskr.Validation.Job
{
    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.InitialPrice).NotEmpty().NotNull().GreaterThan(0).WithMessage("Please specify a price for your task");
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Please specify a title for your task");
            RuleFor(x => x.Category).IsInEnum().NotNull().WithMessage("Please select a valid category");
            RuleFor(x => x.PostCode).NotNull().NotEmpty().WithMessage("Please input a post code");
        }
    }
}
using FluentValidation;
using Taskr.Commands.Auth;

namespace Taskr.Validation.Auth
{
    public class SignUpValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().EmailAddress().NotEmpty();
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Passwords must not be less than 6 chars").Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter").Matches("[a-z]").WithMessage("Password must have at least 1 lowercase char").Matches("[0-9]").WithMessage("Password contains at least one number").Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non Alphanumeric");
        }
    }
}
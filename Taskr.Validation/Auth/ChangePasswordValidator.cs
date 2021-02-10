using FluentValidation;
using Taskr.Commands.Auth;

namespace Taskr.Validation.Auth
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Passwords must not be less than 6 chars").Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter").Matches("[a-z]").WithMessage("Password must have at least 1 lowercase char").Matches("[0-9]").WithMessage("Password contains at least one number").Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non Alphanumeric");
        }
    }
}
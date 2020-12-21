using FluentValidation;
using Taskr.Commands.Auth;

namespace Taskr.Validation.Auth
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        public SignInValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
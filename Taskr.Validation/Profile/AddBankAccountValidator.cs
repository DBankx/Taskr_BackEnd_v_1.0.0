using FluentValidation;
using Taskr.Commands.Profile;

namespace Taskr.Validation.Profile
{
    
    public class AddBankAccountValidator : AbstractValidator<AddBankAccount>
    {
        public AddBankAccountValidator()
        {
            RuleFor(x => x.AccountNumber).NotNull().WithMessage("Account number cannot null").NotEmpty().WithMessage("Account number cannot be empty");
            RuleFor(x => x.RoutingNumber).NotNull().WithMessage("Routing number cannot null").NotEmpty()
                .WithMessage("Routing number cannot be empty").Length(9)
                .WithMessage("Please provide valid 9 digit routing number");
            RuleFor(x => x.AccountHolderName).NotNull().WithMessage("Account Holder account cannot be null").NotEmpty()
                .WithMessage("Account holder name cannot be empty");
            RuleFor(x => x.AccountHolderType).NotNull().WithMessage("Account holder type cannot be null").NotEmpty()
                .WithMessage("Account holder type cannot be empty");
        }
    }
}
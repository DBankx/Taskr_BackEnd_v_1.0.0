using FluentValidation;
using Taskr.Commands.Bid;

namespace Taskr.Validation.Bid
{
    public class CreateBidValidation : AbstractValidator<CreateBidCommand>
    {
        public CreateBidValidation()
        {
            RuleFor(x => x.Price).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
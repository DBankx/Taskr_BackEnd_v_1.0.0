using FluentValidation;
using Taskr.Commands.Review;

namespace Taskr.Validation.Review
{
    public class AddReviewValidator : AbstractValidator<AddReview>
    {
        public AddReviewValidator()
        {
            RuleFor(x => x.Rating).LessThanOrEqualTo(5).WithMessage("Rating must not be greater than 5").NotNull().WithMessage("Rating is required");
            RuleFor(x => x.Text).NotNull().WithMessage("Review text is required")
                .NotEmpty().WithMessage("Review text must not be empty");
        }
    }
}
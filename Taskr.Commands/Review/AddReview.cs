using MediatR;

namespace Taskr.Commands.Review
{
    public class AddReview : IRequest
    {
        public string OrderNumber { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
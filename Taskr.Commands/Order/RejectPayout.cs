using MediatR;

namespace Taskr.Commands.Order
{
    public class RejectPayout : IRequest
    {
        public string OrderNumber { get; set; }

        public RejectPayout(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
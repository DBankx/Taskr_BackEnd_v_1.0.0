using MediatR;

namespace Taskr.Commands.Order
{
    public class AcceptPayoutRequest : IRequest
    {
        public string OrderNumber { get; set; }

        public AcceptPayoutRequest(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
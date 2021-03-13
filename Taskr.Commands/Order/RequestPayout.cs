using MediatR;

namespace Taskr.Commands.Order
{
    public class RequestPayout : IRequest
    {
        public string OrderNumber { get; set; }

        public RequestPayout(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
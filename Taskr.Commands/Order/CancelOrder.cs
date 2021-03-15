using MediatR;

namespace Taskr.Commands.Order
{
    public class CancelOrder : IRequest
    {
        public string OrderNumber { get; set; }

        public CancelOrder(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
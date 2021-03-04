using MediatR;
using Taskr.Dtos.Order;

namespace Taskr.Queries.Order
{
    public class GetOrderByNumber : IRequest<OrderDetailsDto>
    {
        public string OrderNumber { get; set; }

        public GetOrderByNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
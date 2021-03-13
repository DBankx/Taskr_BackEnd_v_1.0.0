using System;
using MediatR;

namespace Taskr.Commands.Order
{
    public class DeleteOrderCommand : IRequest
    {
        public string OrderNumber { get; set; }

        public DeleteOrderCommand(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
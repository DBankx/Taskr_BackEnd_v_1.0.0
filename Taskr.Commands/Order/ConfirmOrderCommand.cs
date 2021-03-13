using System;
using MediatR;

namespace Taskr.Commands.Order
{
    public class ConfirmOrderCommand : IRequest
    {
        public string OrderNumber { get; set; }

        public ConfirmOrderCommand(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
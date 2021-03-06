using System;
using MediatR;

namespace Taskr.Queries.Order
{
    public class MarkOrderAsStarted : IRequest
    {
        public string OrderNumber { get; set; }

        public MarkOrderAsStarted(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
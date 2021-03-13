using System;
using Taskr.Domain;

namespace Taskr.Dtos.Order
{
    public class ListOrderDto
    {
        public string OrderNumber { get; set; }
        public OrderUser User { get; set; }
        public OrderListJob Job{ get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderPlacementDate { get; set; }
        public DateTime PaymentCompletedDate { get; set; }
    }
}
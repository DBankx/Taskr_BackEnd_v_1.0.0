using System;
using System.ComponentModel.DataAnnotations.Schema;
using Taskr.Domain;
using Taskr.Dtos.Chat;

namespace Taskr.Dtos.Order
{
    public class OrderDetailsDto
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public OrderUser User { get; set; }
        public OrderUser PayTo { get; set; }
        public OrderJob Job { get; set; }
        public DateTime OrderPlacementDate{ get; set; }
        public DateTime OrderStartedDate { get; set; }
        public OrderStatus Status{ get; set; }
        public OrderBid AcceptedBid { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal TotalAmount { get; set; }
        public DateTime PaymentCompletedDate { get; set; }
        public DateTime OrderCompletedDate { get; set; }
        public string CheckoutSessionId { get; set; }
        public ChatDto Chat { get; set; }
    }
}
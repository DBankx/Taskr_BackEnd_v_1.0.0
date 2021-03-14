using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskr.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public ApplicationUser User { get; set; }
        public Job Job { get; set; }
        public DateTime OrderPlacementDate{ get; set; }
        public OrderStatus Status{ get; set; }
        public Bid AcceptedBid { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal TotalAmount { get; set; }
        public ApplicationUser PayTo { get; set; }
        public DateTime OrderStartedDate { get; set; }
        public DateTime PaymentCompletedDate { get; set; }
        public DateTime OrderCompletedDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public string CheckoutSessionId { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled,
        Confirmed,
        AwaitingPayout,
        Started
    }
}
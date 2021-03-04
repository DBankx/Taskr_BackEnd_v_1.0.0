using System;
using System.Collections.Generic;

namespace Taskr.Dtos.Order
{
    public class OrderListJob
    {
        public Guid Id { get; set; }
        public ICollection<Domain.Photo> Photos { get; set; }
        public string Title { get; set; }
        public DateTime DeliveryDate{ get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Taskr.Domain;

namespace Taskr.Dtos.Order
{
    public class OrderJob
    {
        public Guid Id { get; set; }
        public virtual ICollection<Domain.Photo> Photos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal InitialPrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Address{ get; set; }
        public DeliveryTypes DeliveryType { get; set; }
        public string PostCode { get; set; }
        public int Views { get; set; }
        public Category Category { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
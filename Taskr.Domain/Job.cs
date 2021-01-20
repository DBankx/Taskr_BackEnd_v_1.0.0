﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskr.Domain
{
    public class Job
    {
        public Guid Id { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal InitialPrice { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveryDate { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Watch> Watching { get; set; }
        
        public string Address{ get; set; }
        public DeliveryTypes DeliveryType { get; set; }
        public string PostCode { get; set; }
        public int Views { get; set; }
        public Category Category { get; set; }
    }

    public enum Category
    {
        Cleaning,
        Catering,
        Laundry,
        Groceries,
        Digital,
        Errands,
        Delivery
    }

    public enum DeliveryTypes
    {
        InPerson,
        Online
    }
}
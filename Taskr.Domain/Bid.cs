﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Taskr.Domain;

namespace Taskr.Domain
{
    public class Bid
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public BidStatus Status { get; set; }
    }
    
    public enum BidStatus
        {
            Submitted,
            Seen,
            Rejected,
            Approved
        }
}
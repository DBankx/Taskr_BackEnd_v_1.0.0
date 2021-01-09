using System;
using System.ComponentModel.DataAnnotations.Schema;
using Taskr.Domain;

namespace Taskr.Dtos
{
    public class BidDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid JobId { get; set; }
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } 
        public BidStatus Status { get; set; }
    }
}
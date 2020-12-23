using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskr.Domain
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")] 
        public decimal? InitialPrice { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
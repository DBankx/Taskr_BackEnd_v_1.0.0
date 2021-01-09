using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Taskr.Dtos.Job
{
    public class AllJobsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public ICollection<Domain.Photo> Photos { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public JobCreatorDto Creator { get; set; }
    }
}
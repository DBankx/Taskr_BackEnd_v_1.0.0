using System;
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
        public string CreatorId { get; set; }
        public string CreatorUsername { get; set; }
    }
}
using System;

namespace Taskr.Domain
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
    }
}
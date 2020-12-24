using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class UpdateJobCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? InitialPrice { get; set; }

    }
}
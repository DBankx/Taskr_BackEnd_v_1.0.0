using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class UpdateTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? InitialPrice { get; set; }

    }
}
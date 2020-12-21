using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class CreateTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
    }
}
using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class WatchJobCommand : IRequest
    {
        public Guid JobId { get; set; }
    }
}
using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class UnWatchTaskCommand : IRequest
    {
        public Guid JobId{ get; set; }
    }
}
using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public Guid TaskId { get; set; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
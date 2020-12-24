using System;
using MediatR;

namespace Taskr.Commands.Task
{
    public class DeleteJobCommand : IRequest
    {
        public Guid JobId { get; set; }

        public DeleteJobCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}
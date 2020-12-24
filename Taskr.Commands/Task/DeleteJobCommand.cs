using System;
using MediatR;
using Taskr.Dtos.ApiResponse;

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
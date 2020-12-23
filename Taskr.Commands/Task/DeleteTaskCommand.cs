using System;
using MediatR;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Commands.Task
{
    public class DeleteTaskCommand : IRequest<ApiResponse<object>>
    {
        public Guid TaskId { get; set; }
        public string UserId { get; set; }

        public DeleteTaskCommand(Guid taskId, string userId)
        {
            TaskId = taskId;
            UserId = userId;
        }
    }
}
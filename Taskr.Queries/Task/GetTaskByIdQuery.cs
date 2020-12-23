using System;
using MediatR;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Queries
{
    public class GetTaskByIdQuery : IRequest<ApiResponse<Job>>
    {
        public Guid TaskId { get; set; }

        public GetTaskByIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
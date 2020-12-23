using System;
using MediatR;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Commands.Task
{
    public class CreateTaskCommand : IRequest<ApiResponse<object>>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public string UserId { get; set; }
    }
}
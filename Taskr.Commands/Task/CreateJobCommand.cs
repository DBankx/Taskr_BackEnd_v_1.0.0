using System;
using MediatR;
using Taskr.Dtos.ApiResponse;

namespace Taskr.Commands.Task
{
    /// <summary>
    /// TODO - add validation
    /// </summary>
    public class CreateJobCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public string UserId { get; set; }
    }
}
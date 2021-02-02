using System;
using MediatR;

namespace Taskr.Commands.Profile
{
    public class MarkNotificationAsReadCommand : IRequest
    {
        public Guid Id { get; set; }

        public MarkNotificationAsReadCommand(Guid id)
        {
            Id = id;
        }
    }
}
using System;
using MediatR;

namespace Taskr.Commands.Profile
{
    public class DeleteNotificationCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeleteNotificationCommand(Guid id)
        {
            Id = id;
        }
    }
}
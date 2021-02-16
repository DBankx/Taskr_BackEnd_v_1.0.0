using System;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Commands.Chat
{
    public class CreateChatCommand : IRequest<ChatCreateReturnDto>
    {
        public string TaskrId { get; set; }
        public Guid JobId { get; set; }

        public CreateChatCommand(string taskrId, Guid jobId)
        {
            TaskrId = taskrId;
            JobId = jobId;
        }
    }
}
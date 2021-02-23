using System;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Commands.Chat
{
    public class CreateChatCommand : IRequest
    {
        public string TaskrId { get; set; }
        public Guid JobId { get; set; }
        public string Text { get; set; }

        public CreateChatCommand(string taskrId, Guid jobId, string text)
        {
            TaskrId = taskrId;
            JobId = jobId;
            Text = text;
        }
    }
}
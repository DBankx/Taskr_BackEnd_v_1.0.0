using System;
using MediatR;

namespace Taskr.Commands.Chat
{
    public class DeleteChatCommand : IRequest
    {
        public Guid ChatId { get; set; }

        public DeleteChatCommand(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
using System;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Queries.Chat
{
    public class GetChatById : IRequest<SingleChatDto>
    {
        public Guid ChatId { get; set; }

        public GetChatById(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
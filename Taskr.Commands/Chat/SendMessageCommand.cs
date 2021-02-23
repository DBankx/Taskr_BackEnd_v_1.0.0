using System;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Commands.Chat
{
    public class SendMessageCommand : IRequest<MessageDto>
    {
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        
        public string SenderId { get; set; }
    }
}
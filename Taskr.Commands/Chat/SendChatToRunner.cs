using System;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Commands.Chat
{
    public class SendChatToRunner : IRequest<ChatDto>
    {
         public string RunnerId { get; set; }
         public Guid JobId { get; set; }
         public string Text { get; set; }
         
        public SendChatToRunner(string runnerid, Guid jobId, string text)
        { 
            RunnerId = runnerid; 
            JobId = jobId;
            Text = text;
        }
    }
}
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Chat;
using Taskr.Domain;
using Taskr.Dtos.Chat;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.MediatrNotifications;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Chat
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, MessageDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
    
        public SendMessageHandler(DataContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        
        public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.SenderId, cancellationToken);
            
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var receiver = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.ReceiverId, cancellationToken);

            if (receiver == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Receiver not found"});
            
            var chat = await _context.Chats.SingleOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

            if (chat == null) throw new RestException(HttpStatusCode.BadRequest, new {error = "Chat not found"});

            if (receiver.Id != chat.Runner.Id || receiver.Id != chat.Taskr.Id)
                throw new RestException(HttpStatusCode.BadRequest, new {error = "Receiver not in chat"});

            var message = new Message
            {
                Chat = chat,
                Receiver = receiver,
                Sender = user,
                SentAt = DateTime.Now,
                Text = request.Text
            };
            
            _context.Messages.Add(message);
            
            var created = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!created)
            {
                throw new Exception("Error occurred while creating bid");
            }
            
            var appUserNotif = new UserPrivateMessageNotification(receiver.Id, user.Id, user.UserName, user.Avatar, $"{user.UserName} sent you a message about {chat.Job.Title} ", chat.Id, DateTime.Now, NotificationType.Message, NotificationStatus.UnRead);

            _mediator.Publish(appUserNotif, cancellationToken);

            return _mapper.Map<MessageDto>(message);
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Chat;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Chat
{
    public class DeleteChatHandler : IRequestHandler<DeleteChatCommand>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public DeleteChatHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<Unit> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var chat = await _context.Chats.Include(x => x.Taskr).SingleOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

            if (chat == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Chat not found"});

            if (chat.Taskr.Id != user.Id)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                    new {error = "Only taskr's are allowed to delete chats"});
            }

            var messagesList = await _context.Messages.Where(x => x.Chat == chat).ToListAsync(cancellationToken);

            _context.Chats.Remove(chat);
            _context.Messages.RemoveRange(messagesList);

            var removed = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!removed)
            {
                throw new Exception("Problem saving changes");
            }            
            
            return Unit.Value;
        }
    }
}
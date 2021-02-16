using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Commands.Chat;
using Taskr.Dtos.Chat;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.Handlers.Chat
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, ChatCreateReturnDto>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public CreateChatHandler(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public async Task<ChatCreateReturnDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (loggedInUser == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var taskr = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.TaskrId, cancellationToken);
            
            if(taskr == null) 
                throw new RestException(HttpStatusCode.NotFound, new {error = "User not found"});

            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.JobId, cancellationToken);
            
            if(job == null)
                throw new RestException(HttpStatusCode.NotFound, new {error = "task not found"});
            
            if(job.UserId != taskr.Id)
                throw new RestException(HttpStatusCode.BadRequest, new {error = "task not owned by requested user"});

            var chat = await _context.Chats.SingleOrDefaultAsync(x =>
                x.Job.Id == job.Id && x.Runner.Id == loggedInUser.Id && x.Taskr.Id == taskr.Id);

            if (chat != null)
                throw new RestException(HttpStatusCode.BadRequest,
                    new {error = "You already have a chat with this taskr about this job"});

            chat = new Domain.Chat
            {
                Job = job,
                Taskr = taskr,
                Runner = loggedInUser,
                CreatedAt = DateTime.Now
            };

            _context.Chats.Add(chat);

            var saved = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!saved) throw new Exception("Problem saving changes");
            
            return new ChatCreateReturnDto{Id = chat.Id};
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Chat;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Queries.Chat;

namespace Taskr.Handlers.Chat
{
    public class GetAllChatsHandler : IRequestHandler<GetAllChats, List<ChatDto>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public GetAllChatsHandler(IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<List<ChatDto>> Handle(GetAllChats request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId());

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            List<Domain.Chat> chats = new List<Domain.Chat>();

            switch (request.Predicate)
            {
                case "AsTaskr":
                    chats = await _queryProcessor.Query<Domain.Chat>().Include(x => x.Job).ThenInclude(x => x.Photos)
                        .Include(x => x.Messages).Include(x => x.Taskr).Include(x => x.Runner)
                        .Where(x => x.Taskr.Id == user.Id).OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
                    break;
                case "AsRunner":
                    chats = await _queryProcessor.Query<Domain.Chat>().Include(x => x.Job).ThenInclude(x => x.Photos)
                        .Include(x => x.Messages).Include(x => x.Taskr).Include(x => x.Runner)
                        .Where(x => x.Runner.Id == user.Id).OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken); 
                    break;
                default:
                    break;
            }

            return _mapper.Map<List<ChatDto>>(chats);
        }
    }
}
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
    public class GetChatByIdHandler : IRequestHandler<GetChatById, SingleChatDto>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public GetChatByIdHandler(IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<SingleChatDto> Handle(GetChatById request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            var chat = await _queryProcessor.Query<Domain.Chat>().Include(x => x.Job).ThenInclude(x => x.Photos)
                .Include(x => x.Messages).ThenInclude(x => x.Sender)
                .SingleOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

            if (chat == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Chat not found"});

            return _mapper.Map<SingleChatDto>(chat);
        }
    }
}
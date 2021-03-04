using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Chat;
using Taskr.Dtos.Order;
using Taskr.Infrastructure.Helpers;

namespace Taskr.MappingProfiles.Order
{
    public class OrderChatResolver : IValueResolver<Domain.Order, OrderDetailsDto, ChatDto>
    {
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;

        public OrderChatResolver(IMapper mapper, IQueryProcessor queryProcessor)
        {
            _mapper = mapper;
            _queryProcessor = queryProcessor;
        }
        public ChatDto Resolve(Domain.Order source, OrderDetailsDto destination, ChatDto destMember, ResolutionContext context)
        {
            var chat = _queryProcessor.Query<Domain.Chat>().Include(x => x.Job).ThenInclude(x => x.Photos).Include(x => x.Messages).Include(x => x.Taskr).Include(x => x.Runner).SingleOrDefaultAsync(x =>
                x.Job == source.Job && x.Taskr == source.User && x.Runner == source.PayTo).Result;

            return _mapper.Map<ChatDto>(chat);
        }
    }
}
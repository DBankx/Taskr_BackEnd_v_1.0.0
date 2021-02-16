using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Chat;
using Taskr.Infrastructure.Helpers;

namespace Taskr.MappingProfiles.Chat
{
    public class LastMessageValueResolver : IValueResolver<Domain.Chat, ChatDto, string>
    {
        private readonly IQueryProcessor _queryProcessor;

        public LastMessageValueResolver(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }
        
        public string Resolve(Domain.Chat source, ChatDto destination, string destMember, ResolutionContext context)
        {
            var chat = _queryProcessor.Query<Domain.Chat>().Include(x => x.Messages)
                .SingleOrDefaultAsync(x => x.Id == source.Id).Result;

            var messages = chat.Messages.OrderByDescending(x => x.SentAt).ToList();

            return messages.First().Text;
        }
    }
}
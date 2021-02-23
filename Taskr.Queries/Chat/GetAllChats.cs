using System.Collections.Generic;
using MediatR;
using Taskr.Dtos.Chat;

namespace Taskr.Queries.Chat
{
    public class GetAllChats : IRequest<List<ChatDto>>
    {
        public string Predicate { get; set; }

        public GetAllChats(string predicate)
        {
            Predicate = predicate;
        }
    }
}
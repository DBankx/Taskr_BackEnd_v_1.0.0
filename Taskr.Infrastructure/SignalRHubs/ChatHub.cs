using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Taskr.Commands.Chat;

namespace Taskr.Infrastructure.SignalRHubs
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendMessage(SendMessageCommand command)
        {
            var senderId = GetUserId(); 

            command.SenderId = senderId;

            var message = await _mediator.Send(command);

            await Clients.Group(command.ChatId.ToString()).SendAsync("ReceiveMessage", message);
            
        }

        /// <summary>
        /// Gets the Id of the user from the context
        /// </summary>
        /// <returns></returns>
        private string GetUserId()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Adds the current user to the chat
        /// </summary>
        /// <param name="chatName">chatName is the id of the chat</param>
        /// <returns></returns>
        public async Task AddToChat(string chatName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
            var userId = GetUserId();
            await Clients.Group(chatName).SendAsync("Send", $"{userId} has joined the chat");
        }

        /// <summary>
        /// Removes the current user from the chat
        /// </summary>
        /// <param name="chatName">chatName is the id of the chat</param>
        /// <returns></returns>
        public async Task RemoveFromChat(string chatName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
            var userId = GetUserId();
            await Clients.Group(chatName).SendAsync("Send", $"{userId} has left the chat");
        }
    }
}
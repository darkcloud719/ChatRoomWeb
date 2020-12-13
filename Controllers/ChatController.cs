using ChatRoomWeb.Hubs;
using ChatRoomWeb.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoomWeb.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : BaseController
    {
        public IHubContext<ChatHub> _chat;

        public ChatController(IHubContext<ChatHub> chat) => _chat = chat;
    }
}
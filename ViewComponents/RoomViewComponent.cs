using System.Linq;
using System.Security.Claims;
using ChatRoomWeb.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomWeb.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ChatRoomDbContext _ctx;

        public RoomViewComponent(ChatRoomDbContext ctx) => _ctx = ctx;

        public IViewComponentResult Invoke()
        {
            var userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _ctx.ChatUsers
                        .Include(x => x.Chat)
                        .Where(x => x.UserId == userid && x.Chat.Type == Models.ChatType.Room)
                        .Select(x => x.Chat)
                        .ToList();
            return View(chats);
        }
    }
}